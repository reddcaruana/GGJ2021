using UnityEngine;
using Assets.Scripts.Views.Boat;
using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.Boats
{
	public class Boat
	{
		public bool HasView => View != null;
		public BoatView View { get; private set; }

		public void CreateView(Transform parent)
		{
			View = MonoBehaviour.Instantiate(AssetLoader.ME.Load<GameObject>("Prefabs/Boats/BoatBasicView"), parent).AddComponent<BoatView>();
		}
	}
}
