using UnityEngine;
using Assets.Scripts.Views.Seasons.Paywalls;

namespace Assets.Scripts.Seasons.Paywalls
{
	public abstract class PaywallBase
	{
		public abstract FishRequired[] Required { get; }
		protected abstract Vector3[] FishRequiredLocalPosition { get; }
		private string seasonStr;

		public bool HasView => view != null;
		private PaywallView view;

		public void Set(string seasonStr) =>
			this.seasonStr = seasonStr;

		public void AssignView(PaywallView view) => this.view = view;

		public void SetView(Vector3 localPosition)
		{
			view.transform.localPosition = localPosition;
			view.Set(Required, seasonStr, FishRequiredLocalPosition);

			if (!TryOpen())
				view.PlayAnimation(true);
		}

		public void ReleaseView() => view = null;

		public void Refresh()
		{
			if (!HasView)
				return;

			view.Refresh();

			TryOpen();
		}

		private bool TryOpen()
		{
			if (!AllComplete())
				return false;
			
			view.PlayAnimation(false);
			view.Finished();
			return true;
		}

		public bool AllComplete()
		{
			for (int i = 0; i < Required.Length; i++)
				if (!Required[i].IsComplete)
					return false;

			return true;
		}
	}
}
