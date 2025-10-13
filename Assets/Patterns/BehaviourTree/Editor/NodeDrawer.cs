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

	MonoBehaviour obj = null;


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
				DrawBehaviourObjectField(contentRect, property);
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
		height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

		if (property.managedReferenceValue is Task)
		{
			///	GameObject Behaviour field
			height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
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

	private void DrawBehaviourObjectField(Rect position, SerializedProperty property)
	{
		EditorGUI.LabelField(position, "Behaviour");

		string labelString = "null";

		position.x += EditorGUIUtility.labelWidth - 13;
		position.width /= 3f;
		obj = (MonoBehaviour)EditorGUI.ObjectField(position, obj, typeof(MonoBehaviour), true);

		if (obj != null)
		{
			if (property.managedReferenceValue is Action && obj.TryGetComponent(out IAction iAction))
			{
				labelString = "Action Loaded";

				property.serializedObject.Update();
				(property.managedReferenceValue as Action).action = iAction;
				property.serializedObject.ApplyModifiedProperties();
			}
			else if (property.managedReferenceValue is Condition && obj.TryGetComponent(out ICondition iCondition))
			{
				labelString = "Condition Loaded";

				property.serializedObject.Update();
				(property.managedReferenceValue as Condition).condition = iCondition;
				property.serializedObject.ApplyModifiedProperties();
			}
		}

		position.x += position.width;
		EditorGUI.LabelField(position, labelString);
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
}
