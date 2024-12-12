using UnityEngine;

namespace Patterns.Singleton.Example
{
	public class DemoSingleton : BaseSingleton<DemoSingleton>
	{
		private void OnGUI()
		{
			GUILayout.BeginArea(new Rect(0, 25, 130, 100));

			if (GUILayout.Button("Destroy Singleton"))
				Destroy(this.gameObject);

			GUILayout.EndArea();
		}
	}
}
