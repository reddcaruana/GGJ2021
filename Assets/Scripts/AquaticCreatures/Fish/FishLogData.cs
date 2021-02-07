using System;
using UnityEngine;
using Assets.Scripts.Seasons;
using Assets.Scripts.Constants;

namespace Assets.Scripts.AquaticCreatures.Fish
{
	[Serializable]
	public struct FishLogData
	{
		[SerializeField] private SeasonAreaType season;
		[SerializeField] private string seasonStr;
		[SerializeField] private FishTypeData type;
		[SerializeField] private float weight;
		[SerializeField] private DateTime date;
		[SerializeField] private uint total;

		public SeasonAreaType Season => season;
		public string SeasonStr => seasonStr;
		public FishTypeData Type => type;
		public float Weight => weight;
		public DateTime Date => date;
		public uint Total => total;

		public FishLogData(IAquaticCreature controller, Season season) :
			this(controller, season, 1)
		{
		}

		public FishLogData(IAquaticCreature controller, Season season, uint total) : 
			this(season.Type, season.NiceName, controller.Data.Type, controller.Weight, DateTime.Now, total)
		{
		}

		public FishLogData(SeasonAreaType season, string seasonStr, FishTypeData type, float weight, DateTime date, uint total)
		{
			this.season = season;
			this.seasonStr = seasonStr;
			this.type = type;
			this.weight = weight;
			this.date = date;
			this.total = total;
		}


		public string ToJson() => JsonUtility.ToJson(this);

		public static FishLogData FromJson(string json) => JsonUtility.FromJson<FishLogData>(json);
		
		public static bool TryBest(FishLogData a, FishLogData b, out FishLogData best)
		{
			if (a.Type.Type != b.Type.Type)
			{
				best = default;
				return false;
			}

			FishLogData c = a.weight > b.weight ? a : b;

			best = new FishLogData(c.Season, c.SeasonStr, c.Type, c.Weight, c.Date, a.Total + b.Total);
			return true;
		}
	}
}
