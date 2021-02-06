using UnityEngine;
using Assets.Scripts.Seasons;
using Assets.Scripts.Framework;
using Assets.Scripts.Framework.Tools;
using Assets.Scripts.Rods;
using UnityEngine.InputSystem;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Controllers
{
	public class GameController : MonobehaviourSingleton<GameController>
	{
		Vector2 mousePos;

		public Season CurrentSeason { get; private set; }
		private RodBase rod;
		private Transform TestPernt;

		protected override void Awake()
		{
			base.Awake();
			TestPernt = new GameObject("MainParent").transform;
			GameFactory.Start();
		}

		private void Start()
		{
			Season seasons = new Summer();
			seasons.CreateView(TestPernt);
			seasons.CreateFishViews();
			seasons.SetAllFish();
			seasons.DistriubuteFish();

			CurrentSeason = seasons;

			rod = new BasicRod();
			rod.CreateView(TestPernt);
			rod.RegisterToOnCastComnplete(CurrentSeason.OnCast);
		}

		private void OnPosition(InputValue inputValue)
		{
			Vector2 value = inputValue.Get<Vector2>();
			mousePos = value;
		}

		private void OnClick()
		{
			rod.TryCast(mousePos);
		}
	}
}
