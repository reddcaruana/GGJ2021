using Assets.Scripts.Framework;
using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Scripts.Views.Rods
{
	public class RodFloatView : MonoBehaviour
	{
		private readonly Vector3 StartScale = Statics.VECTOR3_ONE;
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
			float haldDuration = duration;
			transform.localScale = StartScale;
			transform.DOScale(PeakScale, haldDuration).onComplete = () => 
			{
				transform.DOScale(FloatingScale, haldDuration);
			};

			transform.DOMove(targetWorldPosition, duration).onComplete = () => onComplete?.Invoke();
		}
	}
}
