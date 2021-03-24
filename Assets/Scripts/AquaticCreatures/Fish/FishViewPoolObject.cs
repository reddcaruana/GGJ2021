using UnityEngine;
using Assets.Scripts.Views.Fish;
using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.AquaticCreatures.Fish
{
	public class FishViewPoolObject : PoolObject<FishView>
	{
		private static FishView prefab;
		public static FishView Prefab 
		{ 
			get
			{
				if (prefab == null)
					prefab = AssetLoader.ME.Load<FishView>("Prefabs/Fish/FishBase");
				return prefab;
			}
		}

		public FishViewPoolObject() : base(MonoBehaviour.Instantiate(Prefab, null))
		{
		}

		public static void UnloadPrefab() => prefab = null;
	}
}
