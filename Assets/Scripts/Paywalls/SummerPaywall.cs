using Assets.Scripts.Constants;

namespace Assets.Scripts.Paywalls
{
	public class SummerPaywall : PaywallBase
	{
		public override FishRequired[] Required { get; } = new FishRequired[]
		{
			new FishRequired(FishWiki.F1.Type, 1),
			new FishRequired(FishWiki.F2.Type, 1),
			new FishRequired(FishWiki.F3.Type, 1),
			new FishRequired(FishWiki.F4.Type, 1),
			new FishRequired(FishWiki.F5.Type, 1)
		};
	}
}
