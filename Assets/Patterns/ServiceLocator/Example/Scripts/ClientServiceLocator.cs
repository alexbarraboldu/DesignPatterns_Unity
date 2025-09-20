using UnityEngine;

namespace Patterns.ServiceLocator
{
	public class ClientServiceLocator : MonoBehaviour
	{
		private void Start()
		{
			RegisterServices();
		}

		private void RegisterServices()
		{
			ILoggerService logger = new Logger();
			ServiceLocator.RegisterService(logger);

			IAnalyticsService analytics = new Analytics();
			ServiceLocator.RegisterService(analytics);

			IAdvertisementService advertisement = new Advertisement();
			ServiceLocator.RegisterService(advertisement);
		}

		private void OnGUI()
		{
			GUILayout.Label("Review output in the console:");

			if (GUILayout.Button("Log Event"))
			{
				ILoggerService logger = ServiceLocator.GetService<ILoggerService>();
				logger.Log("Hello World");
			}

			if (GUILayout.Button("Send Analytics"))
			{
				IAnalyticsService analytics = ServiceLocator.GetService<IAnalyticsService>();
				analytics.SendEvent("Hello World!");
			}

			if (GUILayout.Button("Display Advertisement"))
			{
				IAdvertisementService advertisement = ServiceLocator.GetService<IAdvertisementService>();
				advertisement.DisplayAd();
			}
		}
	}
}
