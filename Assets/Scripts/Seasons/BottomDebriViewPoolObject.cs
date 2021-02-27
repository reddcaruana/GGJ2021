using UnityEngine;
using Assets.Scripts.Views.Seasons;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.Seasons
{
	public class BottomDebriViewPoolObject : PoolObject<BottomDebriView>
	{
		private static BottomDebriView prefab;
		public static BottomDebriView Prefab
		{
			get
			{
				if (prefab == null)
					prefab = AssetLoader.ME.Loader<BottomDebriView>("Prefabs/BottomDebri/BottomDebriView");
				return prefab;
			}
		}

		public BottomDebriViewPoolObject() : base(MonoBehaviour.Instantiate(Prefab, null))
		{
		}

		public static void UnloadPrefab() => prefab = null;
	}
}
