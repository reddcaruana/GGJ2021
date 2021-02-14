using UnityEngine;
using Assets.Scripts.Constants;

namespace Assets.Scripts.Player
{
	public static class PlayerData
	{
		public static readonly LogBook LogBook;

		private const string LAST_SEASON_UNLOCKED_KEY = "lastSeasonUnlocked";
		private static SeasonAreaType lastSeasonUnlocked;
		public static SeasonAreaType LastSeasonUnlocked
		{
			get => lastSeasonUnlocked;
			set
			{
				PlayerPrefs.SetInt(LAST_SEASON_UNLOCKED_KEY, (int)value);
				lastSeasonUnlocked = value;
			}
		}

		static PlayerData()
		{
			// LogBook
			string json = PlayerPrefs.GetString(LogBook.LOG_BOOK_KEY, null);
			if (!string.IsNullOrEmpty(json))
				LogBook = LogBook.FromJson(json);
			else
				LogBook = new LogBook();

			// Last Season unlocked
			int value = PlayerPrefs.GetInt(LAST_SEASON_UNLOCKED_KEY, -1);
			if (value > -1)
				LastSeasonUnlocked = (SeasonAreaType)value;
		}

	}
}
