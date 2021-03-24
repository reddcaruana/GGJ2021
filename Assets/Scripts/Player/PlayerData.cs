using UnityEngine;
using Assets.Scripts.Constants;
using Assets.Scripts.Player.Inventories;

namespace Assets.Scripts.Player
{
	public static class PlayerData
	{
		private const string LAST_VERSION_PLAYED_KEY = "lastVersionPlayed";
		private const string TUTORIAL_KEY = "pdtutorial";


		public static readonly bool IsPlayingNewVersion;
		public static readonly string LastPlayedVersion;
		public static TutorialType TutorialLevel { get; private set; }

		public static readonly LogBook LogBook;
		public static readonly FishInventory FishInventory;
		public static readonly SeasonData SeasonData = new SeasonData();



		static PlayerData()
		{
			LastPlayedVersion = PlayerPrefs.GetString(LAST_VERSION_PLAYED_KEY, "");
			LastPlayedVersion = string.IsNullOrEmpty(LastPlayedVersion) ? "UNKOWN VERSION" : LastPlayedVersion;
			IsPlayingNewVersion = !LastPlayedVersion.Equals(GameInfo.VERSION);

			if (IsPlayingNewVersion)
				PlayerPrefs.SetString(LAST_VERSION_PLAYED_KEY, GameInfo.VERSION);

			TutorialLevel = (TutorialType)PlayerPrefs.GetInt(TUTORIAL_KEY, (int)TutorialType.PlayButton);

			TryLoadFromJsonOrNew(ref LogBook, LogBook.LOG_BOOK_KEY);
			TryLoadFromJsonOrNew(ref FishInventory, FishInventory.FISH_INVENTORY_KEY);

			void TryLoadFromJsonOrNew<T>(ref T obj, string key) where T : new()
			{
				string json = PlayerPrefs.GetString(key, null);
				if (!string.IsNullOrEmpty(json))
					obj = JsonUtility.FromJson<T>(json);
				else
					obj = new T();
			}
		}

		public static void CompletedTutorial()
		{
			int level = (int)TutorialLevel + 1;
			TutorialLevel = (TutorialType)level;
			PlayerPrefs.SetInt(TUTORIAL_KEY, level);
		}
	}
}
