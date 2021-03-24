using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Ui.Tutorials
{
	public class LineSlackEscapeSimFrame : CoroutineTutorialSimFrame
	{
		private const float CYCLE_DURATION = 0.2f;
		private static readonly Vector2 FishAnchoredPosition = new Vector2(-309f, -207f);
		private static readonly Vector2 FishTargetAnchoredPosition = new Vector2(-599f, 83f);
		private static readonly Vector3 FishEulerRotation = new Vector3(0f, 0f, -135f);
		private static readonly Quaternion RodLeftRotation = Quaternion.Euler(0f, 0f, 35f);

		protected override void StopInternal()
		{
			assetHelper.RodLine.SetViewActive(true);
			base.StopInternal();
		}

		protected override IEnumerator SimCoroutine()
		{
			assetHelper.fishImage.rectTransform.anchoredPosition = FishAnchoredPosition;
			assetHelper.fishImage.rectTransform.rotation = Quaternion.Euler(FishEulerRotation);

			assetHelper.rodImage.rectTransform.rotation = RodLeftRotation;

			float energyCurrent = 1;
			float limitMin = 0.3f;

			assetHelper.RodLine.Reset();
			assetHelper.RodLine.ProgressBarUpdate(energyCurrent, false);
			assetHelper.RodLine.PositionUpdate(assetHelper.startLinePoint.position, assetHelper.fishImage.rectTransform.position);
			assetHelper.RodLine.LineSlackWarning(false, CYCLE_DURATION);

			float fishStepX = (FishTargetAnchoredPosition.x - FishAnchoredPosition.x) / 3f;
			float fishStepY = (FishTargetAnchoredPosition.y - FishAnchoredPosition.y) / 3f;
			Vector3 fishCurrentAnchoredPosition = FishAnchoredPosition;
			bool fishEscape = false;
			bool isWarning = false;

			while (true)
			{
				yield return new WaitForSeconds(TutorialSimView.INTERVAL);

				if (fishEscape)
				{
					fishCurrentAnchoredPosition.x += fishStepX;
					fishCurrentAnchoredPosition.y += fishStepY;
					if (fishCurrentAnchoredPosition.y >= FishTargetAnchoredPosition.y)
					{
						fishCurrentAnchoredPosition = FishAnchoredPosition;
						fishEscape = false;
						energyCurrent = 1;
						assetHelper.RodLine.Reset();
						assetHelper.RodLine.SetViewActive(true);
					}

					assetHelper.fishImage.rectTransform.anchoredPosition = fishCurrentAnchoredPosition;
					assetHelper.RodLine.PositionUpdate(assetHelper.startLinePoint.position, assetHelper.fishImage.rectTransform.position);
				}

				if (!fishEscape)
				{
					energyCurrent -= TutorialSimView.STEP;

					assetHelper.RodLine.ProgressBarUpdate(energyCurrent, true, duration: TutorialSimView.INTERVAL);
					assetHelper.RodLine.PositionUpdate(assetHelper.startLinePoint.position, assetHelper.fishImage.rectTransform.position);

					if (!isWarning && energyCurrent <= limitMin)
					{
						assetHelper.RodLine.LineSlackWarning(true, CYCLE_DURATION);
						isWarning = true;
					}

					else if (energyCurrent <= 0)
					{
						assetHelper.RodLine.LineSlackWarning(false, CYCLE_DURATION);
						assetHelper.RodLine.SetViewActive(false);
						fishEscape = true;
						isWarning = false;
					}
				}

				yield return null;
			}
		}
	}
}
