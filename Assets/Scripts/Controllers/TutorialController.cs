using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Rods;
using Assets.Scripts.Player;
using Assets.Scripts.Framework;
using Assets.Scripts.Constants;
using Assets.Scripts.Ui.Screens;
using Assets.Scripts.Ui.Tutorials;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Framework.AssetsManagers;
using static Assets.Scripts.Framework.Ui.RDUiTransitionAttachment;
using Assets.Scripts.Framework.Ui.Screens;

namespace Assets.Scripts.Controllers
{
	public class TutorialController
	{
		private const float SPEED_SCREEN_DURATION = 3f;
		private const float ROD_ENERGY_LIMIT_NORMALISED = 0.3f;

		private bool isCatchAssist;
		private TutorialHand tutorialHand;
		private ScreenManager screenManager;

		private TutorialSimView tutorialSim;
		private TutorialSimView TutorialSim 
		{
			get
			{
				if (tutorialSim == null)
				{
					tutorialSim = MonoBehaviour.Instantiate(AssetLoader.ME.Load<GameObject>("Prefabs/Ui/Tutorial/TutorialSim"), 
						ViewController.UiController.Canvas.transform).AddComponent<TutorialSimView>();
					tutorialSim.Hide();
				}
				return tutorialSim;
			}
		}

		public static bool HasCompletedTutorials() => PlayerData.TutorialLevel == TutorialType.MAX;

		public void Init()
		{
			screenManager = ScreenManager.ME_;
			tutorialHand = new TutorialHand();
			tutorialHand.Init();

			if (PlayerData.TutorialLevel <= TutorialType.Nibble)
				GameController.ME.Rod.NibbleTime = float.MaxValue;

			StartTutoial(PlayerData.TutorialLevel);
		}

		private void StartTutoial(TutorialType type)
		{
			switch (type)
			{
				case TutorialType.PlayButton: PlayButtonTutorial(); return;
				case TutorialType.Cast: CastTutorial(); return;
				case TutorialType.Nibble: NibbleTutorial(); return;
				case TutorialType.CounterFish: CounterFishTutorial(); return;
				case TutorialType.RodEnergy: RodEnergyTutorial(); return;
				case TutorialType.LineSlack: LineSlackTutorial(); return;
				case TutorialType.TieringFish: CatchAssistTurorial(); return;
				case TutorialType.PayWall: PayWallTutorial(); return;
				case TutorialType.Scroll: ScrollTutorial(); return;
				default: RDebugUtils.LogError($"Unable to find Tutorial: {type}"); return;
			}
		}

		private void PlayButtonTutorial()
		{
			int mainMenuTypeIndex = (int)ScreenType.MainMenu;

			CoroutineRunner.WaitUntil(() => screenManager.ScreenState(mainMenuTypeIndex) == State.Visible, () =>
			{
				tutorialHand.WorldPosition(screenManager.ActiveScreen.transform.Find("ButtonPlay").transform.position);
				tutorialHand.ClickRepeat();

				CoroutineRunner.WaitUntil(() => screenManager.ScreenState(mainMenuTypeIndex) != State.Visible, () =>
				{
					tutorialHand.Stop();
					CoroutineRunner.WaitUntil(() => screenManager.ScreenState(mainMenuTypeIndex) != State.Hidden, () =>
					{
						PlayerData.CompletedTutorial();
						StartTutoial(PlayerData.TutorialLevel);
					});
				});
			});
		}

		private void CastTutorial()
		{
			GameController.ME.SetActiveInteractions(false);
			CoroutineRunner.WaitUntil(() => !screenManager.IsAScreenActive, () =>
			{
				GameController.ME.SetActiveInteractions(true);
				FishController fishController = ViewController.CurrentSeason.GetTutorialFish();
				fishController.Set(FishWiki.TutorialFish);
				tutorialHand.WorldPosition(fishController.GetTutorialCastPosition());
				tutorialHand.ClickRepeat();

				CoroutineRunner.WaitUntil(() => GameController.ME.Rod.IsCasted, () =>
				{
					tutorialHand.Stop();
					PlayerData.CompletedTutorial();
					StartTutoial(PlayerData.TutorialLevel);
				});
			});
		}

		private void NibbleTutorial()
		{
			CoroutineRunner.WaitUntil(() => GameController.ME.Rod.IsBusy, () => 
			{
				GameController.ME.SetActiveInteractions(false);
				CoroutineRunner.WaitUntil(() => GameController.ME.Rod.FishCanBeCaught, () => 
				{
					GameController.ME.SetActiveInteractions(true);
					tutorialHand.WorldPosition(Statics.VECTOR3_ZERO);
					tutorialHand.ClickRepeat();

					CoroutineRunner.WaitUntil(() => !GameController.ME.Rod.FishCanBeCaught, () => 
					{
						tutorialHand.Stop();
						GameController.ME.Rod.NibbleTime = RodBase.NIBBLE_TIME;
						PlayerData.CompletedTutorial();
						StartTutoial(PlayerData.TutorialLevel);
					});
				});
			});
		}

