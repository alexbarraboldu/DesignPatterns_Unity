using UnityEngine;

namespace Patterns.EventBus.Example
{
	public class ClientEventBus : MonoBehaviour
	{
		private bool _isButtonEnabled;

		private void Start()
		{
			gameObject.AddComponent<HUDController>();
			gameObject.AddComponent<CountdownTimer>();
			gameObject.AddComponent<BikeController>();

			_isButtonEnabled = true;
		}

		private void OnEnable()
		{
			RaceEventBus.Subscribe(RaceEventType.STOP, Restart);
		}

		private void OnDisable()
		{
			RaceEventBus.Unsubscribe(RaceEventType.STOP, Restart);
		}

		private void Restart()
		{
			_isButtonEnabled = true;
		}

		private void OnGUI()
		{
			if (_isButtonEnabled)
			{
				if (GUILayout.Button("Start Countdown"))
				{
					_isButtonEnabled = false;
					RaceEventBus.Publish(RaceEventType.COUNTDOWN);
				}
			}
		}
	}
}
