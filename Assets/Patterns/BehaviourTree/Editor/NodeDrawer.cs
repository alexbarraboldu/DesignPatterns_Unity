using System;
using System.Linq;

using Patterns.BehaviourTree;

using UnityEditor;

using UnityEngine;

using Action = Patterns.BehaviourTree.Action;

[CustomPropertyDrawer(typeof(Node), true)]
public class NodeDrawer : PropertyDrawer
{
	// Add foldout toggle per-property instance
	private bool _isExpanded = true;

	private Type[] allTypes = null;
	private string[] allTypesName = null;

	float inlinePropertyHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;


	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		if (property == null) return;

		if (allTypes == null) allTypes = GetAllNodeTypes();
		if (allTypesName == null) allTypesName = allTypes.Select(t => t.Name).ToArray();


		EditorGUI.BeginProperty(position, label, property);

		/// FOLDOUT HEADER
		_isExpanded = EditorGUI.Foldout(
			new Rect(position.x, position.y, 10, inlinePropertyHeight),
			_isExpanded,
			"",
			true
		);

		/// Adjust rect to start drawing below the foldout
		var contentRect = new Rect(
			position.x,
			position.y,
			position.width,
			EditorGUIUtility.singleLineHeight
		);

		/// Draw a popup to switch Node subclass
		DrawNodeTypeSelector(contentRect, property);

		if (_isExpanded && property.managedReferenceValue != null)
		{
			EditorGUI.indentLevel++;

			contentRect.y += inlinePropertyHeight;
			DrawNodeStatusTypeSelector(ref contentRect, property);

			if (property.managedReferenceValue is Task)
			{
				contentRect.y += inlinePropertyHeight;

				TaskDrawerHelper.DrawBehaviourSelector(contentRect, property);
				//TaskDrawerHelper.DrawInterfaceReferenceField(contentRect, property, label);
			}
			else if (property.managedReferenceValue is Composite)
			{
				EditorGUI.indentLevel++;

				SerializedProperty nodeArray = property.FindPropertyRelative("nodes");
				float nodeArrayHeight = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("nodes"), true);

				contentRect.y += inlinePropertyHeight;
				contentRect.height = nodeArrayHeight;

				EditorGUI.PropertyField(contentRect, nodeArray, true);

				EditorGUI.indentLevel--;
			}

			EditorGUI.indentLevel--;
		}

		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		float height = EditorGUIUtility.singleLineHeight; // Foldout

		if (!_isExpanded || property.managedReferenceValue == null)
			return height;

		/// Node status
		height += inlinePropertyHeight;

		if (property.managedReferenceValue is Task)
		{
			///	GameObject Behaviour field
			height += inlinePropertyHeight;
		}
		else if (property.managedReferenceValue is Composite)
		{
			///	Node Array
			SerializedProperty nodeArray = property.FindPropertyRelative("nodes");
			float nodeArrayHeight = EditorGUI.GetPropertyHeight(nodeArray, true);
			height += nodeArrayHeight + EditorGUIUtility.standardVerticalSpacing;
		}

		return height;
	}

	private void DrawNodeTypeSelector(Rect position, SerializedProperty property)
	{
		var nodeType = property.managedReferenceValue == null ? typeof(Node) : property.managedReferenceValue.GetType();

		int defaultIndex = Array.FindIndex(allTypes, t => t == nodeType);

		int newIndex = EditorGUI.Popup(
			new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
			"Node Type",
			Mathf.Max(defaultIndex, 0),
			allTypesName
		);

		if (newIndex != defaultIndex && newIndex >= 0)
		{
			property.serializedObject.Update();
			property.managedReferenceValue = allTypes[newIndex] == typeof(Node) ? null : Activator.CreateInstance(allTypes[newIndex]);
			property.serializedObject.ApplyModifiedProperties();
		}
	}

	private void DrawNodeStatusTypeSelector(ref Rect position, SerializedProperty property)
	{
		GUI.enabled = false;

		var nodeStatus = (property.managedReferenceValue as Node).status;
		var currentNodeStatus = nodeStatus;

		currentNodeStatus = (NodeStatus)EditorGUI.EnumPopup(position, "Node Status", currentNodeStatus);

		if (nodeStatus != currentNodeStatus)
		{
			property.serializedObject.Update();
			(property.managedReferenceValue as Node).status = currentNodeStatus;
			property.serializedObject.ApplyModifiedProperties();
		}

		GUI.enabled = true;
	}

	private static Type[] GetAllNodeTypes()
	{
		var baseType = typeof(Node);
		var types = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(a => a.GetTypes())
			.Where(t => baseType.IsAssignableFrom(t) && !t.IsAbstract)
			.OrderBy(t => t.Name)
			.ToList();

		types.Add(baseType);

		return types.ToArray();
	}

	public void DrawBehaviourSelector(Rect position, SerializedProperty property)
	{
		var treeContext = (property.serializedObject.targetObject as BehaviourTreeContext);

		if (treeContext == null) return;

		bool isActionNode = property.managedReferenceValue is Action;
		bool isConditionNode = property.managedReferenceValue is Condition;

		MonoBehaviour[] allBehaviours = treeContext.GetComponentsInChildren<MonoBehaviour>(true).Where(behaviour => isActionNode && behaviour is IAction || isConditionNode && behaviour is ICondition).ToArray();

		if (allBehaviours.Length == 0)
		{
			EditorGUI.LabelField(position, "Select behaviour", "No valid behaviours found");
			return;
		}

		string[] allBehavioursNames = allBehaviours.Select(t => t.name).ToArray();

		MonoBehaviour currentBehaviour = null;

		if (property.managedReferenceValue is Action action)
		{
			currentBehaviour = action.action as MonoBehaviour;
		}
		else if (property.managedReferenceValue is Condition condition)
		{
			currentBehaviour = condition.condition as MonoBehaviour;
		}

		int currentIndex = Array.IndexOf(allBehaviours, currentBehaviour);
		if (currentIndex < 0) currentIndex = 0;


		EditorGUI.BeginChangeCheck();

		int newIndex = EditorGUI.Popup(
			new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
			"Select behaviour",
			currentIndex,
			allBehavioursNames
		);

		if (EditorGUI.EndChangeCheck())
		{
			MonoBehaviour selectedBehaviour = allBehaviours[newIndex];

			if (property.managedReferenceValue is Action selectedAction)
			{
				selectedAction.action = selectedBehaviour as IAction;
			}
			else if (property.managedReferenceValue is Condition selectedCondition)
			{
				selectedCondition.condition = selectedBehaviour as ICondition;
			}
		}
	}
}
