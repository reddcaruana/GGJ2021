using System;
using Assets.Scripts.Player.Inventories;
using Assets.Scripts.AquaticCreatures.Fish;

namespace Assets.Scripts.Player
{
	[Serializable]
	public class LogBook : InventoryBase<FishLogData>
	{
		public const string LOG_BOOK_KEY = "pdlogbook";
		protected override string PrefKey { get; } = LOG_BOOK_KEY;
	}
}
