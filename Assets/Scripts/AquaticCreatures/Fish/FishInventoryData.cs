using System;

namespace Assets.Scripts.AquaticCreatures.Fish
{
	[Serializable]
	public class FishInventoryData : FishLogDataBase
	{
		public FishInventoryData(FishTypeData type, uint total) : base(type, total)
		{
		}

		public void Subtract(uint amount) => total -= amount;
	}
}
