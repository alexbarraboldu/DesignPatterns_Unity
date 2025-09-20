using UnityEngine;

namespace Patterns.Observer.Example
{
	public class BikeController : Subject
	{
		public bool IsTurboOn
		{
			get;
			private set;
		}

		public float CurrentHealth => health;

		private bool _isEngineOn;
		private HUDController _hudController;
		private CameraController _cameraController;

		[SerializeField] private float health = 100.0f;

		private void Awake()
		{
			_hudController = gameObject.AddComponent<HUDController>();
			_cameraController = FindFirstObjectByType<CameraController>();
		}

		private void Start()
		{
			StartEngine();
		}

		private void OnEnable()
		{
			if (_hudController) Attach(_hudController);
			if (_cameraController) Attach(_cameraController);
		}
		private void OnDisable()
		{
			if (_hudController) Detach(_hudController);
			if (_cameraController) Detach(_cameraController);
		}

		private void StartEngine()
		{
			_isEngineOn = true;
			NotifyObservers();
		}

		public void ToggleTurbo()
		{
			if (_isEngineOn) IsTurboOn = !IsTurboOn;

			NotifyObservers();
		}

		public void TakeDamage(float amount)
		{
			health -= amount;
			IsTurboOn = false;

			NotifyObservers();

			if (health < 0) Destroy(gameObject);
		}
	}
}
