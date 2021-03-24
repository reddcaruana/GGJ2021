using UnityEngine;
using Assets.Scripts.Constants;

namespace Assets.Scripts.Seasons.Paywalls.Specific
{
	public class SeasonFourPaywall : PaywallBase
	{
		public override FishRequired[] Required { get; } = new FishRequired[]
		{
			new FishRequired(FishWiki.F16.Type, 1),
			new FishRequired(FishWiki.F17.Type, 1),
			new FishRequired(FishWiki.F18.Type, 1),
			new FishRequired(FishWiki.F19.Type, 1),
			new FishRequired(FishWiki.F20.Type, 1)
		};

		protected override Vector3[] FishRequiredLocalPosition { get; } = new Vector3[]
		{
			new Vector3(0f, 2.36f, 0f),
			new Vector3(-0.14f, 1.57f, 0f),
			new Vector3(1.08f, 2.22f, 0f),
			new Vector3(0.9f, 1.22f, 0f),
			new Vector3(1.95f, 1.54f, 0f)
		};
	}
}
