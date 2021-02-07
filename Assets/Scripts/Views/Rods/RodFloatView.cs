using System;
using DG.Tweening;
using UnityEngine;
using Assets.Scripts.Framework;
using Assets.Scripts.AssetsManagers;

namespace Assets.Scripts.Views.Rods
{
	public class RodFloatView : MonoBehaviour
	{
		private readonly Vector3 StartScale = Vector3.one * 2f;
		private Vector3 PeakScale;
		private Vector3 FloatingScale;

		private SpriteRenderer spriteRenderer;

		private void Awake()
		{
			PeakScale = StartScale * 2f;
			FloatingScale = StartScale * 0.7f;

			spriteRenderer = transform.Find("SpriteRodFloat").GetComponent<SpriteRenderer>();
		}

		public void Cast(Vector3 targetWorldPosition, float duration, Action onComplete)
		{
			const float springPercentage = 0.15f;
			float springDuration = duration * springPercentage;
			float haldDuration = (duration * (1 - springPercentage)) / 2f;
			transform.localScale = StartScale;
			transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 10f));
			transform.DOScale(PeakScale, haldDuration).SetEase(Ease.OutSine).onComplete = () => 
			{
				transform.DOScale(FloatingScale * 0.5f, haldDuration).SetEase(Ease.InSine).onComplete = () => 
				{
					transform.DOScale(FloatingScale, springDuration).SetEase(Ease.InOutSine);
				};
			};

			transform.DOLocalRotateQuaternion(Quaternion.identity, duration).SetEase(Ease.OutSine);
			transform.DOMove(targetWorldPosition, duration).onComplete = () => onComplete?.Invoke();
		}

		public void Sink(float duration)
		{
			spriteRenderer.sprite = AssetLoader.ME.Loader<Sprite>("Sprites/RodFloat/RodFloatSunk");
		}

		public void Rise(float duration)
		{
			spriteRenderer.sprite = AssetLoader.ME.Loader<Sprite>("Sprites/RodFloat/RodFloatRise");
		}

		public void SinkAndDisapear(float duration)
		{
			Sink(0);
			spriteRenderer.DOColor(Statics.COLOR_ZERO, duration);
		}

		public void Reset(Vector3 targetWorldPosition, float duration, Action onComplete = null)
		{
			Rise(0);
			spriteRenderer.DOColor(Statics.COLOR_WHITE, duration * 0.2f);

			transform.DOScale(StartScale, duration).SetEase(Ease.InOutSine);
			transform.DOMove(targetWorldPosition, duration).SetEase(Ease.InOutSine).onComplete = () => onComplete?.Invoke();
		}

	}
}
