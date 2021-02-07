using UnityEngine;
using Assets.Scripts.Rods;
using UnityEngine.InputSystem;
using Assets.Scripts.Framework;
using Assets.Scripts.Framework.Tools;
using Assets.Scripts.Player;

namespace Assets.Scripts.Controllers
{
	public class GameController : MonobehaviourSingleton<GameController>
	{
		private Vector2 mousePos;
		public RodBase Rod { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			GameFactory.Start();

			ViewController.Init();
		}

		private void Start()
		{
			Rod = new BasicRod();
			Rod.CreateView(ViewController.MainParent);
			Rod.RegisterToOnCastComnplete(ViewController.CurrentSeason.OnCast);
		}

		private void OnPosition(InputValue inputValue)
		{

			Vector2 value = inputValue.Get<Vector2>();
			mousePos = value;

			float halfX = (Screen.width / 2);
			float padding = halfX * 0.2f;

			if (mousePos.x < (halfX - padding))
				Rod.PullLeft();
			else if (mousePos.x > (halfX + padding))
				Rod.PullRight();
			else
				Rod.NoPull();

		}

		private void OnClick()
		{
			Rod.ReelIn();
			Rod.TryCast(mousePos);
		}

		private void OnUp()
		{
		}

		private void OnDown()
		{
		}

		private void OnRight()
		{
			Rod.PullRight();
		}

		private void OnLeft()
		{
			Rod.PullLeft();
		}
	}
}
