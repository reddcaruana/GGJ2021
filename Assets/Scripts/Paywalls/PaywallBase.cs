using UnityEngine;
using Assets.Scripts.Views;
using Assets.Scripts.AssetsManagers;

namespace Assets.Scripts.Paywalls
{
	public abstract class PaywallBase
	{
		public abstract FishRequired[] Required { get; }
		private string seasonStr;

		public bool HasView => view != null;
		private PaywallView view;

		public void Set(string seasonStr) =>
			this.seasonStr = seasonStr;

		public void CreateView(Transform parent)
		{
			view = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<GameObject>($"Prefabs/Paywalls/PaywallView{seasonStr}"), parent).AddComponent<PaywallView>();
			view.Set(Required, seasonStr);
			
			if (AllComplete())
			{
				view.PlayAnimation(false);
				view.Finished();
			}
			else
				view.PlayAnimation(true);
		}

		public void Refresh()
		{
			if (!HasView)
				return;

			view.Refresh();
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
