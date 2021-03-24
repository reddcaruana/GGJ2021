using UnityEngine;
using Assets.Scripts.Framework.AssetsManagers;
using Assets.Scripts.AquaticCreatures.Fish;

namespace Assets.Scripts.Constants
{
	public static class FishWiki
	{
		public const float PROXIMITY_FEAR = 0.5f;
		public const float PROXIMITY_BITE = 2f;
		public const float SHINY_CHANCE = 0.001f; // 1/988

		private const float MAX_SIZE = 6f; // Scale
		private const float MIN_SIZE = 0.8f; // Scale
		private const float SIZE_RANGE = MAX_SIZE - MIN_SIZE; // Scale
		private const float MAX_WEIGHT = 250f;
		private const float MIN_WEIGHT = 0.5f;
		private const float WEIGHT_RANGE = MAX_WEIGHT - MIN_WEIGHT;

		public static FishData TutorialFish => new FishData(
			type: F2.Type,
			minWeight: F2.MinWeight,
			maxWeight: F2.MinWeight,
			baseEnergy: 7f
			);

		// Season One
		public static readonly FishData F1 = new FishData(
			type: new FishTypeData(FishType.F1, "0001", "Dorat"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f,
			rarity: Rarity.Legendary
		);
		public static readonly FishData F2 = new FishData(
			type: new FishTypeData(FishType.F2, "0002", "Alojv"),
			minWeight: 0.5f,
			maxWeight: 1f,
			baseEnergy: 5f
		);
		public static readonly FishData F3 = new FishData(
			type: new FishTypeData(FishType.F3, "0003", "Pinzel"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 8f,
			rarity: Rarity.Epic
		);
		public static readonly FishData F4 = new FishData(
			type: new FishTypeData(FishType.F4, "0004", "Plat"),
			minWeight: 2f,
			maxWeight: 4f,
			baseEnergy: 10f
		);
		public static readonly FishData F5 = new FishData(
			type: new FishTypeData(FishType.F5, "0005", "Kelb"),
			minWeight: 6f,
			maxWeight: 10f,
			baseEnergy: 10f,
			rarity: Rarity.Rare
		);

		// Season Two
		public static readonly FishData F6 = new FishData(
			type: new FishTypeData(FishType.F6, "0006", "Zly"),
			minWeight: 15f,
			maxWeight: 25f,
			baseEnergy: 10f
		);
		public static readonly FishData F7 = new FishData(
			type: new FishTypeData(FishType.F7, "0007", "Fazma"),
			minWeight: 5f,
			maxWeight: 8f,
			baseEnergy: 10f,
			rarity: Rarity.Legendary
		);
		public static readonly FishData F8 = new FishData(
			type: new FishTypeData(FishType.F8, "0008", "Glob"),
			minWeight: 50,
			maxWeight: 80f,
			baseEnergy: 10f,
			rarity: Rarity.Rare
		);
		public static readonly FishData F9 = new FishData(
			type: new FishTypeData(FishType.F9, "0009", "Baga"),
			minWeight: 100f,
			maxWeight: 124f,
			baseEnergy: 10f,
			rarity: Rarity.Epic
		);
		public static readonly FishData F10 = new FishData(
			type: new FishTypeData(FishType.F10, "0010", "Riz"),
			minWeight: 25f,
			maxWeight: 30f,
			baseEnergy: 10f,
			rarity: Rarity.Rare
		);

		// Season Three
		public static readonly FishData F11 = new FishData(
			type: new FishTypeData(FishType.F11, "0011", "Gravat"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F12 = new FishData(
			type: new FishTypeData(FishType.F12, "0012", "Duk"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f,
			rarity: Rarity.Legendary
		);
		public static readonly FishData F13 = new FishData(
			type: new FishTypeData(FishType.F13, "0013", "Gamb"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F14 = new FishData(
			type: new FishTypeData(FishType.F14, "0014", "Balen"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F15 = new FishData(
			type: new FishTypeData(FishType.F15, "0015", "Xizzo"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);

		// Season Four
		public static readonly FishData F16 = new FishData(
			type: new FishTypeData(FishType.F16, "0016", "Stila"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F17 = new FishData(
			type: new FishTypeData(FishType.F17, "0017", "Dino"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F18 = new FishData(
			type: new FishTypeData(FishType.F18, "0018", "Arzela"),
			minWeight: 1f,
			maxWeight: 5f,
			baseEnergy: 10f
		);
		public static readonly FishData F19 = new FishData(
			 type: new FishTypeData(FishType.F19, "0019", "Gamblet"),
			 minWeight: 1f,
			 maxWeight: 5f,
			 baseEnergy: 10f

		 );
		public static readonly FishData F20 = new FishData(
			 type: new FishTypeData(FishType.F20, "0020", "Fazmalet"),
			 minWeight: 1f,
			 maxWeight: 5f,
			 baseEnergy: 10f

		 );

		// **** Remember to update ARRAY if adding new Fish ****

		public static readonly FishData[] ARRAY = new FishData[]
		{
			F1,
			F2,
			F3,
			F4,
			F5,
			F6,
			F7,
			F8,
			F9,
			F10,
			F11,
			F12,
			F13,
			F14,
			F15,
			F16,
			F17,
			F18,
			F19,
			F20
		};

		public static FishData GetOneRandom(FishData[] data)
		{
			int index;
			do
			{
				index = Random.Range(0, data.Length);

				if (Random.value > RarityValue(data[index].Rarity))
					index = -1;

			} while (index == -1);

			return data[index];
		}

		public static float RarityValue(Rarity rarity)
		{
			switch (rarity)
			{
				case Rarity.Common: return 1;
				case Rarity.Rare: return 0.3f;
				case Rarity.Epic: return 0.1f;
				case Rarity.Legendary: return 0.05f;
				default: return -1;
			}
		}

		public static Sprite GetSprite(FishTypeData data) =>
			 AssetLoader.ME.Load<Sprite>("Sprites/Fish/fish" + data.Alias);

		public static float SizeCalculator(float weight) => (((weight - MIN_WEIGHT) / WEIGHT_RANGE) * SIZE_RANGE) + MIN_SIZE;
	}
}
