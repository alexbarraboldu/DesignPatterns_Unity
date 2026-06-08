using System;
using System.Reflection;

using Patterns.BehaviourTree;

using UnityEditor;

using UnityEngine;

using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(InterfaceReference<>))]
[CustomPropertyDrawer(typeof(InterfaceReference<,>))]
public class InterfaceReferenceDrawer : PropertyDrawer
{
	const string UnderlyingValueFieldName = "underlyingValue";

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var underlyingProperty = property.FindPropertyRelative(UnderlyingValueFieldName);
		var args = GetArguments(fieldInfo);

		EditorGUI.BeginProperty(position, label, property);

		var assignedObject = EditorGUI.ObjectField(position, label, underlyingProperty.objectReferenceValue, typeof(UnityEngine.Object), true);

		if (assignedObject != null)
		{
			underlyingProperty.objectReferenceValue = assignedObject;
		}

		EditorGUI.EndProperty();
	}

	private InterfaceArgs GetArguments(FieldInfo fieldInfo)
	{
		Type fieldType = fieldInfo.FieldType;

		if (!fieldType.IsGenericType)
		{
			throw new ArgumentException(
				$"{nameof(InterfaceReferenceDrawer)} can only be used with generic InterfaceReference fields."
			);
		}

		Type[] genericArguments = fieldType.GetGenericArguments();

		return genericArguments.Length switch
		{
			1 => new InterfaceArgs(
				typeof(Object),
				genericArguments[0]
			),

			2 => new InterfaceArgs(
				genericArguments[0],
				genericArguments[1]
			),

			_ => throw new ArgumentException(
				$"Unsupported InterfaceReference generic argument count: {genericArguments.Length}."
			)
		};
	}
}

public struct InterfaceArgs
{
	public readonly Type ObjectType;
	public readonly Type InterfaceType;

	public InterfaceArgs(Type objectType, Type interfaceType)
	{
		Debug.Assert(typeof(Object).IsAssignableFrom(objectType), "");
		Debug.Assert(interfaceType.IsInterface, "");

		ObjectType = objectType;
		InterfaceType = interfaceType;
	}
}
