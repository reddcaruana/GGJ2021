using UnityEngine;
using Assets.Scripts.Utils;
using Assets.Scripts.Player;
using Assets.Scripts.Seasons;
using Assets.Scripts.Constants;
using Assets.Scripts.Framework;
using Assets.Scripts.Framework.Input;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.AquaticCreatures;
using Assets.Scripts.AquaticCreatures.Fish;

namespace Assets.Scripts.Controllers
{
	public static class ViewController
	{
		public static Camera MainCamera = Camera.main;
		public static readonly Area2D Area;

		public static readonly UiController UiController = new UiController();

		public static Transform MainParent;

		public static Season CurrentSeason => seasons[seasonIdex];
		private static int seasonIdex = 0;
		private static Season[] seasons = new Season[(int)SeasonAreaType.MAX];
		private static SeasonScrollController seasonScrollController = new SeasonScrollController();

		static ViewController()
		{
			float height = MainCamera.orthographicSize * 2.0f;
			float width = height * Screen.width / Screen.height;
			Area = new Area2D(width, height, Statics.VECTOR2_ZERO);
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

			FishFactory.Init(30);
			seasonScrollController.Init();
			UiController.Init();

			InputManager.Init(MainCamera);
			InputManager.Enable(true);

		}

		public static void CreateSeasons()
		{
			for (int i = 0; i < seasons.Length; i++)
				seasons[i] = SeasonFactory.CreateSeason((SeasonAreaType)i);
		}

		public static Season PeekNextSeason() => PeekSeason(1);
		public static Season PeekPrevSeason() => PeekSeason(-1);
		private static Season PeekSeason(int direction) => seasons[MathUtils.LoopIndex(seasonIdex + direction, seasons.Length)];

		private static void NextSeason() => MoveSeason(1);
		private static void PrevSeason() => MoveSeason(-1);
		private static void MoveSeason(int direction) => seasonIdex = MathUtils.LoopIndex(seasonIdex + direction, seasons.Length);

		public static bool CanCast(Vector3 worldPosition)
		{
			// Check if boat is moving
			if (UiController.IsMainMenu || seasonScrollController.IsMoving)
				return false;

			if (!CurrentSeason.ValidatePosition(worldPosition))
			{
				if (!PeekNextSeason().ValidatePosition(worldPosition))
				{
					if (PeekPrevSeason().ValidatePosition(worldPosition) &&
						PlayerData.LastSeasonUnlocked >= PeekPrevSeason().Type)
					{
						PrevSeason();
						return true;
					}
				}
				else if (PlayerData.LastSeasonUnlocked >= PeekNextSeason().Type)
				{
					NextSeason();
					return true;
				}
			}
			else
				return true;

			DebugUtils.Log("Cant Cast There ........./Z X");
			return false;
		}

		public static bool CanMove()
		{
			return !UiController.IsMainMenu && !GameController.ME.Rod.IsCasted;
		}

		public static void MoveFoward() => seasonScrollController.Foward();
		public static void MoveBack() => seasonScrollController.Back();
		public static void ApplBrakes(bool value) => seasonScrollController.ApplyBrakes(value);
	}
}
