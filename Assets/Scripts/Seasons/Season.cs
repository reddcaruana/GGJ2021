using UnityEngine;
using Assets.Scripts.Utils;
using Assets.Scripts.Constants;
using System.Collections.Generic;
using Assets.Scripts.Controllers;
using Assets.Scripts.Views.Seasons;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Seasons.Paywalls;
using Assets.Scripts.AquaticCreatures;
using Assets.Scripts.AquaticCreatures.Fish;

namespace Assets.Scripts.Seasons
{
	public abstract class Season
	{
		private const float MARGIN_NORMALISED = 0.1f;

		public abstract SeasonAreaType Type { get; }
		public abstract string NiceName { get; }
		public abstract int NumberOfSegments { get; }
		public abstract FishData[] SupportedFish { get; }
		public abstract int MaxFish { get; }
		public abstract PaywallBase PayWall { get; }

		public readonly Area2D FishTankArea;
		protected FishController[] fishControllers;


		public bool HasView => view != null;
		protected SeasonView view;

		public Season()
		{

			float x = ViewController.Area.Width - (ViewController.Area.Width * MARGIN_NORMALISED);
			float y = ViewController.Area.Height * NumberOfSegments;
			Vector2 center = new Vector2();
			center.y = PayWall == null ? 0 : -(ViewController.Area.HalfHeight);
			FishTankArea = new Area2D(x, y, center);

			fishControllers = new FishController[MaxFish];
			for (int i = 0; i < fishControllers.Length; i++)
			{
				fishControllers[i] = new FishController();
				fishControllers[i].RegisterToCoolDownComplete(OnFishCoolDown);
			}

			PayWall?.Set(NiceName);
		}

		public void AssignView(SeasonView view, Vector3 baseWorldPosition)
		{
			this.view = view;
			if (PayWall != null) 
			{
				this.view.PaywallView.gameObject.SetActive(true);
				PayWall.AssignView(this.view.PaywallView);
			}
			else
				this.view.PaywallView.gameObject.SetActive(false);

			SetView(baseWorldPosition);
		}

		public Vector2 GetVisualSize()
		{
			Vector2 visualSize = FishTankArea.Size;

			if (PayWall != null)
				visualSize.y += ViewController.Area.Height;

			return visualSize;
		}

		private void SetView(Vector3 baseWorldPosition)
		{
			Vector2 visualSize = GetVisualSize();

			view.Set(visualSize, FishTankArea.Center, NiceName, baseWorldPosition);

			if (PayWall != null)
			{
				Vector3 paywallPos = new Vector3();
				paywallPos.y = (visualSize.y / 2f);
				PayWall.SetView(paywallPos);
			}

			CreateFishViews();
			SetAllFish();
			DistriubuteFish();
		}

		public void ReleaseViews()
		{
			view = null;
			for (int i = 0; i < fishControllers.Length; i++)
				fishControllers[i].ReleaseView();
		}

		private void CreateFishViews()
		{
			for (int i = 0; i < fishControllers.Length; i++)
				fishControllers[i].CreateView(view.FishTank);
		}

		public bool CompareView(SeasonView otherView) => view == otherView;

		public void ParentToView(Transform child)
		{
			if (!HasView || child.parent == view.transform)
				return;

			child.SetParent(view.transform);
		}

		private void SetAllFish()
		{
			for (int i = 0; i < fishControllers.Length; i++)
				SetFish(fishControllers[i]);
		}

		private void SetFish(FishController fishController)
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
				
				if (!fishControllers[i].IsBaotFear())
					fishControllers[i].Appear();
				
				localPosition.y -= verticalStep;
			}
		}

		public void OnFishCoolDown(FishController fishController)
		{
			SetFish(fishController);
			fishController.Appear();
		}

		public bool ValidatePosition(Vector3 worldPos)
		{
			if (!HasView)
			{
				Debug.LogError("[Season] View must be created first");
				return false;
			}

			Area2D worldArea = new Area2D(FishTankArea.Width, FishTankArea.Height, view.FishTank.transform.position);
			return MathUtils.IsInRectArea(worldArea, worldPos);
		}

		public IAquaticCreature OnCast(Vector3 worldPosition)
		{
			DebugUtils.Log("Searching for Fish  ~~~~ >sSD");

			Vector3 localPosition;
			if (HasView)
				localPosition = view.FishTank.transform.InverseTransformPoint(worldPosition);
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

		public void OnBoatMovement()
		{
			for (int i = 0; i < fishControllers.Length; i++)
			{
				if (!fishControllers[i].CanCatch)
					continue;

				fishControllers[i].FearOfBoats();
			}
		}
	}
}
