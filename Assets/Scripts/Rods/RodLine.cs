using System;
using UnityEngine;
using Assets.Scripts.Views.Rods;
using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.Rods
{
	public class RodLine
	{
		public bool HasView => view != null;
		private RodLineView view;

		public void CreateView(Transform parent) =>
			view = MonoBehaviour.Instantiate(AssetLoader.ME.Load<GameObject>("Prefabs/Rods/RodLine"), parent).AddComponent<RodLineView>();

		public void DisplayViewInUI()
		{
			if (!HasView)
				return;

			view.DisplayInUi();
		}

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

		public void LineSnap(Action onComplete = null) => view.LineSnap(onComplete);

		public void SetViewActive(bool value)
		{
			if (!HasView)
				return;
			view.gameObject.SetActive(value);
		}

		public void Reset() => view.Reset();
	}
}
