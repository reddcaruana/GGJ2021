using UnityEngine;

namespace Assets.Scripts.Framework.Utils
{
	public static class MathUtils
	{
		public static bool IsInCircualarArea(Vector2 center, float radius, Vector2 point) => 
			Vector2.Distance(center, point) <= radius;

		public static bool IsInRectArea(Vector2 topLeftCorner, Vector2 bottomRightCorner, Vector2 position) =>
			(position.x >= topLeftCorner.x && position.x <= bottomRightCorner.x) &&
			(position.y <= topLeftCorner.y && position.y >= bottomRightCorner.y);
	}
}
