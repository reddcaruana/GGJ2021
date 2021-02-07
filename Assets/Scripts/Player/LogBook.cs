using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Utils;

namespace Assets.Scripts.Player
{
	[System.Serializable]
	public class LogBook
	{
		public const string LOG_BOOK_KEY = "pdlogbook";

		[SerializeField] private List<FishLogData> log = new List<FishLogData>();

		public bool TryAdd(FishLogData data)
		{
			int index = IndexOf(data);

			if (index == -1)
				log.Add(data);

			else if (FishLogData.TryBest(data, log[index], out FishLogData best))
				log[index] = best;

			else
				return false;

			Save();
			return true;
		}

		private int IndexOf(FishLogData data)
		{
			for (int i = 0; i < log.Count; i++)
			{
				if (log[i].Type.Type == data.Type.Type)
					return i;
			}

			return -1;
		}

		public void Save()
		{
			string json = ToJson();
			DebugUtils.Log("*-* ToJson: " + json);
			PlayerPrefs.SetString(LOG_BOOK_KEY, json);
		}
		public string ToJson() => JsonUtility.ToJson(this);
		public static LogBook FromJson(string json) => JsonUtility.FromJson<LogBook>(json);

	}
}
