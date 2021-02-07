using UnityEngine;
using Assets.Scripts.Constants;
using Assets.Scripts.AquaticCreatures.Fish;

namespace Assets.Scripts.Seasons
{
	public class Summer : Season
	{
		public override SeasonAreaType Type { get; } = SeasonAreaType.One;
		public override string NiceName { get; } = "Summer";

		public override int NumberOfSegments { get; } = 3;

		public override FishData[] SupportedFish { get; } = new FishData[]
		{
			FishWiki.Awrata,
			FishWiki.Vopa
		};

		public override int MaxFish { get; } = 5;

		public Summer(Vector3 baseWorldPosition) : base(baseWorldPosition)
		{
		}
	}
}
