using UnityEngine;
using Assets.Scripts.Constants;

namespace Assets.Scripts.Seasons.Paywalls.Specific
{
	public class SeasonOnePaywall : PaywallBase
	{
		public override FishRequired[] Required { get; } = new FishRequired[]
		{
			new FishRequired(FishWiki.F2.Type, 1),
			new FishRequired(FishWiki.F3.Type, 2),
			new FishRequired(FishWiki.F4.Type, 1),
			new FishRequired(FishWiki.F5.Type, 1)
		};

		protected override Vector3[] FishRequiredLocalPosition { get; } = new Vector3[]
		{
			new Vector3(1.13f, 1.61f, 0f),
			new Vector3(2.03f, 1.45f, 0f),
			new Vector3(0.85f, 0.92f, 0f),
			new Vector3(1.8f, 0.69f, 0f),
			new Vector3(2.65f, 1.1f, 0f)
		};
	}
}
