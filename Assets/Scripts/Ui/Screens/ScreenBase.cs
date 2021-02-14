using UnityEngine;
using Assets.Scripts.Framework.Ui;

namespace Assets.Scripts.Ui.Screens
{
	public class ScreenBase : RDUiTransitionElement
	{
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
	}
}
