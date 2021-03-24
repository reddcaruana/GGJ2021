using TMPro;
using UnityEngine.UI;
using Assets.Scripts.Constants;
using Assets.Scripts.Framework.Ui.Screens;

namespace Assets.Scripts.Ui.Screens
{
	public class ModalScreen : RDSimpleScreenBase
	{
		private static ModalScreen modalScreen;

		public override int Index { get; } = (int)ScreenType.Modal;
		protected override bool UseHistory { get; } = true;

		private TextMeshProUGUI dialogTitleText;
		private TextMeshProUGUI dialogText;

		protected override void Awake()
		{
			if (modalScreen != null)
				Destroy(gameObject);
			else
				modalScreen = this;

			base.Awake();
			dialogTitleText = transform.Find("ImageBg/TextDialogTitle").GetComponent<TextMeshProUGUI>();
			dialogText = transform.Find("ImageBg/TextDialogText").GetComponent<TextMeshProUGUI>();
			transform.Find("ImageBg/ButtonClose").GetComponent<Button>().onClick.AddListener(OnCloseButtonMsg);
		}

		public void SetTitle(string title) => dialogTitleText.text = title;
		public void SetText(string text) => dialogText.text = text;

		private void OnCloseButtonMsg() => ScreenManager.ME.ShowHideScreen(this, false);

		public static void ShowModalDialog(string title, string text)
		{
			if (modalScreen == null)
				return;

			modalScreen.SetTitle(title);
			modalScreen.SetText(text);
			ScreenManager.ME.ShowHideScreen(modalScreen, true);
		}
	}
}
