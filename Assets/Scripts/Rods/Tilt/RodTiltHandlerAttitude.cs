using UnityEngine;
using System.Collections;
using Assets.Scripts.Framework.Input;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Rods.Tilt
{
	public class RodTiltHandlerAttitude : RodTiltHandler
	{
		private const float DEAD_ZONE_RIGHT = 10f;
		private const float DEAD_ZONE_LEFT = 360f - DEAD_ZONE_RIGHT;

		protected override void InitInternal()
		{
		}

		protected override void StartInternal()
		{
			InputManager.EnableAttitude(true);
			CoroutineRunner.RunCoroutine(TiltReadingCorotine());
		}

		protected override void StopInternal()
		{
			InputManager.EnableAttitude(false);
		}


		private IEnumerator TiltReadingCorotine()
		{
			Quaternion attitude;
			Vector3 attitudeEuler; // not converting directly to euler seems more stable :/

			while (IsActive)
			{
				attitude = InputManager.GetAttitude();
				attitudeEuler = attitude.eulerAngles;

				if (IsInDeadZone(attitudeEuler.y))
					tiltCenter();
				else if (IsRight(attitudeEuler.y))
					tiltRight();
				else if (IsLeft(attitudeEuler.y))
					tiltLeft();

				yield return null;
			}
		}

		private bool IsInDeadZone(float eulerAngle) =>
			MathUtils.InRangeFloat(eulerAngle, DEAD_ZONE_LEFT, 360) || MathUtils.InRangeFloat(eulerAngle, 0, DEAD_ZONE_RIGHT);

		private bool IsLeft(float eulerAngle) => eulerAngle > 180;
		private bool IsRight(float eulerAngle) => eulerAngle < 180;
	}
}
