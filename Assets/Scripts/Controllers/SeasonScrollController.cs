using UnityEngine;
using Assets.Scripts.Views.Seasons;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Framework.Tools;

namespace Assets.Scripts.Controllers
{
	public class SeasonScrollController
	{
		private const float SCROLL_SPEED = 0.1f;
		private const float WITH_BRAKE = 0.97f;
		private const float NO_BRAKE = 0.99f;

		private readonly RDAccelerationModule acceleration;
		private readonly SeasonView[] seasons = new SeasonView[2];

		private int currentViewIndex = 0;
		private int NextViewIndex => MathUtils.LoopIndex(currentViewIndex + 1, seasons.Length);

		public bool IsMoving => acceleration.IsMoving;

		public SeasonScrollController()
		{
			Vector3 boatWorldPosition = ViewController.MainCamera.transform.position;
			boatWorldPosition.y -= (ViewController.Area.Height / 2f);

			acceleration = new RDAccelerationModule(MoveUpdate, GetWorldPosition);
			acceleration.SetBounds(CheckPosition);
			//acceleration.SetDrag(0.97f);
		}

		public void Init()
		{
			SeasonView prefab = AssetLoader.ME.Loader<SeasonView>("Prefabs/Seasons/SeasonView");

			seasons[0] = MonoBehaviour.Instantiate(prefab, ViewController.MainParent);
			seasons[1] = MonoBehaviour.Instantiate(prefab, ViewController.MainParent);

			ViewController.CurrentSeason.AssignView(seasons[currentViewIndex], new Vector3(0f, -(ViewController.Area.Height / 2f), 0f));
			ViewController.PeekNextSeason().AssignView(seasons[NextViewIndex], seasons[currentViewIndex].GetTopWorldPosition());

			acceleration.IsActive = true;
		}

		public void Foward() => SetSpeed(-SCROLL_SPEED);
		public void Back() => SetSpeed(SCROLL_SPEED);

		private void SetSpeed(float speed)
		{
			if (!ViewController.CanMove())
				return;

			acceleration.SetSpeed(speed);
		}

		private void MoveUpdate(Vector3 worldPosition)
		{
			int nextViewIndex = NextViewIndex;

			seasons[currentViewIndex].transform.position = worldPosition;
			if (seasons[nextViewIndex].transform.position.y > seasons[currentViewIndex].transform.position.y)
				seasons[nextViewIndex].PositionFromBase(seasons[currentViewIndex].GetTopWorldPosition());
			else
				seasons[nextViewIndex].PositionFromTop(seasons[currentViewIndex].GetBottomWorldPosition());
		}

		private Vector3 GetWorldPosition() => seasons[currentViewIndex].transform.position;

		private Vector3 CheckPosition(Vector3 newPosition)
		{
			float top = seasons[currentViewIndex].GetTopWorldPosition(newPosition).y;
			float bottom = seasons[currentViewIndex].GetBottomWorldPosition(newPosition).y;

			// y = 0 (center of screen) to allow Paywalls come down in view
			if (top > 0 && bottom < ViewController.Area.BottomRightCorner.y)
				return newPosition;

			//Debug.Log("CheckPos.... NOT OK");
			return seasons[currentViewIndex].transform.position;
		}

		public void ApplyBrakes(bool value)
		{
			if (value)
				acceleration.SetDrag(WITH_BRAKE);
			else
				acceleration.SetDrag(NO_BRAKE);
		}
	}
}
