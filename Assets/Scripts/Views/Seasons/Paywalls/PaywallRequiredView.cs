using DG.Tweening;
using UnityEngine;
using Assets.Scripts.Constants;
using Assets.Scripts.Framework;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.AquaticCreatures.Fish;

namespace Assets.Scripts.Views.Seasons.Paywalls
{
	public class PaywallRequiredView
	{
		private readonly SpriteRenderer SpriteRenderer;

		public PaywallRequiredView(SpriteRenderer spriteRenderer)
		{
			SpriteRenderer = spriteRenderer;
		}

		public void Set(FishTypeData fishType) =>
			SpriteRenderer.sprite = FishWiki.GetSprite(fishType);

		public void Position(Vector3 localPosition) =>
			SpriteRenderer.transform.localPosition = localPosition;

		public void Refresh(bool isComplete) => SpriteRenderer.material = isComplete ? MaterialStatics.STANDARD : MaterialStatics.OVERLAY;

		public void Enable() =>
			SpriteRenderer.gameObject.SetActive(true);

		public void Disable() =>
			SpriteRenderer.gameObject.SetActive(false);

		public void Notify(float delay, float duration)
		{
			if (delay > 0)
				CoroutineRunner.Wait(delay, Continue);
			else
				Continue();

			void Continue()
			{
				Refresh(true);
				float targetScale = SpriteRenderer.transform.localScale.y;
				SpriteRenderer.transform.localScale = Statics.VECTOR3_ZERO;
				SpriteRenderer.transform.DOScale(targetScale, duration).SetEase(Ease.OutElastic);
			}
		}
	}
}
