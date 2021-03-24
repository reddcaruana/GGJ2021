using System;
using UnityEngine;
using Assets.Scripts.Seasons;
using Assets.Scripts.Constants;

namespace Assets.Scripts.AquaticCreatures.Fish
{
	[Serializable]
	public class FishLogData : FishLogDataBase
	{
		[SerializeField] private SeasonAreaType season;
		[SerializeField] private string seasonStr;
		[SerializeField] private float weight;
		[SerializeField] private string date;
		[SerializeField] private Rarity rarity;
		[SerializeField] private uint shiny;
		[SerializeField] private bool seenInlogBook;

		public SeasonAreaType Season => season;
		public string SeasonStr => seasonStr;
		public float Weight => weight;
		public string Date => date;
		public Rarity Rarity => rarity;
		public uint Shiny => shiny;
		public bool SeenInLogBook => seenInlogBook;

		public FishLogData(IAquaticCreature controller, Season season, uint shiny) :
			this(controller, season, 1, shiny)
		{
		}

		public FishLogData(IAquaticCreature controller, Season season, uint total, uint shiny) : 
			this(season.Type, season.NiceName, controller.Data.Type, controller.Weight, DateTime.Now.ToString(), total, controller.Data.Rarity, shiny)
		{
		}

		public FishLogData(SeasonAreaType season, string seasonStr, FishTypeData type, float weight, string date, uint total, Rarity rarity, uint shiny) : base(type, total)
		{
			this.season = season;
			this.seasonStr = seasonStr;
			this.weight = weight;
			this.date = date.ToString();
			this.rarity = rarity;
			this.shiny = shiny;
			seenInlogBook = false;
		}

		public void MarkAsSeenInLogBook() => seenInlogBook = true;

		public static new FishLogData FromJson(string json) => JsonUtility.FromJson<FishLogData>(json);
		
		public override bool TryCombine(FishLogDataBase b)
		{
			if (!base.TryCombine(b))
				return false;

			FishLogData nB = (FishLogData)b;
			// This B is better
			if (weight < nB.weight)
			{
				weight = nB.weight;
				date = nB.date;
			}

			shiny += nB.shiny;
			
			if (nB.SeenInLogBook)
				MarkAsSeenInLogBook();

			return true;
		}
	}
}
