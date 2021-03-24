using System;
using Assets.Scripts.Seasons.Paywalls;
using Assets.Scripts.AquaticCreatures.Fish;

namespace Assets.Scripts.Player.Inventories
{
	[Serializable]
	public class FishInventory : InventoryBase<FishInventoryData>
	{
		public const string FISH_INVENTORY_KEY = "pdfishinventory";
		protected override string PrefKey { get; } = FISH_INVENTORY_KEY;

		public bool TryAdd(FishLogData data) => TryAdd(new FishInventoryData(data.Type, data.Total));

		public void PayWallSubtraction(FishRequired[] required)
		{
			for (int i = 0; i < required.Length; i++)
				SubtractFrom(required[i].Type, required[i].Required);
		}

		public void SubtractFrom(FishTypeData type, uint amount)
		{
			int index = IndexOf(type);
			if (index == -1)
				return;

			log[index].Subtract(amount);

			Save();
			onEntryUpdate?.Invoke();
		}
	}
}
