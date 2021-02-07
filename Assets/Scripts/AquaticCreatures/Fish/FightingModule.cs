using UnityEngine;
using System.Collections;
using Assets.Scripts.Rods;
using Assets.Scripts.Utils;
using Assets.Scripts.Constants;
using Assets.Scripts.Controllers;
using Assets.Scripts.Framework.Tools;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.AquaticCreatures.Fish
{
	public class FightingModule
	{
		private readonly Vector3[] directionVectors = new Vector3[]
		{
			new Vector3(0.5f, 0.5f, 0),
			new Vector3(0f, 1f, 0f),
			new Vector3(0f, 0.5f,0.5f)
		};

		private readonly AccelerationModule accelerationModule;
		private readonly FishController fishController;
		private readonly RodBase rod;
		private readonly Vector2 realInTarget;
		private readonly Area2D area;


		public FightingModule(FishController fishController, RodBase rod)
		{
			this.fishController = fishController;
			this.rod = rod;

			accelerationModule = new AccelerationModule(OnMove, fishController.GetViewWorldPosition);
			accelerationModule.SetBounds(CheckPosition);
			CoroutineRunner.RunCoroutine(FightCoroutine());
			realInTarget = new Vector2(0f, ViewController.Area.BottomRightCorner.y);

			area = new Area2D(ViewController.CurrentSeason.FishTankArea.Width, ViewController.Area.Height, ViewController.MainCamera.transform.position);
		}

		private void OnMove(Vector3 newPos) => fishController.ManualSwim(newPos);

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

		private IEnumerator FightCoroutine()
		{
			int directionIndex = 0;
			float redirectStartTime = 0f;
			float redirectTime = 0;
			PullState fishPullState = PullState.None;

			float startRestingTime = 0;

			while (true)
			{
				if (Time.time - redirectStartTime >= redirectTime)
				{
					redirectTime = Random.Range(2f, 4f);
					redirectStartTime = Time.time;
					directionIndex = RedirectFish(directionIndex);
					fishPullState = FishPullState(directionIndex);
				}

				if (!fishController.Energy.IsResting)
					accelerationModule.SetSpeed(0.05f * Time.deltaTime);

				if (rod.Net.IsIn(fishController.GetViewWorldPosition()))
				{
					rod.CaughtFish(fishController.Caught());
					yield break;
				}

				if (rod.Energy.IsResting && Time.time - startRestingTime >= 3f)
				{
					rod.FishEscaped();
					fishController.Escape();
					yield break;
				}

				if (rod.PullState == fishPullState && !rod.Energy.IsResting)
				{
					rod.Energy.Rest();
					startRestingTime = Time.time;
				}
				else
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
		}

		private int RedirectFish(int directionIndex)
		{
			directionIndex += Random.Range(1, directionVectors.Length);
			directionIndex = MathUtils.LoopIndex(directionIndex, directionVectors.Length);
			accelerationModule.SetDirection(directionVectors[directionIndex]);
			return directionIndex;
		}

		private PullState FishPullState(int directionIndex)
		{
			switch (directionIndex)
			{
				case 0: return PullState.Left;
				case 1: return PullState.None;
				case 2: return PullState.Right;
				default: DebugUtils.LogError("[FightModule] Unable to Handle directiuonIndex: " + directionIndex);
					return PullState.None;
			}
		}

		private void ConsumeFishEnergy() => fishController.Energy.Consume(rod.Energy.BlowBack * Time.deltaTime);
		private void ConsumeRodEnergy() => rod.Energy.Consume(fishController.Energy.BlowBack * Time.deltaTime);
	}
}
