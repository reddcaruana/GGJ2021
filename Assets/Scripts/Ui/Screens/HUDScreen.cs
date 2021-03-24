using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Constants;
using Assets.Scripts.Framework.Ui.Screens;

namespace Assets.Scripts.Ui.Screens
{
	public class HUDScreen : RDSimpleScreenBase
	{
		public override int Index { get; } = (int)ScreenType.HUD;

		protected override void Awake()
		{
			base.Awake();
			Button mainMenuButton = transform.Find("ButtonMainMenu").GetComponent<Button>();
			mainMenuButton.onClick.AddListener(OnMainMenuButtonMsg);

			Button logBookButton = transform.Find("ButtonLogBook").GetComponent<Button>();
			logBookButton.onClick.AddListener(OnLogBookButtonMsg);

		}

		private void OnMainMenuButtonMsg() => ScreenManager.ME_.ShowHideScreen(ScreenType.MainMenu, true);

		private void OnLogBookButtonMsg() => ScreenManager.ME_.ShowHideScreen(ScreenType.LogBook, true);

		protected override void HidePosition()
		{
			Vector2 hiddenPos = rectTransform.anchoredPosition;
			hiddenPos.y += rectTransform.rect.height;
			HidePos = hiddenPos;
		}
	}
}
