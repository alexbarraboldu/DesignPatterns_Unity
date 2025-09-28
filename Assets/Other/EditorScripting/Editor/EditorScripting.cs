using UnityEditor;

using UnityEngine;

public class EditorScripting : EditorWindow
{
	string custString = "String Here";
	bool groupEnabled;
	bool optionalSettings = true;
	float jumpMod = 1.0f;
	float impactMod = 0.5f;


	[MenuItem("Window / Custom Controls")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(EditorScripting));
		//EditorWindow.GetWindow<EditorScripting>();
	}

	private void OnGUI()
	{
		GUILayout.Label("Base Settings", EditorStyles.boldLabel);
		custString = EditorGUILayout.TextField("Text Field", custString);

		groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
		optionalSettings = EditorGUILayout.Toggle("Double Jumping Enabled", optionalSettings);
		jumpMod = EditorGUILayout.Slider("Jump Modifier", jumpMod, -5, 5);
		impactMod = EditorGUILayout.Slider("Impact Modifier", impactMod, -5, 5);
		EditorGUILayout.EndToggleGroup();

		GUI.backgroundColor = Color.red;

		GUILayout.FlexibleSpace();
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Reset", GUILayout.Width(100), GUILayout.Height(30)))
		{
			custString = "String Here";
			optionalSettings = true;
			jumpMod = 1.0f;
			impactMod = 0.5f;
		}
		EditorGUILayout.EndHorizontal();
	}
}
