using UnityEngine;

namespace Assets.Scripts.Framework.Utils
{
	public static class MathUtils
	{
		public static bool IsInCircualarArea(Vector2 center, float radius, Vector2 point) => 
			Vector2.Distance(center, point) <= radius;
	}
}
