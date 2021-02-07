using UnityEngine;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using Assets.Scripts.AquaticCreatures.Fish;

namespace Assets.Scripts.Player
{
	[System.Serializable]
	public class LogBook
	{
		public const string LOG_BOOK_KEY = "pdlogbook";

		[SerializeField] private List<FishLogData> log = new List<FishLogData>();

		public bool TryAdd(FishLogData data)
		{
			int index = IndexOf(data.Type);

			if (index == -1)
				log.Add(data);

			else if (FishLogData.TryBest(data, log[index], out FishLogData best))
				log[index] = best;

			else
				return false;

			Save();
			return true;
		}

		public int IndexOf(FishTypeData data)
		{
			for (int i = 0; i < log.Count; i++)
			{
				if (log[i].Type.Type == data.Type)
					return i;
			}

			return -1;
		}

		public bool TryGetData(FishTypeData data, out FishLogData result)
		{
			int index = IndexOf(data);
			if (index == -1)
			{
				result = default;
				return false;
			}
			result = log[index];
			return true;
		}

		public FishLogData GetDataAt(int index) => log[index];

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
