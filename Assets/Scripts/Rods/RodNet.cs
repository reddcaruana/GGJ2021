using UnityEngine;
using Assets.Scripts.Controllers;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Rods
{
	public class RodNet
	{
		public readonly float Radius;
		public Vector3 CenterWorldPos => new Vector3(0f, ViewController.MainCamera.transform.position.y - (ViewController.Area.Height / 2f));

		public RodNet(float radius)
		{
			Radius = radius;
		}

		public bool IsIn(Vector2 worldPos) => MathUtils.IsInCircualarArea(CenterWorldPos, Radius, worldPos);
	}
}
