using System;
using UnityEngine;
using Assets.Scripts.Framework.Ui.SafeArea;
using Assets.Scripts.Framework.AssetsManagers;
using static Assets.Scripts.Framework.Ui.RDUiTransitionAttachment;

namespace Assets.Scripts.Framework.Ui.Screens
{
	public abstract class RDSimpleScreenManager
	{
		public static RDSimpleScreenManager ME;

		public bool IsAScreenActive => lastActiveScreen != null;
		public RDSimpleScreenBase ActiveScreen => lastActiveScreen;

		protected readonly RDSimpleScreenBase[] Screens;
		private RDSimpleScreenBase lastActiveScreen;

		protected RDSimpleScreenManager(int maxScreens)
		{
			Screens = new RDSimpleScreenBase[maxScreens];
			ME = this;
		}

		/// <summary>
		/// Initialization order determines overlay
		/// </summary>
		/// <param name="safeArea"></param>
		public abstract void Init(SafeAreaPanel safeArea);

		protected void LoadScreen<T>(SafeAreaPanel safeArea, string name) where T : RDSimpleScreenBase
		{
			GameObject objPrefab = AssetLoader.ME.Load<GameObject>("Prefabs/Ui/Screens/" + name);
			T screen = MonoBehaviour.Instantiate(objPrefab, safeArea.transform).AddComponent<T>();
			Screens[screen.Index] = screen;
		}

		public void SwitchScreens(int screenindex, Action onComplete = null)
		{
			lastActiveScreen.Hide();
			ShowHideScreen(screenindex, true, onComplete);
		}

		public void ToggleScreen(int screenIndex, Action onComplete = null) =>
			ToggleScreen(Screens[screenIndex], onComplete);

		public void ToggleScreen(RDSimpleScreenBase screen, Action onComplete = null)
		{
			if (screen.CurrentState == State.Hidden)
				ShowHideScreen(screen, true, onComplete);

			else if (screen.CurrentState == State.Visible)
				ShowHideScreen(screen, false, onComplete);
		}

		public void ShowHideScreen(int screenIndex, bool value, Action onComplete = null) =>
			ShowHideScreen(Screens[screenIndex], value, onComplete);

		public void ShowHideScreen(RDSimpleScreenBase screen, bool value, Action onComplete = null)
		{
			if (value)
			{
				if (screen.ManagedShow(lastActiveScreen, onComplete))
					lastActiveScreen = screen;
			}
			else
			{
				bool canHide = false;
				RDSimpleScreenBase returnedScreen = null;
				canHide = screen.ManagedHide(OnHideComplete, out returnedScreen);

				void OnHideComplete()
				{
					if (canHide)
					{
						lastActiveScreen = returnedScreen;
						onComplete?.Invoke();
					}
				}
			}
		}

		public State ScreenState(int screenIndex) => Screens[screenIndex].CurrentState;
	}
}
