using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;
using Assets.Scripts.Framework.Input;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Rods.Tilt
{
	public class RodTiltHandlerGyro : RodTiltHandler
	{
		// Calculated by calibration that 0.001 plus and minus the device is completely level
		private const float SENSITIVITY = 6;

		protected override void InitInternal()
		{
		}

		protected override void StartInternal()
		{
			InputManager.EnableGyro(true);
			CoroutineRunner.RunCoroutine(TiltReadingCorotine());
		}

		protected override void StopInternal()
		{
			InputManager.EnableGyro(false);
		}

		/*
		 *  GYRO
		 * X Up and Down
		 * Y Roll
		 * Z Flat Spin
		 */
		private IEnumerator TiltReadingCorotine()
		{
			Vector3 angularVelocity;

			while (IsActive)
			{
				angularVelocity = InputManager.GetGyro();

				if (!MathUtils.InRangeFloat(angularVelocity.y, -SENSITIVITY, SENSITIVITY))
				{
					if (angularVelocity.y > 0)
						tiltRight();
					else
						tiltLeft();
				}

				yield return null;
			}
		}

		public void Calibrate()
		{
			CoroutineRunner.RunCoroutine(CalibrationCoroutine());
		}

		private IEnumerator CalibrationCoroutine()
		{
			DebugUtils.Log("Starting Calibration");

			InputManager.EnableGyro(true);

			Vector3 highestValues = new Vector3();
			Vector3 lowestValues = new Vector3();
			Vector3 angularVelocity;

			float startTime = Time.time;
			while (Time.time - startTime < 10f)
			{
				angularVelocity = InputManager.GetGyro();

				for (int i = 0; i < 3; i++)
				{
					if (angularVelocity[i] > highestValues[i])
						highestValues[i] = angularVelocity[i];

					if (angularVelocity[i] < lowestValues[i])
						lowestValues[i] = angularVelocity[i];
				}

				yield return null;
			}

			InputManager.EnableGyro(false);

			DebugUtils.Log($"Done Calibration. H: {VectorUtils.Print(highestValues)}, L: {VectorUtils.Print(lowestValues)}");
		}
	}
}
