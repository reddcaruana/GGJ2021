using UnityEngine;
using System.Collections;
using Assets.Scripts.Rods;
using Assets.Scripts.Constants;
using Assets.Scripts.Views.Fish;
using Assets.Scripts.Controllers;
using Assets.Scripts.Framework.Tools;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.AquaticCreatures.Fish
{
	public class FightingModule
	{
		private const float SPEED_VS_WEIGHT = 0.2f;
		private const float LIMIT_PADDING = 0.8f;
		private const float MAX_REST_TIME = 3;
		private const float WARNING_REST_TIME = MAX_REST_TIME * 0.33f;

		public static PullState FishPullState { get; private set; }

		private static VTrail vTrail;
		private readonly Vector3[] directionVectors = new Vector3[]
		{
			new Vector3(-0.5f, 0.5f, 0),
			new Vector3(0.5f, 0.5f, 0f)
		};

		private readonly RDAccelerationModule accelerationModule;
		private readonly FishController fishController;
		private readonly RodBase rod;
		private readonly Vector2 realInTarget;
		private readonly Area2D area;
		private readonly float fishInitMaxEnergy;
		private int directionIndex;

		public FightingModule(FishController fishController, RodBase rod)
		{
			this.fishController = fishController;
			fishInitMaxEnergy = fishController.Energy.Max;

			this.rod = rod;

			realInTarget = new Vector2(0f, ViewController.Area.BottomRightCorner.y);
			area = new Area2D(
				width: ViewController.CurrentSeason.FishTankArea.Width * LIMIT_PADDING,
				height: ViewController.Area.Height * LIMIT_PADDING,
				center: ViewController.MainCamera.transform.position);

			if (vTrail == null)
			{
				vTrail = new VTrail();
				vTrail.Init();
			}
			
			accelerationModule = new RDAccelerationModule(OnMove, fishController.GetViewWorldPosition);
			accelerationModule.IsActive = true;

			CoroutineRunner.RunCoroutine(FightCoroutine());
		}

		private IEnumerator FightCoroutine()
		{
			Debug.Log("<color=yellow> *-* FIGHT!!!</color>");
			float redirectStartTime = 0f;
			float redirectTime = 0;
			FishPullState = PullState.None;

			float startRestingTime = 0;
			bool fishStartedResting = false;

			vTrail.Size(fishController.Size);
			vTrail.Show();

			while (true)
			{
				if (GameController.ME.IsPaused)
					yield return OnPause();

				ShowHideVTrailOnRest();

				if (!fishController.Energy.IsResting)
				{
					if (fishStartedResting)
						fishStartedResting = false;

					if (GamePausableTime.time - redirectStartTime >= redirectTime)
					{
						redirectTime = Random.Range(4, 6);
						redirectStartTime = GamePausableTime.time;
						directionIndex = RedirectFish();
						FishPullState = GetFishPullState(directionIndex);
					}

					accelerationModule.SetSpeed(SPEED_VS_WEIGHT * (fishController.Energy.Max / fishInitMaxEnergy) * Time.deltaTime);
				}
				else
				{
					if (!fishStartedResting)
					{
						fishStartedResting = true;
						fishController.Energy.ResetProperties(fishController.Energy.Max * 0.8f);
						rod.Energy.Reset();
					}
				}

				if (rod.Net.IsIn(fishController.GetViewWorldPosition()))
				{
					rod.CaughtFish(fishController.Caught());
					break;
				}

				if (!fishController.Energy.IsResting && rod.Energy.IsResting)
				{
					float restingTime = GamePausableTime.time - startRestingTime;

					if (restingTime >= MAX_REST_TIME)
					{
						RDebugUtils.Log("Escaped due to much resting.");
						rod.FishEscaped();
						break;
					}
					else if (!rod.IsLineSlack && restingTime >= WARNING_REST_TIME)
						rod.LineSlack(true);
				}

				if (rod.Energy.Value == 0)
				{
					RDebugUtils.Log("Escaped due to line snap.");
					rod.LineSnap();
					break;
				}

				if (!fishController.Energy.IsResting && rod.PullState == FishPullState && !rod.Energy.IsResting)
				{
					rod.Energy.Rest();
					startRestingTime = GamePausableTime.time;
					RDebugUtils.Log("Rod Start Resting");
				}
				else if (rod.PullState != FishPullState)
				{
					rod.Energy.StopResting();
					if (rod.IsLineSlack)
						rod.LineSlack(false);

					if (RodIsCountering(rod, FishPullState))
					{
						ConsumeFishEnergy();
					}

					if (!fishController.Energy.IsResting)
						ConsumeRodEnergy();
				}

				yield return null;
			}

			vTrail.Hide();
			accelerationModule.IsActive = false;
		}

		private IEnumerator OnPause()
		{
			rod.Energy.IsPaused = true;
			fishController.Energy.IsPaused = true;
			yield return new WaitUntil(() => !GameController.ME.IsPaused);
			rod.Energy.IsPaused = false;
			fishController.Energy.IsPaused = false;
		}

		// Bounds are check after acceleration so the e can keep the fish facing the correct direction
		// This will visually help the player know which direction they need to pull with the rid in order to counter.
		private void OnMove(Vector3 newPos)
		{
			Vector3 lookAhead = (directionVectors[directionIndex] * FishView.AHEAD) + newPos;
			newPos = CheckPosition(newPos);
			fishController.ManualSwim(newPos, lookAhead, isFaceAhead: false);
			vTrail.Update(newPos, lookAhead);
		}


		private Vector3 CheckPosition(Vector3 newPos)
		{
			newPos.x = Mathf.Max(newPos.x, area.TopLeftCorner.x);
			newPos.x = Mathf.Min(newPos.x, area.BottomRightCorner.x);

			newPos.y = Mathf.Min(newPos.y, area.TopLeftCorner.y);
			newPos.y = Mathf.Max(newPos.y, area.BottomRightCorner.y);

			return newPos;
		}

		public void ReelIn(float speed)
		{
			if (fishController.Energy.IsResting && accelerationModule.DirectionVector.y > 0)
				accelerationModule.ResetSpeed();

			accelerationModule.SetDirection(MathUtils.AimAtTarget2D(realInTarget, fishController.GetViewWorldPosition()));
			accelerationModule.SetSpeed(speed);
		}

		private int RedirectFish()
		{
			int directionIndex = Random.Range(0, directionVectors.Length);
			directionIndex = MathUtils.LoopIndex(directionIndex, directionVectors.Length);
			accelerationModule.SetDirection(directionVectors[directionIndex]);
			return directionIndex;
		}

		private static PullState GetFishPullState(int directionIndex)
		{
			switch (directionIndex)
			{
				case 0: return PullState.Left;
				case 1: return PullState.Right;
				default: RDebugUtils.LogError("[FightModule] Unable to Handle directiuonIndex: " + directionIndex);
					return PullState.None;
			}
		}

		public static bool RodIsCountering(RodBase rod, PullState fishPullState)
		{
			return (rod.PullState == PullState.Left && fishPullState == PullState.Right) ||
						(rod.PullState == PullState.Right && fishPullState == PullState.Left);
		}

		public static bool RodIsRestoring(RodBase rod, PullState fishPullState)
		{
			return (rod.PullState == PullState.Left && fishPullState == PullState.Left) ||
						(rod.PullState == PullState.Right && fishPullState == PullState.Right);
		}

		private void ConsumeFishEnergy() => fishController.Energy.Consume(rod.Energy.BlowBack * Time.deltaTime);
		private void ConsumeRodEnergy() => rod.Energy.Consume(fishController.Energy.BlowBack * Time.deltaTime, autoRest: false);

		private void ShowHideVTrailOnRest()
		{
			if (!fishController.Energy.IsResting && !vTrail.IsActive)
				vTrail.Show();

			else if (fishController.Energy.IsResting && vTrail.IsActive)
				vTrail.Hide();
		}
	}
}
