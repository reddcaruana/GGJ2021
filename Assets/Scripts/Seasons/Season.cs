using UnityEngine;
using Assets.Scripts.Constants;
using Assets.Scripts.Views.Seasons;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Framework;

namespace Assets.Scripts.Seasons
{
	public abstract class Season
	{
		public abstract SeasonAreaType Type { get; }
		public abstract int NumberOfSegments { get; }
		public abstract FishData[] SupportedFish { get; }
		public abstract int MaxFish { get; }

		protected static FishController[] fishControllers;
		private readonly Vector2 FishTankSize;


		public bool HasView => view != null;
		protected SeasonView view;

		public Season()
		{
			float height = Camera.main.orthographicSize * 2.0f;
			float width = height * Screen.width / Screen.height;
			FishTankSize = new Vector2(width, height * NumberOfSegments);

			fishControllers = new FishController[MaxFish];
			for (int i = 0; i < fishControllers.Length; i++)
				fishControllers[i] = new FishController();
		}

		public void CreateView(Transform parent)
		{
			view = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<SeasonView>("Prefabs/Seasons/SeasonView"), parent);
			view.Set(FishTankSize);
		}

		public void CreateFishViews()
		{
			for (int i = 0; i < fishControllers.Length; i++)
				fishControllers[i].CreateView(view.FishTank);
		}

		public void SetAllFish()
		{
			for (int i = 0; i < fishControllers.Length; i++)
				SetFish(fishControllers[i]);
		}

		public void SetFish(FishController fishController)
		{
			if (!fishController.IsReadyToSet)
				return;

			fishController.Set(FishWiki.GetOneRandom(SupportedFish));
		}

		public void DistriubuteFish()
		{
			float verticalStep = FishTankSize.y / MaxFish;
			Vector3 position = new Vector3()
			{
				x = 0,
				y = (FishTankSize.y / 2f) - (verticalStep / 2f)
			};

			for (int i = 0; i < fishControllers.Length; i++)
			{
				fishControllers[i].SetSpawnPoint(position);
				fishControllers[i].Appear();
				position.y -= verticalStep;
			}
		}

		public void OnFishCoolDown(FishController fishController)
		{
			SetFish(fishController);
		}

		public bool ValidatePosition(Vector2 screenPosition, out Vector3 worldPos)
		{
			if (!HasView)
			{
				Debug.LogError("[Season] View must be created first");
				worldPos = Statics.VECTOR2_ZERO;
				return false;
			}

			worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
			Vector3 topLftCorner = view.transform.position;
			topLftCorner.x -= FishTankSize.x / 2f;
			topLftCorner.y += FishTankSize.y / 2;

			Vector3 bottomRightCorner = topLftCorner;
			bottomRightCorner.x += FishTankSize.x;
			bottomRightCorner.y -= FishTankSize.y;

			return MathUtils.IsInRectArea(topLftCorner, bottomRightCorner, worldPos);
		}
	}
}
