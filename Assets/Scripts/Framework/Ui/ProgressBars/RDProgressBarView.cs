using UnityEngine.UI;

namespace Assets.Scripts.Framework.Ui.ProgressBars
{
	public abstract class RDProgressBarView : RDProgressBarViewBase
	{
		public override void Set(bool isVertical, bool inverted)
		{
			progressImage.type = Image.Type.Filled;
			progressImage.fillMethod = isVertical ? Image.FillMethod.Vertical : Image.FillMethod.Horizontal;
			progressImage.fillOrigin = inverted ? 1 : 0;
		}

		public override void ProgressUpdate(float value) => progressImage.fillAmount = value;
	}
}
