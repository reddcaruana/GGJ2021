using Assets.Scripts.Constants;
using Assets.Scripts.Seasons.Paywalls;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Seasons.Paywalls.Specific;

namespace Assets.Scripts.Seasons.Specific
{
	public class SeasonFour : Season
	{
		public override SeasonAreaType Type { get; } = SeasonAreaType.Four;

		public override string Alias { get; } = "Four";

		public override string NiceName { get; } = "Spring";

		public override int NumberOfSegments { get; } = 5;

		public override FishData[] SupportedFish { get; } = new FishData[]
		{
			FishWiki.F16,
			FishWiki.F17,
			FishWiki.F18,
			FishWiki.F19,
			FishWiki.F20
		};

		public override int MaxFish { get; } = 10;

		public override PaywallBase PayWall { get; } = new SeasonFourPaywall();
	}
}
