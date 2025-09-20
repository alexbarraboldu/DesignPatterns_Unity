using UnityEngine;

namespace Patterns.ServiceLocator
{
	public class Advertisement : IAdvertisementService
	{
		public void DisplayAd()
		{
			Debug.Log("Displaying video advertisement");
		}
	}
}
