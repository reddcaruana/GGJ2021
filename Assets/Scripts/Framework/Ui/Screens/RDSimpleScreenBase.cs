using System;
using UnityEngine;
using Assets.Scripts.Ui.Screens;

namespace Assets.Scripts.Framework.Ui.Screens
{
	public abstract class RDSimpleScreenBase : RDUiTransitionElement
	{
		public abstract int Index { get; }
		protected RDSimpleScreenBase PreviousScreen { get; private set; }
		protected virtual bool UseHistory { get; } = false;

		protected virtual void Awake()
		{
			InitTransitionElement();
		}

		protected virtual void InitTransitionElement()
		{
			IsMovable = true;
			HideTime = ShowTime = 0.3f;
			InitTransitions(RDUiTransitionAttachment.State.Visible);
			HidePosition();
			HideInstantly();
			DisableOnHide = true;
		}

		protected virtual void HidePosition()
		{
			Vector2 hiddenPos = rectTransform.anchoredPosition;
			hiddenPos.y -= rectTransform.rect.height;
			HidePos = hiddenPos;
		}

		public bool ManagedShow(RDSimpleScreenBase lastActiveScreen, Action onComplete)
		{
			if (Show(onComplete))
			{
				if (UseHistory)
					PreviousScreen = lastActiveScreen;

				return true;
			}
			return false;
		}

		public bool ManagedHide(Action onComplete, out RDSimpleScreenBase lastActiveScreen)
		{
			lastActiveScreen = null;

			if (Hide(onComplete))
			{
				if (PreviousScreen != null && UseHistory)
				{
					ScreenManager.ME.ShowHideScreen(PreviousScreen, true);
					lastActiveScreen = PreviousScreen;
					PreviousScreen = null;
				}
				return true;
			}

			return false;
		}
	}
}
