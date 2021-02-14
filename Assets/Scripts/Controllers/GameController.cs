using UnityEngine;
using Assets.Scripts.Rods;
using Assets.Scripts.Framework;
using Assets.Scripts.Framework.Tools;
using Assets.Scripts.Framework.Input;

namespace Assets.Scripts.Controllers
{
	/* **** NOTES ****
	 * 
	 * - Unable to catch fish on Season 2 
	 */

	public class GameController : RDMonobehaviourSingleton<GameController>
	{
		public RodBase Rod { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			MakePersistent();

			GameFactory.Start();
			ViewController.Init();
		}

		private void Start()
		{
			Rod = new BasicRod();
			Rod.CreateView(ViewController.MainCamera.transform.Find("Boat"));
			Rod.RegisterToOnCastComnplete(ViewController.OnCast);

			InputManager.onClickPosition += OnClick;
			InputManager.onSwipeDetaction += OnSwipe;
			InputManager.onHoldDetaction += OnHold;
		}

		private void OnClick(Vector3 worldPosition)
		{
			Rod.ReelIn();
			Rod.TryCast(worldPosition);
		}

		private void OnHold(bool holding)
		{
			Debug.Log("Hold: " + holding);
			//ViewController.ApplBrakes(holding);
		}

		private void OnSwipe(SwipeData data)
		{
			Vector3 filteredDirection = data.FilteredDirection();

			ShowHideLogBook(filteredDirection);
			TryMoveBoat(filteredDirection);
		}

		private bool TryMoveBoat(Vector3 filteredDirection)
		{

			if (filteredDirection.y == 0)
				return false;

			if (filteredDirection.y > 0)
				ViewController.MoveBack();
			else
				ViewController.MoveFoward();

			return true;
		}

		private void ShowHideLogBook(Vector3 filteredDirection)
		{
			if (filteredDirection.x == 0)
				return;

			ViewController.UiController.ShowHideLogBook(filteredDirection.x < 0);
		}

		public void Update()
		{
			InputManager.Gyro();
		}
	}
}
