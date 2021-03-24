using Assets.Scripts.Constants;
using Assets.Scripts.Seasons.Paywalls;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Seasons.Paywalls.Specific;

namespace Assets.Scripts.Seasons.Specific
{
	public class SeasonThree : Season
	{
		public override SeasonAreaType Type { get; } = SeasonAreaType.Three;

		public override string Alias { get; } = "Three";
		public override string NiceName { get; } = "Winter";

		public override int NumberOfSegments { get; } = 5;

		public override FishData[] SupportedFish { get; } = new FishData[]
		{
			FishWiki.F11,
			FishWiki.F12,
			FishWiki.F13,
			FishWiki.F14,
			FishWiki.F15
		};

		public override int MaxFish { get; } = 10;

		public override PaywallBase PayWall { get; } = new SeasonThreePaywall();
	}
}
