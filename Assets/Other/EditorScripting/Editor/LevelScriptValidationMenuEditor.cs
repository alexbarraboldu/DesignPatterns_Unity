using UnityEditor;

using UnityEngine;

public class LevelScriptValidationMenuEditor : Editor
{
	[MenuItem("CONTEXT/LevelScript/DebugLevelScript")]
	[MenuItem("Assets/DebugLevelScript")]
	public static void DebugLevel()
	{
		GameObject selectedGameObject = Selection.activeGameObject;
		string debugString = "nothing selected";
		if (selectedGameObject)
		{
			LevelScript levelScript = selectedGameObject.GetComponent<LevelScript>();
			debugString = $"Selected object type: {levelScript.name}, Experience: {levelScript.experience}, Level: {levelScript.Level}";
		}

		Debug.Log($"{debugString}");
	}

	[MenuItem("Assets/DebugLevelScript", true)]
	public static bool Validate()
	{
		return Selection.activeObject.GetType() == typeof(LevelScript);
	}

	[MenuItem("CONTEXT/LevelScript/ResetLevelScript")]
	public static void ResetLevelScript(MenuCommand menuCommand)
	{
		var levelScript = menuCommand.context as LevelScript;
		levelScript.experience = 0;
	}
}
