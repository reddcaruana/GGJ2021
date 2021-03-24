using UnityEngine;
using Assets.Scripts.Constants;
using Assets.Scripts.Ui.Screens;
using Assets.Scripts.Framework.Ui.SafeArea;

namespace Assets.Scripts.Controllers
{
	public class UiController
	{
		public Canvas Canvas { get; private set; }
		private SafeAreaPanel safeArea;

		public void Init()
		{
			Canvas = GameObject.FindObjectOfType<Canvas>();
			new GameObject("SafeArea", typeof(RectTransform)).transform.SetParent(Canvas.transform);
			safeArea = Canvas.transform.Find("SafeArea").gameObject.AddComponent<SafeAreaPanel>();

			ScreenManager.ME.Init(safeArea);
			ScreenManager.ME_.ShowHideScreen(ScreenType.GameInfo, true);
			GameInfo.VersionCheck();
		}
	}
}
