using UnityEngine;
using Assets.Scripts.Utils;

namespace Assets.Scripts.Player
{
	public static class PlayerData
	{
		public static readonly LogBook LogBook;

		static PlayerData()
		{
			string json = PlayerPrefs.GetString(LogBook.LOG_BOOK_KEY, null);
			DebugUtils.Log("*-* FromJson: " + json);
			if (!string.IsNullOrEmpty(json))
				LogBook = LogBook.FromJson(json);
			else
				LogBook = new LogBook();
		}
	}
}
