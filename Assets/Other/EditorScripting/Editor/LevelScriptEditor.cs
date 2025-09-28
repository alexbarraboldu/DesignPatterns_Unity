using UnityEditor;

[CustomEditor(typeof(LevelScript))]
public class LevelScriptEditor : Editor
{
	public override void OnInspectorGUI()
	{
		LevelScript myLeveScript = (LevelScript)target;

		myLeveScript.experience = EditorGUILayout.IntField("Experience", myLeveScript.experience);
		EditorGUILayout.LabelField("Level", myLeveScript.Level.ToString());
	}
}
