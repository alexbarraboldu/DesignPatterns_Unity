using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patterns.Singleton.Example
{
	public class Client : MonoBehaviour
	{
		private GameObject _demoSingleton;

		private void OnGUI()
		{
			if (GUILayout.Button("Instantiate Singleton"))
			{
				if (!DemoSingleton.Exist)
				{
					_demoSingleton = new GameObject();
					_demoSingleton.AddComponent<DemoSingleton>();
					_demoSingleton.name = "Demo Singleton";
				}
			}
		}
	}
}
