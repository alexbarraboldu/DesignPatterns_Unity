using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Patterns.BehaviourTree;

[CustomPropertyDrawer(typeof(Node), true)]
public class NodeDrawer : PropertyDrawer
{
	// Add foldout toggle per-property instance
	private bool _isExpanded = true;

	private Type[] allTypes = null;
	private string[] allTypesName = null;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		if (property == null) return;
		
		if (allTypes == null) allTypes = GetAllNodeTypes();
		if (allTypesName == null) allTypesName = allTypes.Select(t => t.Name).ToArray();

		EditorGUI.BeginProperty(position, label, property);

		// Draw the foldout header
		_isExpanded = EditorGUI.Foldout(
			new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
			_isExpanded,
			property.displayName,
			true
		);

		if (_isExpanded)
		{
			EditorGUI.indentLevel++;

			// Adjust rect to start drawing below the foldout
			var contentRect = new Rect(
				position.x,
				position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
				position.width,
				position.height
			);

			// Draw a popup to switch Node subclass
			DrawTypeSelector(contentRect, property);

			if (property.managedReferenceValue != null)
			{
				// Move rect below type selector
				contentRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

				// Draw all serialized fields for this Node (auto layout)
				EditorGUI.PropertyField(contentRect, property, true);
			}

			EditorGUI.indentLevel--;
		}

		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		if (!_isExpanded)
			return EditorGUIUtility.singleLineHeight;

		if (property.managedReferenceValue == null)
			return EditorGUIUtility.singleLineHeight * 3f;

		// Dynamically calculate full height of nested serialized fields
		return EditorGUI.GetPropertyHeight(property, true) + EditorGUIUtility.singleLineHeight * 2f;
	}

	// Draw popup for switching Node subclass
	private void DrawTypeSelector(Rect position, SerializedProperty property)
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
