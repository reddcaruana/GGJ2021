using System;
using UnityEngine;
using Assets.Scripts.Player;
using Assets.Scripts.Ui.Screens;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Framework.Ui.SafeArea;
using static Assets.Scripts.Framework.Ui.RDUiTransitionAttachment;

namespace Assets.Scripts.Controllers
{
	public class UiController
	{
		public bool IsMainMenu { get; private set; }

		private Canvas canvas;
		private SafeAreaPanel safeArea;

		private MainMenuScreen mainMenuScreen;
		private CaughtFishScreen caughtFishScreen;
		private DebugScreen debugScreen;
		private LogBookScreen logBookScreen;
		private HUDScreen hudScreen;

		public void Init()
		{
			canvas = GameObject.FindObjectOfType<Canvas>();
			new GameObject("SafeArea", typeof(RectTransform)).transform.SetParent(canvas.transform);
			safeArea = canvas.transform.Find("SafeArea").gameObject.AddComponent<SafeAreaPanel>();

			// Initialization determines overlay
			LoadDebugScreen();
			LoadCaughtScreen();
			LoadHudScreen();
			LoadMainMenuScreen();
			LoadLogBookScreen();

			ShowHideMainMenu(true);
		}

		private void LoadMainMenuScreen() => LoadScreen(ref mainMenuScreen, "MainMenuScreen");
		private void LoadCaughtScreen() => LoadScreen(ref caughtFishScreen, "CaughtFishScreen");
		private void LoadDebugScreen() => LoadScreen(ref debugScreen, "DebugScreen");
		private void LoadLogBookScreen() => LoadScreen(ref logBookScreen, "LogBookScreen");
		private void LoadHudScreen() => LoadScreen(ref hudScreen, "HUDScreen");

		private void LoadScreen<T>(ref T screen, string name) where T : ScreenBase
		{
			GameObject objPrefab = AssetLoader.ME.Loader<GameObject>("Prefabs/Ui/Screens/" + name);
			screen = MonoBehaviour.Instantiate(objPrefab, safeArea.transform).AddComponent<T>();
		}

		public void ShowHideMainMenu(bool value, Action onComplete = null)
		{
			ShowHideScreen(mainMenuScreen, value, OnComplete);
			void OnComplete()
			{
				IsMainMenu = value;
				ShowHideScreen(hudScreen, !IsMainMenu, null);
				onComplete?.Invoke();
			}

		}

		public void CaughtFish(FishTypeData data, Action onComplete = null)
		{
			caughtFishScreen.Show(data, OnComplete);

			void OnComplete()
			{
				caughtFishScreen.HideInstantly();
				onComplete?.Invoke();
			}
		}

		public void ToggleLogBook() => ShowHideLogBook(logBookScreen.CurrentState == State.Hidden);

		public void ShowHideLogBook(bool value, Action onComplete = null)
		{
			if (logBookScreen.CurrentState == State.Hidden && value)
				logBookScreen.Log(PlayerData.LogBook.ToArray());

			ShowHideScreen(logBookScreen, value, onComplete);
		}

		private void ShowHideScreen(ScreenBase screen, bool value, Action onComplete)
		{
			if (value)
				screen.Show(onComplete);
			else
				screen.Hide(onComplete);
		}
		private void ToggleScreen(ScreenBase screen, Action onComplete)
		{
			if (screen.CurrentState == State.Hidden)
				screen.Show(onComplete);

			else if (screen.CurrentState == State.Visible)
				screen.Hide(onComplete);
		}
		
		[System.Diagnostics.Conditional("RDEBUG")]
		public void DebugShow(float rodMax, float fishMax)
		{
			debugScreen.UpdateProgressBarMax(rodMax, fishMax);
			debugScreen.Show();
		}
		[System.Diagnostics.Conditional("RDEBUG")]
		public void DebugHide() => debugScreen.Hide();
		[System.Diagnostics.Conditional("RDEBUG")]
		public void UpdateFightValues(float rodValue, float fishValue) => debugScreen.UpdateProgressBarValues(rodValue, fishValue);
		[System.Diagnostics.Conditional("RDEBUG")]
		public void UpdateInfo(string info) => debugScreen.UpdateInfo(info);
	}
}
