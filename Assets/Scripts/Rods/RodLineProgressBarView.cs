using DG.Tweening;
using UnityEngine;
using DG.Tweening.Core;
using Assets.Scripts.Framework;
using Assets.Scripts.Constants;
using DG.Tweening.Plugins.Options;
using Assets.Scripts.Framework._2DSprites;

namespace Assets.Scripts.Rods
{
	public class RodLineProgressBarView : RDProgressBar2DSpriteView
	{
		private SpriteRenderer progressRecoverySpriteRenderer;
		private Vector3 recoveryScale;
		private TweenerCore<Color, Color, ColorOptions> colorTween;
		private Color originalColor;
		private bool isAnimatingRecovery;

		protected override void Awake()
		{
			base.Awake();
			progressRecoverySpriteRenderer = transform.Find("SpriteProgressRecoveryBar").GetComponent<SpriteRenderer>();
			originalColor = ProgressSpriteRenderer.color;
		}

		public override void DisplayInUi()
		{
			base.DisplayInUi();
			progressRecoverySpriteRenderer.sortingLayerID = GameStatics.UI_SORTING_LAYER;
		}

		protected override void SetInternal()
		{
			progressRecoverySpriteRenderer.transform.localScale = Statics.VECTOR3_ZERO;
		}

		protected override string GetImagePath() => "SpriteProgressBar";

		public void ColorFlashStart(Color a, Color b, float cycleDuration)
		{
			ProgressSpriteRenderer.color = a;
			ColorFlashStart(b, cycleDuration);
		}

		public void ColorFlashStart(Color b, float cycleDuration)
		{
			ColorFlashStop(toOriginalColor: false);

			colorTween = ProgressSpriteRenderer.DOColor(b, cycleDuration);
			colorTween.SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
		}

		public void ColorFlashStop(bool toOriginalColor = true)
		{
			if (colorTween != null && colorTween.IsPlaying())
			{
				colorTween.Kill();
				colorTween = null;
				if (toOriginalColor)
					ProgressSpriteRenderer.color = originalColor;
			}
		}

		public void RecoveryProgressUpdate(float value, float duration)
		{
			recoveryScale = currentScale;
			ProgressUpdate(value);

			if (recoveryScale == currentScale || isAnimatingRecovery)
				return;

			isAnimatingRecovery = true;
			progressRecoverySpriteRenderer.transform.localScale = recoveryScale;
			progressRecoverySpriteRenderer.transform.DOScale(currentScale, duration)
				.SetEase(Ease.InOutSine)
				.onComplete = OnComplete;

			void OnComplete()
			{
				progressRecoverySpriteRenderer.transform.localScale = Statics.VECTOR3_ZERO;
				isAnimatingRecovery = false;
			}
		}
	}
}
