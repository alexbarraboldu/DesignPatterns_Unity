using UnityEngine;

namespace Patterns.State
{
	public class BikeStartState : MonoBehaviour, IBikeState
	{
		private BikeController _bikeController;

		public void Handle(BikeController controller)
		{
			if (!_bikeController) _bikeController = controller;

			_bikeController.CurrentSpeed = _bikeController.maxSpeed;
		}

		public void Update()
		{
			if (!_bikeController) return;

			if (_bikeController.CurrentSpeed > 0)
			{
				_bikeController.transform.Translate(Vector3.forward * (_bikeController.CurrentSpeed * Time.deltaTime));
			}
		}
	}
}
