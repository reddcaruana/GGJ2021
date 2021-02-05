
namespace Assets.Scripts.Fish
{
	public enum FishType { Awrata, Vopa}

	public readonly struct FishData
	{
		public readonly FishTypeData Type;
	 	public readonly float MinWeight;
	 	public readonly float MaxWeight;

		public FishData(FishTypeData type, float minWeight, float maxWeight)
		{
			Type = type;
			MinWeight = minWeight;
			MaxWeight = maxWeight;
		}
	}

	public readonly struct FishTypeData
	{
		public readonly FishType Type;
		public readonly string NiceName;

		public FishTypeData(FishType type, string niceName)
		{
			Type = type;
			NiceName = niceName;
		}
	}
}
