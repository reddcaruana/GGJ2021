using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Scripts.Framework.Utils.Animations
{
	public static class PresetAnimations
	{
		public static void RDBounceAnimation(this Transform transform, float duration, float startScale = 1f, float targetScale = 0.8f, Ease ease = Ease.Linear, Action onComplete = null)
		{
			float i = 0;
			float f;
			Vector3 currentScale = transform.localScale;

			DOTween.To(() => i, (x) =>
			{
				i = x;
				f = i < 0.5f ? i / 0.5f : 1 - ((i - 0.5f) / 0.5f);
				currentScale.y = Mathf.Lerp(startScale, targetScale, f);
				currentScale.x = currentScale.z = MathUtils.VolumePreservation(currentScale.y, startScale);
				transform.localScale = currentScale;
			}, 1f, duration).
			SetEase(ease).
			onComplete = OnComplete;
			
			void OnComplete() => onComplete?.Invoke();
		}
	}
}
