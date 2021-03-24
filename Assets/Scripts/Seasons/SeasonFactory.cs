using Assets.Scripts.Player;
using Assets.Scripts.Constants;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Seasons.Specific;

namespace Assets.Scripts.Seasons
{
	public static class SeasonFactory
	{
		public static Season CreateSeason(SeasonAreaType type)
		{
			Season seasons = ConstructSeason(type);
			PlayerData.FishInventory.RegisterToEntryUpdate(seasons.PayWall.RefreshView);
			return seasons;
		}

		private static Season ConstructSeason(SeasonAreaType type)
		{
			switch (type)
			{
				case SeasonAreaType.One: return new SeasonOne();
				case SeasonAreaType.Two: return new SeasonTwo();
				case SeasonAreaType.Three: return new SeasonThree();
				case SeasonAreaType.Four: return new SeasonFour();
				default: RDebugUtils.LogError($"[SeasonFactory] {type} Not supported"); return null;
			}
		}
	}
}