		private void CounterFishTutorial()
		{
			CoroutineRunner.WaitUntil(() => GameController.ME.Rod.FishHooked, OnFishHooked);
			void OnFishHooked()
			{
				IndicateRodFishCounter();

				RodBase rod = GameController.ME.Rod;
				float limit = rod.Energy.Max * ROD_ENERGY_LIMIT_NORMALISED;
				CoroutineRunner.WaitUntil(() => rod.Energy.Value > limit, () => 
				{
					CatchAssist(true);
					PlayerData.CompletedTutorial();
					StartTutoial(PlayerData.TutorialLevel);
				});
			}
		}

		private void RodEnergyTutorial()
		{
			RodBase rod = GameController.ME.Rod;
			float limit = rod.Energy.Max * ROD_ENERGY_LIMIT_NORMALISED;

			CatchAssist(true);
			CoroutineRunner.WaitUntil(() => rod.Energy.Value < limit, () => 
			{
				ShowSpeedScreenHelper("Sprites/Tutorial/BgTileDumbel", "Sprites/Tutorial/StrengthFish",
					new TutorialSimFrame[] { new LineSnapSimFrame(), new RodRestingSimFrame(1), new RodRestingSimFrame(-1) },
					() => 
				{
					CoroutineRunner.WaitUntil(() => FightingModule.RodIsRestoring(rod, FightingModule.FishPullState), () =>
					{
						GameController.ME.IsPaused = false;
						PlayerData.CompletedTutorial();
						StartTutoial(PlayerData.TutorialLevel);
					});
				});
			});
		}

		private void LineSlackTutorial()
		{
			RodBase rod = GameController.ME.Rod;

			CatchAssist(true);
			CoroutineRunner.WaitUntil(() => rod.IsLineSlack, () =>
			{
				ShowSpeedScreenHelper("Sprites/Tutorial/BgTileAngry", "Sprites/Tutorial/SpittingFish", 
					new TutorialSimFrame[] { new LineSlackEscapeSimFrame(), new RodRestedEnoughSimFrame(1), new RodRestedEnoughSimFrame(-1) },
					() => 
				{
					CoroutineRunner.WaitUntil(() => FightingModule.RodIsCountering(rod, FightingModule.FishPullState), () =>
					{
						GameController.ME.IsPaused = false;
						PlayerData.CompletedTutorial();
						StartTutoial(PlayerData.TutorialLevel);
					});
				});
			});
		}

		private void CatchAssistTurorial()
		{
			RodBase rod = GameController.ME.Rod;

			CoroutineRunner.WaitUntil(() => rod.FishHooked, () => 
			{
				CatchAssist(true);
				CoroutineRunner.WaitUntil(() => !rod.FishHooked || rod.PotentialCatch.Energy.IsResting, () =>
			   {
				   if (!rod.FishHooked)
					   return;

				   ShowSpeedScreenHelper("Sprites/Tutorial/BgTileDrop", "Sprites/Tutorial/TiredFish",
					   new TutorialSimFrame[] { new ReelInSimFrame() },
					   () => 
				   {
					   GameController.ME.IsPaused = false;
					   PlayerData.CompletedTutorial();
					   StartTutoial(PlayerData.TutorialLevel);
				   });
			   });
			});
		}

		private void PayWallTutorial()
		{
			int caughtFishIndex = (int)ScreenType.CaughtFish;
			CoroutineRunner.WaitUntil(() => ScreenManager.ME.IsAScreenActive &&
			ScreenManager.ME.ActiveScreen.Index == caughtFishIndex, () => 
			{
				RDSimpleScreenBase caughtFishScreen = ScreenManager.ME.ActiveScreen;
				CoroutineRunner.WaitUntil(() => caughtFishScreen.CurrentState == State.Hidden, () =>
				{
					SeasonCinematicController.TrySeasonIntro(() => 
					{
						PlayerData.CompletedTutorial();
						StartTutoial(PlayerData.TutorialLevel);
					});
				});
			});
		}

		private void ScrollTutorial()
		{

			CoroutineRunner.WaitUntil(() => !ScreenManager.ME.IsAScreenActive, () => 
			{
				Vector2 startPos = new Vector2(0, ViewController.Area.HalfHeight * 0.8f);
				Vector2 endPos = Statics.VECTOR2_ZERO;

				tutorialHand.DragRepeat(startPos, endPos);
				CoroutineRunner.WaitUntil(() => ViewController.IsFollowing, () => 
				{
					tutorialHand.Stop();
					PlayerData.CompletedTutorial();
				});
			});
		}

