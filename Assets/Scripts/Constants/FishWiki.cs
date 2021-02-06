using UnityEngine;
using Assets.Scripts.AquaticCreatures.Fish;

namespace Assets.Scripts.Constants
{
	public static class FishWiki
	{
		public const float SIZE_PER_WEIGHT_RATIO = 1f; // Scale : Weight
		public const float PROXIMITY_FEAR = 1f;
		public const float PROXIMITY_BITE = 2f;

		public static readonly FishData Awrata = new FishData(
			type: new FishTypeData(FishType.Awrata, "Awrata"),
			minWeight: 1f,
			maxWeight: 5f
		);

		public static readonly FishData Vopa = new FishData(
			type: new FishTypeData(FishType.Vopa, "Vopa"),
			minWeight: 0.5f,
			maxWeight: 1f
		);

		public static FishData GetOneRandom(FishData[] data) =>
			data[Random.Range(0, data.Length)];
	}
}
