using System;
using UnityEngine;

namespace Assets.Scripts.Framework.Ui
{
	public class UiTransitionElement : RUiObject
	{
		private UiTransitionAttachment uiTransition = new UiTransitionAttachment();
		public bool IsMovable { get => uiTransition.isMovable; set => uiTransition.isMovable = value; }
		public float HideTime { get => uiTransition.hideTime; set => uiTransition.hideTime = value; }
		public float ShowTime { get => uiTransition.showTime; set => uiTransition.showTime = value; }

		public Vector2 ShowPos { get => uiTransition.showPos; set => uiTransition.showPos = value; }
		public Vector2 HidePos { get => uiTransition.hidePos; set => uiTransition.hidePos = value; }

		public void InitTransitions(UiTransitionAttachment.State state, bool autoGrabMissingPosFromAwake = true) => 
			uiTransition.Init(rectTransform, state, autoGrabMissingPosFromAwake);

		public virtual void Show(Action onComplete = null) => uiTransition.Show(onComplete);
		public void ShowInstantly() => uiTransition.ShowInstantly();
		public void Hide(Action onComplete = null) => uiTransition.Hide(onComplete);
		public void HideInstantly() => uiTransition.HideInstantly();
	}
}
