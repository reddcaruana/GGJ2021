using UnityEngine;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.AssetsManagers;

namespace Assets.Scripts.Constants
{
	public static class FishWiki
	{
		public const float SIZE_PER_WEIGHT_RATIO = 0.5f; // Scale : Weight
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
			type: new FishTypeData(FishType.F3, "0003"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F4 = new FishData(
			type: new FishTypeData(FishType.F4, "0004"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F5 = new FishData(
			type: new FishTypeData(FishType.F5, "0005"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F6 = new FishData(
			type: new FishTypeData(FishType.F6, "0006"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F7 = new FishData(
			type: new FishTypeData(FishType.F7, "0007"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F8 = new FishData(
			type: new FishTypeData(FishType.F8, "0008"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F9 = new FishData(
			type: new FishTypeData(FishType.F9, "0009"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F10 = new FishData(
			type: new FishTypeData(FishType.F10, "0010"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F11 = new FishData(
			type: new FishTypeData(FishType.F11, "0011"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F12 = new FishData(
			type: new FishTypeData(FishType.F12, "0012"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F13 = new FishData(
			type: new FishTypeData(FishType.F13, "0013"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F14 = new FishData(
			type: new FishTypeData(FishType.F14, "0014"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F15 = new FishData(
			type: new FishTypeData(FishType.F15, "0015"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F16 = new FishData(
			type: new FishTypeData(FishType.F16, "0016"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F17 = new FishData(
			type: new FishTypeData(FishType.F17, "0017"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F18 = new FishData(
			type: new FishTypeData(FishType.F18, "0018"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F19 = new FishData(
			 type: new FishTypeData(FishType.F19, "0019"),
			 minWeight: 1f,
			 maxWeight: 5f,
			 baseEnergy: 10f

		 );
		public static readonly FishData F20 = new FishData(
			 type: new FishTypeData(FishType.F20, "0020"),
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
