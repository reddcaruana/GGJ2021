using UnityEngine;
using Assets.Scripts.Seasons;
using Assets.Scripts.Framework;
using Assets.Scripts.Framework.Tools;

namespace Assets.Scripts.Controllers
{
	public class GameController : MonobehaviourSingleton<GameController>
	{
		public Season Current { get; private set; }
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

			Current = seasons;
		}
	}
}
