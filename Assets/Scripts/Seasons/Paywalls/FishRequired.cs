using Assets.Scripts.Player;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Views.Seasons.Paywalls;

namespace Assets.Scripts.Seasons.Paywalls
{
	/* ---- TO DO -----
	 *  This should no longer get the data from the logbook but instead save paywall data
	 *  Since like this it is not possible to have the same fish reuiqrment in two different paywalls
	 */

	public class FishRequired
	{
		public const float DELAY_PER_NOTIFICATION = 0.2f;
		public const float NOTIFICATION_DURATION = 2f;

		public readonly FishTypeData Type;
		public readonly uint Required;
		public bool IsComplete => Obtained >= Required;
		public uint ToBeNotifiedCount => Obtained - SeenInPayWallCount;
		public bool HasNotifications => ToBeNotifiedCount > 0;
		public uint SeenInPayWallCount { get; private set; }
		public uint Obtained
		{
			get
			{
				if (Index == -1)
					return 0;

				uint obtained = PlayerData.FishInventory.GetDataAt(Index).Total;
				if (obtained > Required)
					return Required;
				return obtained;
			}
		}

		public bool HasView => views[0] != null;
		private PaywallRequiredView[] views;


		private int index = -1;
		private int Index 
		{
			get
			{
				if (index == -1)
					index = PlayerData.FishInventory.IndexOf(Type);
				return index;
			}
		}

		public FishRequired(FishTypeData type, uint count)
		{
			Type = type;
			Required = count;
			views = new PaywallRequiredView[Required];
		}

		public void AssignView(PaywallRequiredView view)
		{
			for (int i = 0; i < views.Length; i++)
			{
				if (views[i] == null)
				{
					view.Set(Type);

					RefreshView(view);

					views[i] = view;
					return;
				}
			}
			RDebugUtils.LogError("[FishRequired] No space to put View");
		}

		public void ReleaseViews()
		{
			for (int i = 0; i < views.Length; i++)
				views[i] = null;
		}

		public void RefreshViews()
		{
			if (!HasView)
				return;

			for (int i = 0; i < SeenInPayWallCount; i++)
			{
				if (views[i] != null)
					RefreshView(views[i]);
			}
		}

		private void RefreshView(PaywallRequiredView view) => view.Refresh(IsComplete);

		public bool TryNotify(float startDelay)
		{
			if (!HasNotifications)
				return false;

			Notify(startDelay);
			return true;
		}

		public void Notify(float startDelay)
		{
			int startAt = (int)SeenInPayWallCount;
			SeenInPayWallCount += ToBeNotifiedCount;

			if (startDelay > 0)
				CoroutineRunner.Wait(startDelay, Continue);
			else
				Continue();

			void Continue()
			{
				uint obtained = Obtained;
				for (int i = startAt; i < obtained; i++)
					views[i].Notify(DELAY_PER_NOTIFICATION * i, NOTIFICATION_DURATION);
			}
		}
	}
}