using UnityEngine;
using Assets.Scripts.Views;
using Assets.Scripts.AssetsManagers;

namespace Assets.Scripts.Fish
{
	public class FishController
	{
		public FishData Data { get; private set; }
		public FishView View { get; private set; }

		public FishController(FishData data)
		{
			Data = data;
		}

		public void CreateView(Transform parent)
		{
			View = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<FishView>("Prefabs/Fish/" + Data.Type.NiceName), parent);
		}
	}
}
