using UnityEngine.UI;
using Assets.Scripts.Controllers;
using UnityEngine;

namespace Assets.Scripts.Ui.Screens
{
	public class HUDScreen : ScreenBase
	{
		protected override void Awake()
		{
			base.Awake();
			Button mainMenuButton = transform.Find("ButtonMainMenu").GetComponent<Button>();
			mainMenuButton.onClick.AddListener(OnMainMenuButtonMsg);

			Button logBookButton = transform.Find("ButtonLogBook").GetComponent<Button>();
			logBookButton.onClick.AddListener(OnLogBookButtonMsg);

		}

		private void OnMainMenuButtonMsg() => ViewController.UiController.ShowHideMainMenu(true);

		private void OnLogBookButtonMsg() => ViewController.UiController.ShowHideLogBook(true);

		protected override void HidePosition()
		{
			Vector2 hiddenPos = rectTransform.anchoredPosition;
			hiddenPos.y += rectTransform.rect.height;
			HidePos = hiddenPos;
		}
	}
}
