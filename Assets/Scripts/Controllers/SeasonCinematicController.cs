using System;
using Assets.Scripts.Player;
using Assets.Scripts.Constants;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Seasons.Paywalls;

namespace Assets.Scripts.Controllers
{
	public static class SeasonCinematicController
	{
		public static bool IsCinematic { get; private set; }

		public static void TrySeasonIntro(Action onComplete = null)
		{
			if (IsCinematic)
				return;

			if (PlayerData.SeasonData.LastSeasonCinematic.Value != SeasonAreaType.MAX &&
				ViewController.CurrentSeason.Type <= PlayerData.SeasonData.LastSeasonCinematic.Value)
				return;

			IsCinematic = true;
			ViewController.MakeSeasonIntro(OnComplete);
			void OnComplete()
			{
				PlayerData.SeasonData.LastSeasonCinematic.Value = ViewController.CurrentSeason.Type;
				IsCinematic = false;
				onComplete?.Invoke();
			}
		}

		public static void OpenPaywall(Action onOpen, Action onComplete = null)
		{
			if (IsCinematic)
				return;

			IsCinematic = true;

			float waitTime = 0;
			for (int i = 0; i < ViewController.CurrentSeason.PayWall.Required.Length; i++)
				waitTime += ViewController.CurrentSeason.PayWall.Required[i].ToBeNotifiedCount;

			waitTime *= FishRequired.DELAY_PER_NOTIFICATION;
			waitTime += FishRequired.NOTIFICATION_DURATION;

			ViewController.MoveToPayWall((initYPos) =>
			{
				CoroutineRunner.Wait(waitTime, onComplete: () =>
				{
					onOpen();
					ViewController.MoveBackFromPaywall(initYPos, OnComplete);
				});
			});

			void OnComplete()
			{
				IsCinematic = false;
				onComplete?.Invoke();
			}
		}
	}
}
