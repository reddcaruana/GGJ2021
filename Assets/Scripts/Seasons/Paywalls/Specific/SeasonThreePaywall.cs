using UnityEngine;
using Assets.Scripts.Constants;

namespace Assets.Scripts.Seasons.Paywalls.Specific
{
	public class SeasonThreePaywall : PaywallBase
	{
		public override FishRequired[] Required { get; } = new FishRequired[]
		{
			new FishRequired(FishWiki.F11.Type, 2),
			new FishRequired(FishWiki.F13.Type, 1),
			new FishRequired(FishWiki.F14.Type, 1),
			new FishRequired(FishWiki.F15.Type, 1)
		};

		protected override Vector3[] FishRequiredLocalPosition { get; } = new Vector3[]
		{
			new Vector3(-2.04f, 1.06f, 0f),
			new Vector3(-1.26f, 0.9f, 0f),
			new Vector3(1.7f, 1.38f, 0f),
			new Vector3(0.98f, 1.16f, 0f),
			new Vector3(1.77f, 0.96f, 0f)
		};
	}
}
