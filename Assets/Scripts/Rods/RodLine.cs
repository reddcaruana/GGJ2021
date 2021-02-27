using System;
using UnityEngine;
using Assets.Scripts.Views.Rods;
using Assets.Scripts.AssetsManagers;

namespace Assets.Scripts.Rods
{
	public class RodLine
	{
		public bool HasView => view != null;
		private RodLineView view;

		public void CreateView(Transform parent) =>
			view = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<GameObject>("Prefabs/Rods/RodLine"), parent).AddComponent<RodLineView>();

		public void PositionUpdate(Vector3 startWorldPos, Vector3 endWorlPosition)
		{
			if (HasView)
				view.PositionUpdate(startWorldPos, endWorlPosition);
		}

		public void ProgressBarUpdate(float value, bool isRecovery, float duration = 0)
		{
			if (isRecovery)
				view.ProgressBarRecoveryUpdate(value, duration);
			else
				view.ProgressBarUpdate(value);
		}

		public void LineSlackWarning(bool value, float cycleDuration) => view.LineSlackWarning(value, cycleDuration);

		public void LineSnap(Action onComplete) => view.LineSnap(onComplete);

		public void Reset() => view.Reset();
	}
}
