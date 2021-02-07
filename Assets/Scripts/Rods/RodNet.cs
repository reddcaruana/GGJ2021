using Assets.Scripts.Framework.Utils;
using UnityEngine;

namespace Assets.Scripts.Rods
{
	public class RodNet
	{
		public readonly float Radius;
		public readonly Vector3 CenterWorldPos;

		public RodNet(float radius, Vector3 centerWorldPos)
		{
			Radius = radius;
			CenterWorldPos = centerWorldPos;
		}

		public bool IsIn(Vector2 worldPos) => MathUtils.IsInCircualarArea(CenterWorldPos, Radius, worldPos);
	}
}
