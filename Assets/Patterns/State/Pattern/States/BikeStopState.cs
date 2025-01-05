using UnityEngine;

namespace Patterns.State
{
	public class BikeStopState : MonoBehaviour, IBikeState
	{
		private BikeController _bikeController;

		public void Handle(BikeController controller)
		{
			if (!_bikeController)
				_bikeController = controller;

			_bikeController.CurrentSpeed = 0;
		}
	}
}
