using UnityEngine;

namespace Patterns.State.Example
{
	public class ClientState : MonoBehaviour
	{
		[SerializeField] private BikeController _bikeController;

		private void Awake()
		{
			_bikeController = FindFirstObjectByType<BikeController>();
		}

		private void OnGUI()
		{
			if (GUILayout.Button("Start Bike")) _bikeController.StartBike();
			if (GUILayout.Button("Turn Left")) _bikeController.TurnBike(Direction.LEFT);
			if (GUILayout.Button("Turn Right")) _bikeController.TurnBike(Direction.RIGHT);
			if (GUILayout.Button("Stop Bike")) _bikeController.StopBike();
		}
	}
}
