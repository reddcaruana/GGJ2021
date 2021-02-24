using UnityEngine;

namespace Assets.Scripts.Rods
{
	public class BasicRod : RodBase
	{
		public override Vector3 LineStartLocalPos { get; } = new Vector3(0.51f, 4.46f, 0);
		public override string NiceName { get; } = "Basic";

	}
}
