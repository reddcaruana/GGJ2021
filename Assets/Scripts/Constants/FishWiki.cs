using Assets.Scripts.Fish;
using static Assets.Scripts.Constants.EnumList;

namespace Assets.Scripts.Constants
{
	public static class FishWiki
	{
		public const float SIZE_PER_WEIGHT_RATIO = 1; // Scale 1 : Weight 1
		public const float PROXIMITY_FEAR = 0.1f;
		public const float PROXIMITY_BITE = 0.2f;

		public static readonly FishData Awrata = new FishData(
			type: new FishTypeData(FishType.Awrata, "Awrata"),
			minWeight: 1,
			maxWeight: 5
		);
	}
}
