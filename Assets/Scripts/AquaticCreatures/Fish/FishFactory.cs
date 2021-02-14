using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.AquaticCreatures.Fish
{
	public static class FishFactory
	{
 		private static FishPoolView[] fishPool;

		public static void Init(int maxFishCount)
		{
			fishPool = new FishPoolView[maxFishCount];
			for (int i = 0; i < fishPool.Length; i++)
				fishPool[i] = new FishPoolView();
		}

		public static FishPoolView Spawn()
		{
			for (int i = 0; i < fishPool.Length; i++)
			{
				if (!fishPool[i].IsSpawned)
					return fishPool[i];
			}

			DebugUtils.LogError("[FishFactory] Ran out of Fish.. This Should NEVER Happen");
			return null;
		}
	}
}
