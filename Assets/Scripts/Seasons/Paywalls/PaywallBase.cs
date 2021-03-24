using System;
using UnityEngine;
using Assets.Scripts.Controllers;
using Assets.Scripts.Views.Seasons.Paywalls;
using Assets.Scripts.Player;

namespace Assets.Scripts.Seasons.Paywalls
{
	public abstract class PaywallBase
	{
		private Action onPaywallCompletedCallBack;

		public abstract FishRequired[] Required { get; }
		protected abstract Vector3[] FishRequiredLocalPosition { get; }

		public bool HasView => view != null;
		private PaywallView view;
		private bool IsOpen;

		public void Set(Action paywallCompletedCallBack)
		{
			onPaywallCompletedCallBack = paywallCompletedCallBack;
		}

		public void AssignView(PaywallView view) => this.view = view;

		public void SetView(Vector3 localPosition, string seasonStr, bool alreadyOpened)
		{
			view.transform.localPosition = localPosition;
			view.Set(Required, seasonStr, FishRequiredLocalPosition);

			IsOpen = alreadyOpened;

			if (alreadyOpened || TryOpen())
				OpenView(withCinematic: false);
			else
				view.PlayAnimation(true);
		}

		public void ReleaseView() 
		{
			view = null;
			for (int i = 0; i < Required.Length; i++)
				Required[i].ReleaseViews();
		}

		public void RefreshView()
		{
			if (!HasView)
				return;

			if (TryOpen())
			{
				OpenView(withCinematic: true, onAnimationComplete);

				void onAnimationComplete() => PlayerData.FishInventory.PayWallSubtraction(Required);
			}
		}

		private bool TryOpen()
		{
			if (IsOpen || !AllComplete())
				return false;

			IsOpen = true;
			return true;
		}

		public bool AllComplete()
		{
			if (IsOpen)
				return true;

			for (int i = 0; i < Required.Length; i++)
				if (!Required[i].IsComplete)
					return false;

			onPaywallCompletedCallBack();
			return true;
		}

		private void OpenView(bool withCinematic, Action onComplete = null)
		{
			if (!HasView)
			{
				onComplete?.Invoke();
				return;
			}


			if (withCinematic)
				SeasonCinematicController.OpenPaywall(Open);
			else
				Open();

			void Open()
			{
				view.PlayAnimation(false);
				view.Finished();
				onComplete?.Invoke();
			}
		}

		public void TryNotify()
		{
			if (!SeasonCinematicController.IsCinematic && IsOpen)
				return;

			uint toNotify;
			float startDelay = 0;
			for (int i = 0; i < Required.Length; i++)
			{
				toNotify = Required[i].ToBeNotifiedCount;
				if (toNotify > 0)
				{
					Required[i].Notify(startDelay);
					startDelay += FishRequired.DELAY_PER_NOTIFICATION * toNotify;
				}
			}
		}
	}
}
