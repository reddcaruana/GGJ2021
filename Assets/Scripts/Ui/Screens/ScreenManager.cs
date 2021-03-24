using System;
using Assets.Scripts.Constants;
using Assets.Scripts.Framework.Ui.Screens;
using Assets.Scripts.Framework.Ui.SafeArea;
using Assets.Scripts.AquaticCreatures.Fish;
using static Assets.Scripts.Framework.Ui.RDUiTransitionAttachment;

namespace Assets.Scripts.Ui.Screens
{
	public class ScreenManager : RDSimpleScreenManager
	{
		public static ScreenManager ME_ { get; private set; }

		private CaughtFishScreen caughtFishScreen;

		public ScreenManager() : base((int)ScreenType.MAX)
		{
			ME_ = (ScreenManager)ME;
		}

		public override void Init(SafeAreaPanel safeArea)
		{
			// Initialization determines overlay
			LoadScreen<CaughtFishScreen>(safeArea, "CaughtFishScreen");
			LoadScreen<PauseScreen>(safeArea, "PauseScreen");
			LoadScreen<LogBookScreen>(safeArea, "LogBookScreen");
			LoadScreen<LogBookDetailsScreen>(safeArea, "LogBookDetailsScreen");
			LoadScreen<SpeedScreen>(safeArea, "SpeedScreen");
			LoadScreen<MainMenuScreen>(safeArea, "MainMenuScreen");
			LoadScreen<GameInfoScreen>(safeArea, "GameInfoScreen");
			LoadScreen<ModalScreen>(safeArea, "ModalScreen");
			//LoadScreen<HUDScreen>(safeArea, "HUDScreen");
		}

		public void SwitchScreens(ScreenType screenType, Action onComplete = null) =>
			SwitchScreens((int)screenType, onComplete);

		public void ToggleScreen(ScreenType screenType, Action onComplete = null) =>
			ToggleScreen(Screens[(int)screenType], onComplete);

		public void ShowHideScreen(ScreenType screenType, bool value, Action onComplete = null) =>
			ShowHideScreen(Screens[(int)screenType], value, onComplete);

		public State ScreenState(ScreenType screenType) => ScreenState((int)screenType);

		public void CaughtFish(FishLogData fishData, Action onComplete = null)
		{
			if (caughtFishScreen == null)
				caughtFishScreen = (CaughtFishScreen)Screens[(int)ScreenType.CaughtFish];

			caughtFishScreen.Set(fishData);
			ShowHideScreen(caughtFishScreen, true, OnComplete);
			void OnComplete() => ShowHideScreen(caughtFishScreen, false, onComplete);
		}
	}
}
