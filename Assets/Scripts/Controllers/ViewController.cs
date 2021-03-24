using UnityEngine;
using Assets.Scripts.Player;
using Assets.Scripts.Seasons;
using Assets.Scripts.Constants;
using Assets.Scripts.Factories;
using Assets.Scripts.Framework;
using Assets.Scripts.Framework.Input;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.AquaticCreatures;
using System;
using Assets.Scripts.Ui.Screens;

namespace Assets.Scripts.Controllers
{
	public static class ViewController
	{
		public static Camera MainCamera = Camera.main;

		public static readonly Area2D Area;
		public static readonly Vector2 ScreenTop;
		public static readonly Vector2 ScreenBottom;

		public static readonly UiController UiController = new UiController();

		public static Transform MainParent;

		public static bool IsFollowing => SeasonScrollController.IsFollowing;
		public static Season CurrentSeason => seasons[seasonIdex];
		private static int seasonIdex = (int)PlayerData.SeasonData.LastSeasonVisited.Value;
		private static Season[] seasons = new Season[(int)SeasonAreaType.MAX];
		private static readonly SeasonScrollController SeasonScrollController = new SeasonScrollController();

		static ViewController()
		{
			float height = MainCamera.orthographicSize * 2.0f;
			float width = height * Screen.width / Screen.height;
			Area = new Area2D(width, height, Statics.VECTOR2_ZERO);
			ScreenTop = new Vector2(0, Area.HalfHeight); ;
			ScreenBottom = new Vector2(0, -Area.HalfHeight);
		}

		public static IAquaticCreature OnCast(Vector3 worldPos)
		{
			return CurrentSeason.OnCast(worldPos);
		}

		public static void Init()
		{

			MainParent = new GameObject("MainParent").transform;
			MainParent.ResetTransforms();

			CreateSeasons();

			FactoryManager.Fish.Init(30);
			FactoryManager.BottomDebri.Init(400);
			FactoryManager.Stream.Init(8);
			FactoryManager.Terrain.Init(500);

			SeasonScrollController.Init();
			UiController.Init();

			InputManager.Init(MainCamera);
			InputManager.Enable(true);

			FactoryManager.Stream.StartStream(8, null);
		}

		public static void CreateSeasons()
		{
			for (int i = 0; i < seasons.Length; i++)
				seasons[i] = SeasonFactory.CreateSeason((SeasonAreaType)i);
		}

		public static Season PeekNextSeason() => PeekSeason(1);
		public static Season PeekPrevSeason() => PeekSeason(-1);
		private static Season PeekSeason(int direction) => seasons[MathUtils.LoopIndex(seasonIdex + direction, seasons.Length)];

		public static void NextSeason() => MoveSeason(1);
		public static void PrevSeason() => MoveSeason(-1);
		private static void MoveSeason(int direction)
		{
			seasonIdex = MathUtils.LoopIndex(seasonIdex + direction, seasons.Length);
			PlayerData.SeasonData.LastSeasonVisited.Value = seasons[seasonIdex].Type;
			SeasonCinematicController.TrySeasonIntro();
		}

		public static bool CanCast(Vector3 worldPosition)
		{
			// Check if a Screen is active or boat is moving
			if (ScreenManager.ME.IsAScreenActive || SeasonScrollController.IsMoving || SeasonCinematicController.IsCinematic)
				return false;

			if (!CurrentSeason.ValidatePosition(worldPosition))
			{
				if (!PeekNextSeason().ValidatePosition(worldPosition))
				{
					if (PeekPrevSeason().ValidatePosition(worldPosition) &&
						PlayerData.SeasonData.LastSeasonUnlocked.Value >= PeekPrevSeason().Type)
					{
						PrevSeason();
						return true;
					}
				}
				else if (PlayerData.SeasonData.LastSeasonUnlocked.Value >= PeekNextSeason().Type)
				{
					NextSeason();
					return true;
				}
			}
			else
				return true;

			RDebugUtils.Log("Cant Cast There ........./Z X");
			return false;
		}

		public static bool TryResetRodAndMove()
		{
			bool result = CanMove();
			if (result)
				GameController.ME.Rod.ReelIn();
			return result;
		}

		public static bool CanMove()
		{
			if (ScreenManager.ME.IsAScreenActive || GameController.ME.Rod.IsBusy || SeasonCinematicController.IsCinematic)
				return false;

			return true;
		}

		public static void FollowStart() => SeasonScrollController.FollowStart();
		public static void FollowUpdate(float followDistrance) => SeasonScrollController.FollowUpdate(followDistrance);
		public static void FollowStop() => SeasonScrollController.FollowStop();
		public static void Accelerate(float speed) => SeasonScrollController.SetSpeed(speed);

		public static void MakeSeasonIntro(Action onComplete = null) => MoveToPayWallAndBack(onComplete);

		public static void MakeSeasonOutro(Action onComplete = null) => MoveToPayWallAndBack(onComplete);

		public static void MoveToPayWall(Action<float> onComplete)
		{
			CoroutineRunner.Wait(0.5f, () =>
			{
				// Hide Boat
				GameController.ME.Boat.View.Hide(0.5f);
				// Scroll Seasons Till Paywall
				SeasonScrollController.AutoScrollToPayWall(onComplete: onComplete);
			});
		}

		public static void MoveBackFromPaywall(float initYPos, Action onComplete)
		{
			// Scroll Back to initial Position
			SeasonScrollController.AutoScrollToPosition(initYPos, onComplete: onComplete);
			CoroutineRunner.Wait(1.5f, onComplete: () =>
			{
				GameController.ME.Boat.View.Show(0.5f);
			});
		}

		private static void MoveToPayWallAndBack(Action onComplete)
		{
			MoveToPayWall(onComplete: (initYPos) =>
			{
				// Wait For Seconds
				CoroutineRunner.Wait(2f, onComplete: () =>
				{
					MoveBackFromPaywall(initYPos, onComplete);
				});
			});
		}
	}
}
