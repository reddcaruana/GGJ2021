using UnityEngine;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.AssetsManagers;

namespace Assets.Scripts.Constants
{
	public static class FishWiki
	{
		public const float SIZE_PER_WEIGHT_RATIO = 1f; // Scale : Weight
		public const float PROXIMITY_FEAR = 0.5f;
		public const float PROXIMITY_BITE = 2f;

		public static readonly FishData F1 = new FishData(
			type: new FishTypeData(FishType.F1, "0001"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F2 = new FishData(
			type: new FishTypeData(FishType.F2, "0002"),
			minWeight: 0.5f,
			maxWeight: 1f,
			baseEnergy: 5f
		);
		public static readonly FishData F3 = new FishData(
			type: new FishTypeData(FishType.F1, "0003"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F4 = new FishData(
			type: new FishTypeData(FishType.F1, "0004"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F5 = new FishData(
			type: new FishTypeData(FishType.F1, "0005"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F6 = new FishData(
			type: new FishTypeData(FishType.F1, "0006"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F7 = new FishData(
			type: new FishTypeData(FishType.F1, "0007"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F8 = new FishData(
			type: new FishTypeData(FishType.F1, "0008"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F9 = new FishData(
			type: new FishTypeData(FishType.F1, "0009"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F10 = new FishData(
			type: new FishTypeData(FishType.F1, "0010"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F11 = new FishData(
			type: new FishTypeData(FishType.F1, "0011"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F12 = new FishData(
			type: new FishTypeData(FishType.F1, "0012"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F13 = new FishData(
			type: new FishTypeData(FishType.F1, "0013"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F14 = new FishData(
			type: new FishTypeData(FishType.F1, "0014"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F15 = new FishData(
			type: new FishTypeData(FishType.F1, "0015"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F16 = new FishData(
			type: new FishTypeData(FishType.F1, "0016"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F17 = new FishData(
			type: new FishTypeData(FishType.F1, "0017"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F18 = new FishData(
			type: new FishTypeData(FishType.F1, "0018"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F19 = new FishData(
			 type: new FishTypeData(FishType.F1, "0019"),
			 minWeight: 1f,
			 maxWeight: 5f,
			 baseEnergy: 10f

		 );
		public static readonly FishData F20 = new FishData(
			 type: new FishTypeData(FishType.F1, "0020"),
			 minWeight: 1f,
			 maxWeight: 5f,
			 baseEnergy: 10f

		 );

		public static FishData GetOneRandom(FishData[] data) =>
			data[Random.Range(0, data.Length)];

		public static Sprite GetSprite(FishTypeData data) =>
			 AssetLoader.ME.Loader<Sprite>("Sprites/Fish/Fish" + data.NiceName);
	}
}