		public void CatchAssist(bool value)
		{
			if (value && !isCatchAssist)
				CoroutineRunner.RunCoroutine(CatchAssistCoroutine());
			else if (!value)
				isCatchAssist = false;
		}

		private IEnumerator CatchAssistCoroutine()
		{
			RDebugUtils.Log("<color=red>[TutorialManager] Starting Catch Assist</color>");
			isCatchAssist = true;
			RodBase rod = GameController.ME.Rod;
			float limit = rod.Energy.Max * ROD_ENERGY_LIMIT_NORMALISED;
			byte indication = 0;

			while (isCatchAssist && rod.FishHooked)
			{
				// Stop animation if not needed
				if ((indication == 1 && !rod.PotentialCatch.Energy.IsResting) || 
					(indication == 2 && FightingModule.RodIsCountering(rod, FightingModule.FishPullState)) ||
					(indication == 3 && FightingModule.RodIsRestoring(rod, FightingModule.FishPullState)))
				{
					TryStopCurrentIndication();
				}

				// Reel In
				if (indication != 1 && rod.PotentialCatch.Energy.IsResting)
				{
					RDebugUtils.Log("<color=aqua>[TutorialManager] Catch Assist Reel in indication</color>");
					TryStopCurrentIndication();
					indication = 1;
					HandRodFishCounterPosition();
					tutorialHand.ClickFastRepeat();
				}
				// Counter Fish
				else if (indication != 2 && rod.IsLineSlack)
				{
					RDebugUtils.Log("<color=green>[TutorialManager] Catch Assist Counter</color>");
					TryStopCurrentIndication();
					indication = 2;
					IndicateRodFishCounter();
				}
				// Restore Rod
				else if (indication != 3 && rod.Energy.Value <= limit)
				{
					RDebugUtils.Log("<color=yellow>[TutorialManager] Catch Assist Restore</color>");
					TryStopCurrentIndication();
					indication = 3;
					IndicateRodRestore();
				}

				yield return null;
			}

			TryStopCurrentIndication();
			isCatchAssist = false;
			RDebugUtils.Log("[TutorialManager] Stopping Catch Assist");

			void TryStopCurrentIndication()
			{
				if (indication == 0)
					return;
				tutorialHand.Stop();
				indication = 0;
			}
		}

		private void IndicateRodFishCounter()
		{
			tutorialHand.ClickRepeat();
			HandRodFishCounterPosition();
		}

		private void HandRodFishCounterPosition()
		{
			if (FightingModule.FishPullState == PullState.Left)
				tutorialHand.PositionOnPullRight();

			else if (FightingModule.FishPullState == PullState.Right)
				tutorialHand.PositionOnPullLeft();

			else if (FightingModule.FishPullState == PullState.None)
				RDebugUtils.LogError("[TutorialManager] CounterFish => Unsupported Pull State: " + FightingModule.FishPullState);
		}

		private void IndicateRodRestore()
		{
			tutorialHand.ClickRepeat();

			if (FightingModule.FishPullState == PullState.Right)
				tutorialHand.PositionOnPullRight();

			else if (FightingModule.FishPullState == PullState.Left)
				tutorialHand.PositionOnPullLeft();

			else if (FightingModule.FishPullState == PullState.None)
				RDebugUtils.LogError("[TutorialManager] CounterFish => Unsupported Pull State: " + FightingModule.FishPullState);
		}

		private void ShowSpeedScreenHelper(string tilePath, string mainPath, TutorialSimFrame[] simFrames, Action onComplete, bool reactivateInteractions = true)
		{
			CatchAssist(false);
			GameController.ME.IsPaused = true;
			GameController.ME.SetActiveInteractions(false);
			SpeedScreen.Screen.SpeedImageHandler.SetTileSprite(AssetLoader.ME.Load<Sprite>(tilePath));
			SpeedScreen.Screen.SpeedImageHandler.SetMainSprite(AssetLoader.ME.Load<Sprite>(mainPath));
			SpeedScreen.Show(SPEED_SCREEN_DURATION, onComplete: () =>
			{
				TutorialSim.LoadSims(simFrames);
				TutorialSim.Show(OnComplete);
				void OnComplete()
				{
					if (reactivateInteractions)
					{
						GameController.ME.SetActiveInteractions(true);
						CatchAssist(true);
					}
					onComplete?.Invoke();
				}
			});
		}
	}	
}
