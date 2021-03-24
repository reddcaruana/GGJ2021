using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Player;
using Assets.Scripts.Constants;
using Assets.Scripts.Framework;
using Assets.Scripts.Framework.Ui;
using Assets.Scripts.Framework.Ui.Screens;
using Assets.Scripts.Framework.Utils.Data;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Framework.Utils.Animations;

namespace Assets.Scripts.Ui.Screens
{
	public class CaughtFishScreen : RDSimpleScreenBase
	{
		public override int Index { get; } = (int)ScreenType.CaughtFish;

		private RDUiTransitionElement bucketTransitionElement;
		private Image fishImage;
		private Image glowImage;

		private PositioningData fishPositioningData;
		private PositioningData glowPositioningData;

		private bool isNew;
		private bool isShiny;

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
			fishPositioningData = new PositioningData(fishImage.rectTransform);
			fishImage.rectTransform.localScale = Statics.VECTOR3_ZERO;

			glowImage = transform.Find("ImageGlow").GetComponent<Image>();
			glowPositioningData = new PositioningData(glowImage.rectTransform);
			glowImage.rectTransform.localScale = Statics.VECTOR3_ZERO;
		}

		public void Set(FishLogData data)
		{
			isNew = PlayerData.LogBook.TryGetData(data.Type, out FishLogData result) && result.Total == 1;
			isShiny = data.Shiny != 0;
			fishImage.material = isShiny ? MaterialStatics.SHINY : null;
			fishImage.sprite = FishWiki.GetSprite(data.Type);
		}

		public override bool Show(Action onComplete = null)
		{
			if (!ShowInstantly())
				return false;

			if (isNew || isShiny)
				GlowAnimation();

			fishImage.rectTransform.DOScale(fishPositioningData.Scale, 1f).SetEase(Ease.OutElastic).onComplete = () => 
			{
				bucketTransitionElement.Show(() => 
				{
					fishImage.rectTransform.DOAnchorPosY(-1500, 0.3f).SetEase(Ease.InSine).onComplete = () =>
					{
						const float startScale = 1f;
						const float targetScale = 0.8f;
						bucketTransitionElement.rectTransform.RDBounceAnimation(0.3f, startScale, targetScale, onComplete: HideBucket);

						void HideBucket() => bucketTransitionElement.Hide(OnComplete);
					};
				});
			};

			void OnComplete()
			{
				fishImage.rectTransform.localScale = Statics.VECTOR3_ZERO;
				fishImage.rectTransform.anchoredPosition3D = fishPositioningData.AnchoredPos3D;

				if (isNew || isShiny)
				{
					glowImage.rectTransform.localScale = Statics.VECTOR3_ZERO;
					glowImage.rectTransform.anchoredPosition3D = glowPositioningData.AnchoredPos3D;
					glowImage.color = Statics.COLOR_WHITE;
				}

				onComplete?.Invoke();
			}

			return true;
		}

		private void GlowAnimation()
		{
			glowImage.rectTransform.DOScale(glowPositioningData.Scale, 0.8f).SetEase(Ease.OutBack).onComplete = OnComplete;

			void OnComplete() => glowImage.DOFade(0, 0.3f);
		}



	}
}
