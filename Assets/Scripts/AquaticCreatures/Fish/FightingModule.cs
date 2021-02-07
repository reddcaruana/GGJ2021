using UnityEngine;
using System.Collections;
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
		private readonly FishController controller;
		private readonly Vector2 realInTarget;
		private readonly Area2D area;


		public FightingModule(FishController controller)
		{
			this.controller = controller;
			accelerationModule = new AccelerationModule(OnMove, controller.GetViewWorldPosition);
			accelerationModule.SetBounds(CheckPosition);
			CoroutineRunner.RunCoroutine(FightCoroutine());
			realInTarget = new Vector2(0f, ViewController.Area.BottomRightCorner.y);

			area = new Area2D(ViewController.CurrentSeason.FishTankArea.Width, ViewController.Area.Height, ViewController.MainCamera.transform.position);
		}

		public void ReelIn(float speed)
		{
			accelerationModule.SetDirection(MathUtils.AimAtTarget2D(realInTarget, controller.GetViewWorldPosition()));
			accelerationModule.SetSpeed(speed);
		}

		private IEnumerator FightCoroutine()
		{
			int directionIndex = 0;
			float startTime;
			while (true)
			{
				directionIndex += Random.Range(1, directionVectors.Length);
				directionIndex = MathUtils.LoopIndex(directionIndex, directionVectors.Length);
				accelerationModule.SetDirection(directionVectors[directionIndex]);
				
				// Burst - To change using Energy
				startTime = Time.time;
				while(Time.time - startTime <= 2f)
				{
					accelerationModule.SetSpeed(0.05f);
					yield return new WaitForSeconds(0.2f);
				}

				yield return new WaitForSeconds(Random.Range(1f, 2f));
			}
		}


		private void OnMove(Vector3 newPos)
		{
			controller.ManualSwim(newPos);
		}

		private bool CheckPosition(Vector3 newPos) => MathUtils.IsInRectArea(area, newPos);
	}
}
