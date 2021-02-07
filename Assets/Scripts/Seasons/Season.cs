using UnityEngine;
using Assets.Scripts.Constants;
using Assets.Scripts.Framework;
using System.Collections.Generic;
using Assets.Scripts.Controllers;
using Assets.Scripts.Views.Seasons;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.AquaticCreatures;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Paywalls;

namespace Assets.Scripts.Seasons
{
	public abstract class Season
	{
		private const float MARGIN_NORMALISED = 0.2f;

		public abstract SeasonAreaType Type { get; }
		public abstract string NiceName { get; }
		public abstract int NumberOfSegments { get; }
		public abstract FishData[] SupportedFish { get; }
		public abstract int MaxFish { get; }
		public abstract PaywallBase PayWall { get; }

		private Vector3 baseWorldPosition;
		public readonly Area2D FishTankArea;
		protected FishController[] fishControllers;


		public bool HasView => view != null;
		protected SeasonView view;

		public Season(Vector3 baseWorldPosition)
		{
			this.baseWorldPosition = baseWorldPosition;

			float x = ViewController.Area.Width - (ViewController.Area.Width * MARGIN_NORMALISED);
			float y = ViewController.Area.Height * NumberOfSegments;
			Vector2 center = baseWorldPosition;
			center.y += y / 2f;
			FishTankArea = new Area2D(x, y, center);

			fishControllers = new FishController[MaxFish];
			for (int i = 0; i < fishControllers.Length; i++)
			{
				fishControllers[i] = new FishController();
				fishControllers[i].RegisterToCoolDownComplete(OnFishCoolDown);
			}

			PayWall.Set(NiceName);
		}

		public void CreateView(Transform parent)
		{
			view = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<SeasonView>("Prefabs/Seasons/SeasonView"), parent);
			view.Set(FishTankArea.Size, NiceName, baseWorldPosition);
			PayWall.CreateView(view.transform);

		}

		public Vector3 GetTopWorldPosition()
		{
			Vector3 result = view.transform.position;
			result.y += (FishTankArea.Height / 2f);
			return result;
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
			float verticalStep = FishTankArea.Height / MaxFish;
			Vector3 localPosition = new Vector3()
			{
				x = 0,
				y = (FishTankArea.Height / 2f) - (verticalStep / 2f)
			};

			for (int i = 0; i < fishControllers.Length; i++)
			{
				fishControllers[i].SetSpawnPoint(localPosition);
				fishControllers[i].Appear();
				localPosition.y -= verticalStep;
			}
		}

		public void OnFishCoolDown(FishController fishController)
		{
			SetFish(fishController);
			fishController.Appear();
		}

		public bool ValidatePosition(Vector2 screenPosition, out Vector3 worldPos)
		{
			if (!HasView)
			{
				Debug.LogError("[Season] View must be created first");
				worldPos = Statics.VECTOR2_ZERO;
				return false;
			}

			worldPos = ViewController.MainCamera.ScreenToWorldPoint(screenPosition);
			return MathUtils.IsInRectArea(FishTankArea, worldPos);
		}

		public IAquaticCreature OnCast(Vector3 worldPosition)
		{
			Vector3 localPosition;
			if (HasView)
				localPosition = view.transform.InverseTransformPoint(worldPosition);
			else
				localPosition = worldPosition;

			List<IAquaticCreature> potentialCatch = new List<IAquaticCreature>();

			for (int i = 0; i < fishControllers.Length; i++)
			{
				if (!fishControllers[i].CanCatch)
					continue;

				IAquaticCreature creature = fishControllers[i].OnCast(localPosition);
				if (creature != null)
					potentialCatch.Add(creature);
			}

			IAquaticCreature approachFloat = null;

			for (int i = 0; i < potentialCatch.Count; i++)
			{
				if (approachFloat == null)
				{
					approachFloat = potentialCatch[i];
					continue;
				}

				if (approachFloat.Weight < potentialCatch[i].Weight)
				{
					approachFloat.Escape();
					approachFloat = potentialCatch[i];
				}
			}

			if (approachFloat != null)
				approachFloat.ApproachFloat(localPosition);

			return approachFloat;
		}
	}
}
