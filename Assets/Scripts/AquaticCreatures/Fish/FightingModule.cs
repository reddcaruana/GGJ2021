using UnityEngine;
using System.Collections;
using Assets.Scripts.Rods;
using Assets.Scripts.Utils;
using Assets.Scripts.Constants;
using Assets.Scripts.Controllers;
using Assets.Scripts.Framework.Tools;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Views.Fish;

namespace Assets.Scripts.AquaticCreatures.Fish
{
	public class FightingModule
	{
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
		private int directionIndex;

		public FightingModule(FishController fishController, RodBase rod)
		{
			this.fishController = fishController;
			this.rod = rod;

			realInTarget = new Vector2(0f, ViewController.Area.BottomRightCorner.y);
			area = new Area2D(ViewController.CurrentSeason.FishTankArea.Width, ViewController.Area.Height, ViewController.MainCamera.transform.position);

			accelerationModule = new RDAccelerationModule(OnMove, fishController.GetViewWorldPosition);
			accelerationModule.IsActive = true;

			ShowDebug();
			CoroutineRunner.RunCoroutine(FightCoroutine());
		}

		private IEnumerator FightCoroutine()
		{
			float redirectStartTime = 0f;
			float redirectTime = 0;
			PullState fishPullState = PullState.None;

			float startRestingTime = 0;

			while (true)
			{
				DebugViewUpdate();
				DebugInfo(fishPullState);


				if (!fishController.Energy.IsResting)
				{
					if (Time.time - redirectStartTime >= redirectTime)
					{
						redirectTime = Random.Range(4, 6);
						redirectStartTime = Time.time;
						directionIndex = RedirectFish();
						fishPullState = FishPullState(directionIndex);
					}

					accelerationModule.SetSpeed(0.1f * Time.deltaTime);
				}

				if (rod.Net.IsIn(fishController.GetViewWorldPosition()))
				{
					rod.CaughtFish(fishController.Caught());
					break;
				}

				if (!fishController.Energy.IsResting && rod.Energy.IsResting && Time.time - startRestingTime >= 3f)
				{
					DebugUtils.Log("Escaped due to much resting.");
					rod.FishEscaped();
					break;
				}

				if (rod.Energy.Value == 0)
				{
					DebugUtils.Log("Escaped due to line snap.");
					rod.FishEscaped();
					break;
				}

				if (rod.PullState == fishPullState && !rod.Energy.IsResting)
				{
					rod.Energy.Rest();
					startRestingTime = Time.time;
					DebugUtils.Log("Rod Start Resting");
				}
				else if (rod.PullState != fishPullState)
				{
					rod.Energy.StopResting();

					if ((rod.PullState == PullState.Left && fishPullState == PullState.Right) || 
						(rod.PullState == PullState.Right && fishPullState == PullState.Left))
					{
						ConsumeFishEnergy();
						ConsumeRodEnergy();
					}
					else
						ConsumeRodEnergy();
				}

				yield return null;
			}

			accelerationModule.IsActive = false;
			HideDebug();
		}

		// Bounds are check after acceleration so the e can keep the fish facing the correct direction
		// This will visually help the player know which direction they need to pull with the rid in order to counter.
		private void OnMove(Vector3 newPos)
		{
			Vector3 lookAhead = (directionVectors[directionIndex] * FishView.AHEAD) + newPos;
			fishController.ManualSwim(CheckPosition(newPos), lookAhead, isFaceAhead: false);
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

		private PullState FishPullState(int directionIndex)
		{
			switch (directionIndex)
			{
				case 0: return PullState.Left;
				case 1: return PullState.Right;
				default: DebugUtils.LogError("[FightModule] Unable to Handle directiuonIndex: " + directionIndex);
					return PullState.None;
			}
		}

		private void ConsumeFishEnergy() => fishController.Energy.Consume(rod.Energy.BlowBack * Time.deltaTime);
		private void ConsumeRodEnergy() => rod.Energy.Consume(fishController.Energy.BlowBack * Time.deltaTime, autoRest: false);


		[System.Diagnostics.Conditional("RDEBUG")]
		private void ShowDebug() => ViewController.UiController.DebugShow(rod.Energy.Max, fishController.Energy.Max);
		[System.Diagnostics.Conditional("RDEBUG")]
		private void HideDebug() => ViewController.UiController.DebugHide();
		[System.Diagnostics.Conditional("RDEBUG")]
		private void DebugViewUpdate() => ViewController.UiController.UpdateFightValues(rod.Energy.Value, fishController.Energy.Value);
		[System.Diagnostics.Conditional("RDEBUG")]
		private void DebugInfo(PullState fishPullState) => 
			ViewController.UiController.UpdateInfo(
				$"Rod: {rod.PullState}, Fish: {fishPullState}\nDV: {VectorUtils.Print(accelerationModule.DirectionVector)}");
	}
}
