using System;
using UnityEngine;
using static Assets.Scripts.Framework.Ui.RDUiTransitionAttachment;

namespace Assets.Scripts.Framework.Ui
{
	public class RDUiTransitionElement : RDUiObject
	{
		private RDUiTransitionAttachment uiTransition = new RDUiTransitionAttachment();
		public bool IsMovable { get => uiTransition.isMovable; set => uiTransition.isMovable = value; }
		public float HideTime { get => uiTransition.hideTime; set => uiTransition.hideTime = value; }
		public float ShowTime { get => uiTransition.showTime; set => uiTransition.showTime = value; }

		public Vector2 ShowPos { get => uiTransition.showPos; set => uiTransition.showPos = value; }
		public Vector2 HidePos { get => uiTransition.hidePos; set => uiTransition.hidePos = value; }

		public State CurrentState => uiTransition.CurrentState;

		public bool DisableOnHide { get => uiTransition.disableOnHide; set => uiTransition.disableOnHide = value; }

		public void InitTransitions(RDUiTransitionAttachment.State state, bool autoGrabMissingPosFromAwake = true) => 
			uiTransition.Init(rectTransform, state, autoGrabMissingPosFromAwake);

		public virtual void Show(Action onComplete = null) => uiTransition.Show(onComplete);
		public virtual void ShowInstantly() => uiTransition.ShowInstantly();
		public virtual void Hide(Action onComplete = null) => uiTransition.Hide(onComplete);
		public virtual void HideInstantly() => uiTransition.HideInstantly();
	}
}
