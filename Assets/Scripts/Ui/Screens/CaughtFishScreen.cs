using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Constants;
using Assets.Scripts.Framework;
using Assets.Scripts.Framework.Ui;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.AquaticCreatures.Fish;

namespace Assets.Scripts.Ui.Screens
{
	public class CaughtFishScreen : ScreenBase
	{
		private RDUiTransitionElement bucketTransitionElement;
		private Image fishImage;

		private Vector3 fishOriginalScale;
		private Vector3 fishOriginalPos;

		protected override void Awake()
		{
			base.Awake();

			bucketTransitionElement = transform.Find("ImageBucket").gameObject.AddComponent<RDUiTransitionElement>();
			bucketTransitionElement.IsMovable = true;
			bucketTransitionElement.ShowTime = 0.5f;
			bucketTransitionElement.HideTime = 1;
			bucketTransitionElement.InitTransitions(RDUiTransitionAttachment.State.Visible);
			Vector2 hiddenPos = rectTransform.anchoredPosition;
			hiddenPos.y -= rectTransform.rect.height;
			bucketTransitionElement.HidePos = hiddenPos;
			bucketTransitionElement.HideInstantly();
			bucketTransitionElement.DisableOnHide = true;

			fishImage = transform.Find("ImageFish").GetComponent<Image>();
			fishOriginalScale = fishImage.rectTransform.localScale;
			fishImage.rectTransform.localScale = Statics.VECTOR3_ZERO;
			fishOriginalPos = fishImage.rectTransform.anchoredPosition;

		}

		public void Show(FishTypeData data, Action onComplete = null)
		{
			ShowInstantly();
			fishImage.sprite = FishWiki.GetSprite(data);
			fishImage.rectTransform.DOScale(fishOriginalScale, 0.5f).SetEase(Ease.OutElastic).onComplete = () => 
			{
				bucketTransitionElement.Show(() => 
				{
					fishImage.rectTransform.DOAnchorPosY(-1500, 0.3f).SetEase(Ease.InSine).onComplete = () =>
					{
						const float startScale = 1f;
						const float targetScale = 0.8f;

						float i = 0;
						float f;
						Vector3 currentScale = bucketTransitionElement.rectTransform.localScale;
						DOTween.To(() => i, (x) =>
						{
							i = x;
							f = i < 0.5f ? i / 0.5f: 1 - ((i - 0.5f) / 0.5f);
							currentScale.y = Mathf.Lerp(startScale, targetScale, f);
							currentScale.x = currentScale.z = MathUtils.VolumePreservation(currentScale.y, startScale);
							bucketTransitionElement.rectTransform.localScale = currentScale;
						}, 1f, 0.3f).onComplete = () =>
						{
							bucketTransitionElement.Hide(OnComplete);
						};
					};
				});
			};

			void OnComplete()
			{
				fishImage.rectTransform.localScale = Statics.VECTOR3_ZERO;
				fishImage.rectTransform.anchoredPosition = fishOriginalPos;
				onComplete?.Invoke();
			}
		}




	}
}
