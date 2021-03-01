using UnityEngine;
using Assets.Scripts.Constants;

namespace Assets.Scripts.Seasons.Paywalls.Specific
{
	public class WinterPaywall : PaywallBase
	{
		public override FishRequired[] Required { get; } = new FishRequired[]
		{
			new FishRequired(FishWiki.F11.Type, 1),
			new FishRequired(FishWiki.F12.Type, 1),
			new FishRequired(FishWiki.F13.Type, 1),
			new FishRequired(FishWiki.F14.Type, 1),
			new FishRequired(FishWiki.F15.Type, 1)
		};

		protected override Vector3[] FishRequiredLocalPosition { get; } = new Vector3[]
		{
			new Vector3(-1.85f, 0.48f, 0f),
			new Vector3(-0.13f, 0.67f, 0f),
			new Vector3(1.34f, 0.72f, 0f),
			new Vector3(2.1f, 0.83f, 0f),
			new Vector3(2.79f, 0.7f, 0f)
		};
	}
}
