using UnityEngine;
using Assets.Scripts.Constants;

namespace Assets.Scripts.Seasons.Paywalls.Specific
{
	public class FallPaywall : PaywallBase
	{
		public override FishRequired[] Required { get; } = new FishRequired[]
		{
			new FishRequired(FishWiki.F6.Type, 1),
			new FishRequired(FishWiki.F7.Type, 1),
			new FishRequired(FishWiki.F8.Type, 1),
			new FishRequired(FishWiki.F9.Type, 1),
			new FishRequired(FishWiki.F10.Type, 1)
		};

		protected override Vector3[] FishRequiredLocalPosition { get; } = new Vector3[]
		{
			new Vector3(1.13f, 1.41f, 0f),
			new Vector3(2.03f, 1.45f, 0f),
			new Vector3(0.85f, 1.72f, 0f),
			new Vector3(1.8f, 0.69f, 0f),
			new Vector3(2.65f, 0.99f, 0f)
		};
	}
}
