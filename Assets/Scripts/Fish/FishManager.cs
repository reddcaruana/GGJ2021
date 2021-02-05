using System.Collections.Generic;
using static Assets.Scripts.Constants.EnumList;

namespace Assets.Scripts.Fish
{
	public static class FishManager
	{
		private static List<FishController>[] fishControllersInSeason;

		static FishManager()
		{
			fishControllersInSeason = new List<FishController>[(int)SeasonAreaType.MAX];
		}

		public static void SpawnInArea(SeasonAreaType seasonArea, FishData fishData)
		{
			List<FishController> seasonFish = fishControllersInSeason[(int)seasonArea];

			if (seasonFish == null)
			{
				seasonFish = new List<FishController>();
				FishController fish = new FishController(fishData);
				fish.Init(seasonArea);
				seasonFish.Add(fish);
			}
		}
	}
}
