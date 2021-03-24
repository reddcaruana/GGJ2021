using Assets.Scripts.Constants;
using Assets.Scripts.Seasons.Paywalls;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Seasons.Paywalls.Specific;

namespace Assets.Scripts.Seasons.Specific
{
	public class SeasonTwo : Season
	{
		public override SeasonAreaType Type => SeasonAreaType.Two;

		public override string Alias { get; } = "Two";
		public override string NiceName { get; } = "Fall";

		public override int NumberOfSegments { get; } = 5;

		public override FishData[] SupportedFish { get; } = new FishData[]
		{
			FishWiki.F6,
			FishWiki.F7,
			FishWiki.F8,
			FishWiki.F9,
			FishWiki.F10
		};

		public override int MaxFish { get; } = 10;

		public override PaywallBase PayWall { get; } = new SeasonTwoPaywall();
	}
}
