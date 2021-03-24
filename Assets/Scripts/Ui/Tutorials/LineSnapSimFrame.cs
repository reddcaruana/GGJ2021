using UnityEngine;
using System.Collections;
using Assets.Scripts.Controllers;

namespace Assets.Scripts.Ui.Tutorials
{
	public class LineSnapSimFrame : CoroutineTutorialSimFrame
	{
		private static readonly Vector2 FishAnchoredPosition = new Vector2(0f, -207f);
		private static readonly Quaternion FishRotation = Quaternion.Euler(0f, 0f, 180f);


		protected override IEnumerator SimCoroutine()
		{
			assetHelper.fishImage.rectTransform.anchoredPosition = FishAnchoredPosition;
			assetHelper.fishImage.rectTransform.rotation = FishRotation;
			assetHelper.rodImage.rectTransform.rotation = Quaternion.identity;
			assetHelper.RodLine.Reset();
			assetHelper.RodLine.ProgressBarUpdate(0, false);
			assetHelper.RodLine.PositionUpdate(assetHelper.startLinePoint.position, assetHelper.fishImage.rectTransform.position);

			float energyCurrent = 0;
			bool fishEscape = false;
			Vector2 fishWorldPosition = assetHelper.fishImage.rectTransform.position;
			float startFishPosY = fishWorldPosition.y;
			float targetFishPosY = ViewController.Area.HalfHeight;
			float fishStep = (targetFishPosY - startFishPosY) / 3f;

			while (true)
			{
				yield return new WaitForSeconds(TutorialSimView.INTERVAL);

				// This allows rod line to update after reseting fish position
				if (!fishEscape)
				{
					energyCurrent += TutorialSimView.STEP;
					assetHelper.RodLine.ProgressBarUpdate(energyCurrent, false);
				}

				if (energyCurrent >= 1 && !fishEscape)
				{
					fishEscape = true;
					assetHelper.RodLine.LineSnap();
				}

				if (fishEscape)
				{
					fishWorldPosition.y += fishStep;
					if (fishWorldPosition.y > targetFishPosY)
					{
						fishWorldPosition.y = startFishPosY;
						fishEscape = false;
						energyCurrent = 0;
						assetHelper.RodLine.Reset();
						assetHelper.RodLine.ProgressBarUpdate(energyCurrent, false);
					}

					assetHelper.fishImage.rectTransform.position = fishWorldPosition;
					assetHelper.RodLine.PositionUpdate(assetHelper.startLinePoint.position, assetHelper.fishImage.rectTransform.position);
				}

				yield return null;
			}
		}
	}
}
