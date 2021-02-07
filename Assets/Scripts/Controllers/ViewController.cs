using UnityEngine;
using Assets.Scripts.Framework;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Seasons;
using Assets.Scripts.Constants;

namespace Assets.Scripts.Controllers
{
	public static class ViewController
	{
		public static Camera MainCamera = Camera.main;
		public static readonly Area2D Area;

		public static readonly UiController UiController = new UiController();

		public static Transform MainParent;
		public static Season CurrentSeason { get; private set; }
		private static Season[] seasons = new Season[(int)SeasonAreaType.MAX];

		static ViewController()
		{
			float height = MainCamera.orthographicSize * 2.0f;
			float width = height * Screen.width / Screen.height;
			Area = new Area2D(width, height, Statics.VECTOR2_ZERO);
		}

		public static void Init()
		{
			MainParent = new GameObject("MainParent").transform;
			MainParent.ResetTransforms();

			// Temporary Create Summer
			CurrentSeason = seasons[0] = SeasonFactory.CreateSeason(MainParent, SeasonAreaType.One, GetNewSeasonWorldPosition(0));

			UiController.Init();
		}

		public static void CreateSeasons()
		{
			for (int i = 0; i < seasons.Length; i++)
			{
				seasons[0] = SeasonFactory.CreateSeason(MainParent, (SeasonAreaType)i, GetNewSeasonWorldPosition(i));
			}
		}

		private static Vector3 GetNewSeasonWorldPosition(int index)
		{
			Vector3 basePosition;

			if (index == 0)
			{
				basePosition = Area.Center;
				basePosition.y = Area.BottomRightCorner.y;
			}
			else
				basePosition = seasons[index - 1].GetTopWorldPosition();


			return basePosition;
		}
	}
}
