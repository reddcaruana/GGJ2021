using UnityEngine;
using Assets.Scripts.Utils;
using Assets.Scripts.Constants;

namespace Assets.Scripts.Seasons
{
	public static class SeasonFactory
	{
		public static Season CreateSeason(Transform parent, SeasonAreaType type, Vector3 baseWorldPos)
		{
			Season seasons = ConstructSeason(type, baseWorldPos);
			seasons.CreateView(parent);
			seasons.CreateFishViews();
			seasons.SetAllFish();
			seasons.DistriubuteFish();
			return seasons;
		}

		private static Season ConstructSeason(SeasonAreaType type, Vector3 baseWorldPos)
		{
			switch (type)
			{
				case SeasonAreaType.One: return new Summer(baseWorldPos);
				default: DebugUtils.LogError($"[SeasonFactory] {type} Not supported"); return null;
			}
		}
	}
}
