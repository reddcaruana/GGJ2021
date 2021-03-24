using UnityEngine;
using Assets.Scripts.Constants;

namespace Assets.Scripts.Player
{
	public class SeasonData
	{
		private const string LAST_SEASON_UNLOCKED_KEY = "lastSeasonUnlocked";
		private const string LAST_SEASON_VISITED_KEY = "lastSeasonVisited";
		private const string LAST_SEASON_CINEMATIC_KEY = "lastSeasonCinematic";

		public readonly SeasonDataValue LastSeasonUnlocked;
		public readonly SeasonDataValue LastSeasonVisited;
		public readonly SeasonDataValue LastSeasonCinematic;

		public SeasonData()
		{
			LastSeasonUnlocked = new SeasonDataValue(LAST_SEASON_UNLOCKED_KEY, (int)SeasonAreaType.MAX);
			LastSeasonVisited = new SeasonDataValue(LAST_SEASON_VISITED_KEY);
			LastSeasonCinematic = new SeasonDataValue(LAST_SEASON_CINEMATIC_KEY, (int)SeasonAreaType.MAX);
		}

		public bool IsOpen(SeasonAreaType seasonType) => 
			LastSeasonUnlocked.Value < SeasonAreaType.MAX && LastSeasonUnlocked.Value >= seasonType;
	}

	public class SeasonDataValue
	{
		private readonly string key;

		private SeasonAreaType value;
		public SeasonAreaType Value
		{
			get => value;
			set
			{
				PlayerPrefs.SetInt(key, (int)value);
				this.value = value;
			}
		}

		public SeasonDataValue(string key, int defaultValue = 0)
		{
			this.key = key;
			value = (SeasonAreaType)PlayerPrefs.GetInt(this.key, defaultValue);
		}
	}
}
