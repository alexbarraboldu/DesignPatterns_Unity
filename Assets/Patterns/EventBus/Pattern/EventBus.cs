using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace Patterns.EventBus
{
	public abstract class EventBus<E> : MonoBehaviour where E : Enum
	{
		private static readonly
			IDictionary<E, UnityEvent>
			Events = new Dictionary<E, UnityEvent>();

		public static void Subscribe(E eventType, UnityAction listener)
		{
			UnityEvent thisEvent;

			if (Events.TryGetValue(eventType, out thisEvent))
			{
				thisEvent.AddListener(listener);
			}
			else
			{
				thisEvent = new UnityEvent();
				thisEvent.AddListener(listener);
				Events.Add(eventType, thisEvent);
			}
		}

		public static void Unsubscribe(E eventType, UnityAction listener)
		{
			UnityEvent thisEvent;

			if (Events.TryGetValue(eventType, out thisEvent))
			{
				thisEvent.RemoveListener(listener);
			}
		}

		public static void Publish(E eventType)
		{
			UnityEvent thisEvent;

			if (Events.TryGetValue(eventType, out thisEvent))
			{
				thisEvent.Invoke();
			}
		}
	}
}
