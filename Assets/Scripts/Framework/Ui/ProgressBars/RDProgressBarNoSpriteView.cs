using UnityEngine;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Framework.Ui.ProgressBars
{
	public abstract class RDProgressBarNoSpriteView : RDProgressBarViewBase
	{
		private int scaleAxis;
		private float originalScale;
		private Vector3 currentScale;

		public override void Set(bool isVertical, bool inverted)
		{
			scaleAxis = isVertical ? 1 : 0;
			currentScale = progressImage.rectTransform.localScale;
			originalScale = isVertical ? currentScale.y : currentScale.x;
			progressImage.rectTransform.SetPivot(inverted ? PivotPresets.MiddleRight : PivotPresets.MiddleLeft);
		}
		
		public override void ProgressUpdate(float value)
		{
			currentScale[scaleAxis] = originalScale * value;
			progressImage.rectTransform.localScale = currentScale;
		}
	}
}
