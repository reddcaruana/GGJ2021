using UnityEngine;

namespace Assets.Scripts.Seasons
{
	public static class BottomDebriFactory
	{
		private static readonly string[] DebriArray = new string[]
		{
			"stone1",
			"stone2",
			"stone3",
			"stone4",
			"stone5",
			"stone6"
		};

		private static readonly Sprite[] spriteArray = new Sprite[DebriArray.Length];

		public static SpriteRenderer CreateRandomDebri()
		{
			throw new System.NotImplementedException();
		}

	}
}
