using TMPro;
using Assets.Scripts.Ui.Tools;
using Assets.Scripts.Framework.Tools;

namespace Assets.Scripts.Ui.Screens
{
	public class DebugScreen : ScreenBase
	{
		RDProgressBar progressBarRod = new RDProgressBar();
		RDProgressBar progressBarFish = new RDProgressBar();
		TextMeshProUGUI directionText;

		protected override void Awake()
		{
			base.Awake();

			SimpleProgressBarView rodProgressBarView = transform.Find("ProgressBarViewRod").gameObject.AddComponent<SimpleProgressBarView>();
			SimpleProgressBarView fishProgressBarView = transform.Find("ProgressBarViewFish").gameObject.AddComponent<SimpleProgressBarView>();
			directionText = transform.Find("TextDebug").gameObject.GetComponent<TextMeshProUGUI>();

			rodProgressBarView.Set(isVertical: false, inverted: false);
			fishProgressBarView.Set(isVertical: false, inverted: true);

			progressBarRod.AssignView(rodProgressBarView);
			progressBarFish.AssignView(fishProgressBarView);
		}

		public void UpdateProgressBarMax(float rodMax, float fishMax)
		{
			progressBarRod.SetMax(rodMax);
			progressBarFish.SetMax(fishMax);
		}

		public void UpdateProgressBarValues(float rodValue, float fishValue)
		{
			progressBarRod.Set(rodValue);
			progressBarFish.Set(fishValue);
		}

		public void UpdateInfo(string info) => directionText.text = info;
	}
}
