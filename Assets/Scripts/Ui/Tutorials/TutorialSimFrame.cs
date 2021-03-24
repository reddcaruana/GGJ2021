namespace Assets.Scripts.Ui.Tutorials
{
	public abstract class TutorialSimFrame
	{
		protected SimAssetHelper assetHelper { get; private set; }

		public void LendAssets(SimAssetHelper assetHelper) => this.assetHelper = assetHelper;

		public void Play() => PlayInternal();
		public void Stop() => StopInternal();

		protected abstract void PlayInternal();
		protected abstract void StopInternal();

		public virtual void Release() => assetHelper = null;
	}
}
