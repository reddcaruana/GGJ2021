using UnityEngine;
using Assets.Scripts.Views.Fish;
using Assets.Scripts.AssetsManagers;

namespace Assets.Scripts.AquaticCreatures.Fish
{
	public class FishPoolView
	{
		public FishView FishView { get; private set; }
		public bool IsSpawned { get; private set; }

		public FishPoolView()
		{
			FishView = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<FishView>("Prefabs/Fish/FishBase"), null);
			Despawn();
		}

		public void Spawn(Transform parent, Vector3 position, bool isLocalPos = true)
		{
			Spawn(parent);

			if (isLocalPos)
				FishView.transform.localPosition = position;
			else
				FishView.transform.position = position;

		}

		public void Spawn(Transform parent)
		{
			IsSpawned = true;

			FishView.transform.SetParent(parent);
			FishView.gameObject.SetActive(IsSpawned);
		}

		public void Despawn()
		{
			IsSpawned = false;
			FishView.gameObject.SetActive(IsSpawned);
		}
	}
}
