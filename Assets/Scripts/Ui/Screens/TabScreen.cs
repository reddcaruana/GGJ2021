using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Framework.Ui.Screens;

namespace Assets.Scripts.Ui.Screens
{
	public abstract class TabScreen : RDSimpleScreenBase
	{
		float buttonSizeX;

		protected override void Awake()
		{
			Button button = transform.Find("ButtonTab").GetComponent<Button>();
			button.onClick.AddListener(OnButtonMsg);
			buttonSizeX = button.GetComponent<RectTransform>().rect.width;

			base.Awake();
			DisableOnHide = false;
		}

		protected override void HidePosition()
		{
			Vector2 hiddenPos = rectTransform.anchoredPosition;
			hiddenPos.x += rectTransform.rect.width - buttonSizeX - (buttonSizeX * 0.1f);
			HidePos = hiddenPos;
		}

		protected abstract void OnButtonMsg();
	}
}
