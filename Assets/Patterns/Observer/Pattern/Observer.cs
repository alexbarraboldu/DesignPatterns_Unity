using UnityEngine;

namespace Patterns.Observer
{
	public abstract class Observer : MonoBehaviour
	{
		public abstract void Notify(Subject subject);
	}
}
