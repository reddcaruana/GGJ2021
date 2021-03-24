using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Ui.Tutorials
{
	public class ReelInSimFrame : CoroutineTutorialSimFrame
	{
		private static readonly Vector2 FishAnchoredPosition = new Vector2(0f, -85f);
		private static readonly Vector2 FishTargetAnchoredPosition = new Vector2(0, -1105f);
		private static readonly Vector3 FishEulerRotation = new Vector3(0f, 0f, 180f);

		protected override IEnumerator SimCoroutine()
		{
			assetHelper.fishImage.rectTransform.anchoredPosition = FishAnchoredPosition;
			assetHelper.fishImage.rectTransform.rotation = Quaternion.Euler(FishEulerRotation);
			assetHelper.rodImage.rectTransform.rotation = Quaternion.identity;
			
			float energyMax = 0.8f;
			float energyCurrent = energyMax;
			assetHelper.RodLine.Reset();
			assetHelper.RodLine.ProgressBarUpdate(energyCurrent, false);
			assetHelper.RodLine.PositionUpdate(assetHelper.startLinePoint.position, assetHelper.fishImage.rectTransform.position);

			Vector2 fishAnchoredPos = FishAnchoredPosition;
			float fishStep = (FishAnchoredPosition.y - FishTargetAnchoredPosition.y) / 4f;

			while (true)
			{
				yield return new WaitForSeconds(TutorialSimView.INTERVAL);

				if (energyCurrent == energyMax)
				{
					energyCurrent = 0;
					assetHelper.RodLine.ProgressBarUpdate(energyCurrent, true, 0.2f);
				}
				else if (assetHelper.vTrailImage.gameObject.activeSelf)
				{
					assetHelper.vTrailImage.gameObject.SetActive(false);
				}
				else
				{
					fishAnchoredPos.y -= fishStep;
					if (fishAnchoredPos.y < FishTargetAnchoredPosition.y)
					{
						fishAnchoredPos.y = FishAnchoredPosition.y;
						energyCurrent = energyMax;
						assetHelper.RodLine.Reset();
						assetHelper.RodLine.ProgressBarUpdate(energyCurrent, false);
						assetHelper.vTrailImage.gameObject.SetActive(true);
					}

					assetHelper.fishImage.rectTransform.anchoredPosition = fishAnchoredPos;
					assetHelper.RodLine.PositionUpdate(assetHelper.startLinePoint.position, assetHelper.fishImage.rectTransform.position);
				}

				yield return null;
			}
		}
	}
}
