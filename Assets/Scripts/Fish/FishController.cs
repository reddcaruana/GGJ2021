using UnityEngine;
using Assets.Scripts.Constants;
using Assets.Scripts.Views.Fish;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.Framework.Utils;
using static Assets.Scripts.Constants.EnumList;

namespace Assets.Scripts.Fish
{
	public class FishController
	{
		public FishData Data { get; private set; }
		public SeasonAreaType SeasonArea { get; private set; }
		public float Weight { get; private set; }
		public float Size { get; private set; }

		private float proximityFear;
		private float proximityBite;

		public bool HasView => view != null;
		private FishView view; 

		public FishController(FishData data)
		{
			Data = data;
		}

		public void Init(SeasonAreaType area) 
		{
			SeasonArea = area;
			Weight = Data.GetRandomWeight();
			Size = Weight * FishWiki.SIZE_PER_WEIGHT_RATIO;
			proximityFear = Size + (Size * FishWiki.PROXIMITY_FEAR);
			proximityBite = Size + (Size * FishWiki.PROXIMITY_BITE);
		}

		public void CreateView(Transform parent)
		{
			view = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<FishView>("Prefabs/Fish/FishBase"), parent);
			view.Set(this);
		}

		public bool InFearProximity(Vector2 position) => InProximity(proximityFear, position);
		public bool InBiteProximity(Vector2 position) => InProximity(proximityBite, position);

		private bool InProximity(float distance, Vector2 position) =>
			MathUtils.IsInCircualarArea(view.transform.position, distance, position);

		public void OnCast(Vector3 floatPosition)
		{
			if (InFearProximity(floatPosition))
				Escape();
			else if (InBiteProximity(floatPosition))
				ApproachFloat(floatPosition);
		}

		private void Escape()
		{
			Debug.Log("[FishController] Escape");
		}

		private void ApproachFloat(Vector2 floatPosition)
		{
			Debug.Log("[FishController] Approach");
		}
	}
}
