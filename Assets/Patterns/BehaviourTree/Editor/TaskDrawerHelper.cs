using System;
using System.Linq;

using Patterns.BehaviourTree;

using UnityEditor;

using UnityEngine;

using Action = Patterns.BehaviourTree.Action;

public static class TaskDrawerHelper
{
	public static void DrawBehaviourObjectField(Rect position, SerializedProperty property)
	{
		EditorGUI.LabelField(position, "Behaviour");

		string labelString = "null";

		position.x += EditorGUIUtility.labelWidth - 13;
		position.width /= 3f;

		MonoBehaviour obj = null;
		obj = (MonoBehaviour)EditorGUI.ObjectField(position, obj, typeof(MonoBehaviour), true);

		if (obj != null)
		{
			if (property.managedReferenceValue is Action && obj.TryGetComponent(out IAction iAction))
			{
				labelString = $"{typeof(Action).Name} Loaded";

				property.serializedObject.Update();
				(property.managedReferenceValue as Action).action = iAction;
				property.serializedObject.ApplyModifiedProperties();
			}
			else if (property.managedReferenceValue is Condition && obj.TryGetComponent(out ICondition iCondition))
			{
				labelString = $"{typeof(Condition).Name} Loaded";

				property.serializedObject.Update();
				(property.managedReferenceValue as Condition).condition = iCondition;
				property.serializedObject.ApplyModifiedProperties();
			}
		}

		position.x += position.width;
		EditorGUI.LabelField(position, labelString);
	}

	public static void DrawBehaviourSelector(Rect position, SerializedProperty property)
	{
		var treeContext = property.serializedObject.targetObject as BehaviourTreeContext;

		if (treeContext == null)
		{
			EditorGUI.LabelField(position, "Select behaviour", "No BehaviourTreeContext found");
			return;
		}

		if (property.managedReferenceValue is not Task task)
		{
			EditorGUI.LabelField(position, "Select behaviour", "Not a task");
			return;
		}

		treeContext.CacheBehaviours();

		bool isAction = task is Patterns.BehaviourTree.Action;
		bool isCondition = task is Condition;

		var validBehaviours = treeContext.BehavioursById
			.Where(pair =>
				isAction && pair.Value is IAction ||
				isCondition && pair.Value is ICondition)
			.ToArray();

		if (validBehaviours.Length == 0)
		{
			EditorGUI.LabelField(position, "Select behaviour", "No valid behaviours found");
			return;
		}

		string[] behaviourNames = validBehaviours
			.Select(pair => $"{pair.Value.gameObject.name} / {pair.Value.GetType().Name}")
			.ToArray();

		string[] behaviourIds = validBehaviours
			.Select(pair => pair.Key)
			.ToArray();

		SerializedProperty selectedBehaviourIdProperty =
			property.FindPropertyRelative(nameof(Task.SelectedBehaviourId));

		string currentId = selectedBehaviourIdProperty.stringValue;

		int currentIndex = Array.IndexOf(behaviourIds, currentId);

		if (currentIndex < 0)
		{
			currentIndex = 0;
		}

		EditorGUI.BeginChangeCheck();

		int newIndex = EditorGUI.Popup(
			new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
			"Select behaviour",
			currentIndex,
			behaviourNames
		);

		if (EditorGUI.EndChangeCheck())
		{
			selectedBehaviourIdProperty.stringValue = behaviourIds[newIndex];
			property.serializedObject.ApplyModifiedProperties();

			task.SelectedBehaviourId = behaviourIds[newIndex];
			task.ResolveBehaviour(treeContext);

			EditorUtility.SetDirty(property.serializedObject.targetObject);
		}
	}

	public static void DrawInterfaceReferenceField(Rect position, SerializedProperty property, GUIContent label)
	{
		string refInterface = property.managedReferenceValue is Action ? "refAction" : "refCondition";
		var refTask = property.FindPropertyRelative(refInterface);
		EditorGUI.PropertyField(position, refTask/*, label*/, true);
	}
}
