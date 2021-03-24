using UnityEngine;
using  Assets.Scripts.Constants;

namespace Assets.Scripts.AquaticCreatures.Fish
{
	public readonly struct FishData
	{
		public readonly FishTypeData Type;
		public readonly float MinWeight;
		public readonly float MaxWeight;
		public readonly float BaseEnergy;
		public readonly Rarity Rarity;

		public FishData(FishTypeData type, float minWeight, float maxWeight, float baseEnergy, Rarity rarity = Rarity.Common)
		{
			Type = type;
			MinWeight = minWeight;
			MaxWeight = maxWeight;
			BaseEnergy = baseEnergy;
			Rarity = rarity;
		}

		public float GetRandomWeight() =>
			Random.Range(MinWeight, MaxWeight);

		public float IdealWeight() =>
			MinWeight + ((MaxWeight - MinWeight) / 2f);
	}

	[System.Serializable]
	public struct FishTypeData
	{
		[SerializeField] FishType type;
		[SerializeField] string alias;
		[SerializeField] string niceName;

		public FishType Type => type;
		public string Alias => alias;
		public string NiceName => niceName;

		public FishTypeData(FishType type, string alias) : this(type, alias, alias)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="alias is used to laod files"></param>
		/// <param name="niceName"></param>
		public FishTypeData(FishType type, string alias, string niceName)
		{
			this.type = type;
			this.alias = alias;
			this.niceName = niceName;
		}
	}
}
