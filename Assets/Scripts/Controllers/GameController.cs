using UnityEngine;
using Assets.Scripts.Fish;
using Assets.Scripts.Framework;
using Assets.Scripts.Constants;
using Assets.Scripts.Seasons;
using static Assets.Scripts.Constants.EnumList;

namespace Assets.Scripts.Controllers
{
	public class GameController : MonoBehaviour
	{
		private Transform TestPernt;
		private SpriteRenderer TestArea;

		private void Awake()
		{
			TestPernt = new GameObject("MainParent").transform;
			TestArea = new GameObject("TestArea").AddComponent<SpriteRenderer>();
			TestArea.transform.SetParent(TestPernt);
			TestArea.drawMode = SpriteDrawMode.Sliced;
			TestArea.size = new Vector2(100, 110);

			GameFactory.Start();
		}

		private void Start()
		{
			FishController fish = new FishController(FishWiki.Awrata);
			fish.CreateView(TestPernt);

			Season seasons = new Season(SeasonAreaType.One);
			seasons.CreateView(TestPernt);
		}
	}
}
