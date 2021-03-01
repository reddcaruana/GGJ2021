using Assets.Scripts.Utils;
using Assets.Scripts.Player;
using Assets.Scripts.Constants;
using Assets.Scripts.Seasons.Specific;

namespace Assets.Scripts.Seasons
{
	public static class SeasonFactory
	{
		public static Season CreateSeason(SeasonAreaType type)
		{
			Season seasons = ConstructSeason(type);
			PlayerData.LogBook.RegisterToEntryUpdtae(seasons.PayWall.Refresh);
			return seasons;
		}

		private static Season ConstructSeason(SeasonAreaType type)
		{
			switch (type)
			{
				case SeasonAreaType.One: return new Summer();
				case SeasonAreaType.Two: return new Fall();
				case SeasonAreaType.Three: return new Winter();
				default: DebugUtils.LogError($"[SeasonFactory] {type} Not supported"); return null;
			}
		}
	}
}
