using UnityEngine;
using Assets.Scripts.Paywalls;
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
			FishWiki.F1,
			FishWiki.F2,
			FishWiki.F3,
			FishWiki.F4,
			FishWiki.F5
		};

		public override int MaxFish { get; } = 5;

		public override PaywallBase PayWall { get; } = new SummerPaywall();

		public Summer(Vector3 baseWorldPosition) : base(baseWorldPosition)
		{
		}
	}
}
