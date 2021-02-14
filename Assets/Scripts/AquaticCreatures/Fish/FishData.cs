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

		public FishData(FishTypeData type, float minWeight, float maxWeight, float baseEnergy)
		{
			Type = type;
			MinWeight = minWeight;
			MaxWeight = maxWeight;
			BaseEnergy = baseEnergy;
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
		[SerializeField] string niceName;

		public FishType Type => type;
		public string NiceName => niceName;

		public FishTypeData(FishType type, string niceName)
		{
			this.type = type;
			this.niceName = niceName;
		}
	}
}
