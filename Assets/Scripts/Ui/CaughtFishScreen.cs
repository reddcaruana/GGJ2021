using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Framework;
using Assets.Scripts.Framework.Ui;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Constants;

namespace Assets.Scripts.Ui
{
	public class CaughtFishScreen : MonoBehaviour
	{
		private UiTransitionElement bucketTransitionElement;
		private Image fishImage;

		private Vector3 fishOriginalScale;
		private Vector3 fishOriginalPos;

		private void Awake()
		{
			bucketTransitionElement = transform.Find("ImageBucket").gameObject.AddComponent<UiTransitionElement>();
			bucketTransitionElement.InitTransitions(UiTransitionAttachment.State.Visible);
			bucketTransitionElement.IsMovable = true;
			Vector2 pos = bucketTransitionElement.rectTransform.anchoredPosition;
			pos.y -= 1000f;
			bucketTransitionElement.HidePos = pos;
			bucketTransitionElement.ShowTime = 0.5f;
			bucketTransitionElement.HideTime = 1;
			bucketTransitionElement.HideInstantly();

			fishImage = transform.Find("ImageFish").GetComponent<Image>();
			fishOriginalScale = fishImage.rectTransform.localScale;
			fishImage.rectTransform.localScale = Statics.VECTOR3_ZERO;
			fishOriginalPos = fishImage.rectTransform.anchoredPosition;
		}

		public void Show(FishTypeData data, Action onComplete = null)
		{
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
