using Assets.Scripts.Framework;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Ui.Tutorials
{
	public class RodRestedEnoughSimFrame : CoroutineTutorialSimFrame
	{
		private const float CYCLE_DURATION = 0.2f;
		private static readonly Vector2 FishAnchoredPosition = new Vector2(309f, -207f);
		private static readonly Vector3 FishEulerRotation = new Vector3(0f, 0f, 135f);
		private static readonly Quaternion RodLeftRotation = Quaternion.Euler(0f, 0f, 35f);
		private static readonly Quaternion RodRightRotation = Quaternion.Euler(0f, 0f, 325f);

		private int direction;

		public RodRestedEnoughSimFrame(int direction)
		{
			this.direction = direction;
		}

		protected override void StopInternal()
		{
			assetHelper.rodImage.rectTransform.localScale = Statics.VECTOR3_ONE;
			base.StopInternal();
		}

		protected override IEnumerator SimCoroutine()
		{
			Vector2 fishAnchoredPos = FishAnchoredPosition;
			fishAnchoredPos.x *= direction < 0 ? -1 : 1;
			assetHelper.fishImage.rectTransform.anchoredPosition = fishAnchoredPos;

			Vector3 fishEulerRotation = FishEulerRotation;
			fishEulerRotation.z *= direction < 0 ? -1 : 1;
			assetHelper.fishImage.rectTransform.rotation = Quaternion.Euler(fishEulerRotation);

			Vector3 currentScale = Statics.VECTOR3_ONE;
			if (direction < 0)
				RodRight();
			else
				RodLeft();

			assetHelper.RodLine.Reset();
			assetHelper.RodLine.ProgressBarUpdate(0, false);
			assetHelper.RodLine.PositionUpdate(assetHelper.startLinePoint.position, assetHelper.fishImage.rectTransform.position);
			assetHelper.RodLine.LineSlackWarning(false, CYCLE_DURATION);

			float energyCurrent = 0.3f;
			bool restoring = false;
			bool isWarning = false;
			float limitMax = 0.8f;
			float limitWarning = 0.6f;
			float limitMin = 0.3f;

			while (true)
			{
				yield return new WaitForSeconds(TutorialSimView.INTERVAL);

				if (!restoring && energyCurrent >= limitMax)
				{
					restoring = true;

					if (direction < 0)
						RodLeft();
					else
						RodRight();
				}

				else if (restoring && !isWarning && energyCurrent <= limitWarning)
				{
					assetHelper.RodLine.LineSlackWarning(true, CYCLE_DURATION);
					isWarning = true;
				}

				else if (restoring && isWarning && energyCurrent <= limitMin)
				{
					assetHelper.RodLine.LineSlackWarning(false, CYCLE_DURATION);
					isWarning = false;
					restoring = false;
					if (direction < 0)
						RodRight();
					else
						RodLeft();
				}

				energyCurrent += TutorialSimView.STEP * (restoring ? -1 : 1);
				assetHelper.RodLine.ProgressBarUpdate(energyCurrent, restoring, duration: TutorialSimView.INTERVAL);
				assetHelper.RodLine.PositionUpdate(assetHelper.startLinePoint.position, assetHelper.fishImage.rectTransform.position);

				yield return null;
			}

			void RodLeft()
			{
				assetHelper.rodImage.rectTransform.rotation = RodLeftRotation;
				currentScale.x = 1;
				assetHelper.rodImage.rectTransform.localScale = currentScale;
			}

			void RodRight()
			{
				assetHelper.rodImage.rectTransform.rotation = RodRightRotation;
				currentScale.x = -1;
				assetHelper.rodImage.rectTransform.localScale = currentScale;
			}
		}
	}
}
