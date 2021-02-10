using UnityEngine.UI;
using Assets.Scripts.Framework.Tools;

namespace Assets.Scripts.Framework.Ui
{
	public abstract class RDProgressBarViewBase : RDUiObject, IProgressBarView
	{
		protected Image progressImage { get; private set; }

		protected virtual void Awake()
		{
			progressImage = transform.Find(GetImagePath()).GetComponent<Image>();
		}

		protected abstract string GetImagePath();
		public abstract void Set(bool isVertical, bool inverted);
		public abstract void ProgressUpdate(float value);
	}
}
