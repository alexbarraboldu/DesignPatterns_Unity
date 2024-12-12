using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patterns.EventBus.Example
{
	public class CountdownTimer : MonoBehaviour
	{
		private float _currentTime;
		private float _duration = 3.0f;

		private void OnEnable()
		{
			RaceEventBus.Subscribe(RaceEventType.COUNTDOWN, StartTimer);
		}

		private void OnDisable()
		{
			RaceEventBus.Unsubscribe(RaceEventType.COUNTDOWN, StartTimer);
		}

		private void StartTimer()
		{
			StartCoroutine(Countdown());
		}

		private IEnumerator Countdown()
		{
			_currentTime = _duration;
			
			while (_currentTime > 0)
			{
				yield return new WaitForSeconds(1f);
				_currentTime--;
			}

			RaceEventBus.Publish(RaceEventType.START);
		}

		private void OnGUI()
		{
			GUI.color = Color.blue;
			GUI.Label(new Rect(125, 0, 100, 20), "COUNTDOWN: " + _currentTime);
		}
	}
}
