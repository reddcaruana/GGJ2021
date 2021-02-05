using Assets.Scripts.Fish;
using Assets.Scripts.Framework;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
	public class GameController : MonoBehaviour
	{
		private void Awake()
		{
			GameFactory.Start();
		}

		private void Start()
		{
			FishController fish = new FishController(FishWiki.Awrata);
			fish.CreateView(transform);
		}
	}
}
