using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Constants;
using Assets.Scripts.Controllers;
using Assets.Scripts.Framework.Ui.Screens;

namespace Assets.Scripts.Ui.Screens
{
	public class SpeedScreen : RDSimpleScreenBase
	{
		public static SpeedScreen Screen { get; private set; }

		public override int Index { get; } = (int)ScreenType.SpeedScreen;
		public readonly SpeedImageHandler SpeedImageHandler = new SpeedImageHandler();

		protected override void Awake()
		{
			if (Screen == null)
				Screen = this;
			else
				Destroy(gameObject);

			SpeedImageHandler.Init(rectTransform);
			base.Awake();
		}

		public static void Show(float duration, Action onComplete = null)
		{
			if (Screen == null)
				return;

			ScreenManager.ME.ShowHideScreen(Screen, true, () => 
			{
				Screen.SpeedImageHandler.Show(duration, onComplete: () => 
				{
					ScreenManager.ME.ShowHideScreen(Screen, false, onComplete);
				});
			});
		}
	}

	public class SpeedImageHandler
	{
		private Image bgImage;
		private Image tileImage;
		private Image mainImage;


		public void Init(Transform parent)
		{
			bgImage = parent.transform.Find("Speedimage_grp/ImageSpeedPlateBgUnder").GetComponent<Image>();
			tileImage = bgImage.transform.Find("ImageSpeedPlateBg/ImageSpeedPlateTile").GetComponent<Image>();
			mainImage = parent.transform.Find("Speedimage_grp/ImageMain").GetComponent<Image>();

			bgImage.gameObject.SetActive(false);
			mainImage.gameObject.SetActive(false);
		}

		public void SetBgColor(Color color) => bgImage.color = color;

		public void SetTileColor(Color color) => tileImage.color = color;
		public void SetTileSprite(Sprite sprite) => tileImage.sprite = sprite;
		public void SetMainSprite(Sprite sprite) => mainImage.sprite = sprite;

		public void Show(float duration, Action onComplete = null)
		{
			bgImage.gameObject.SetActive(true);
			mainImage.gameObject.SetActive(true);

			ShowElement(bgImage.rectTransform, duration, direction: -1, OnComplete);
			ShowElement(mainImage.rectTransform, duration, direction: 1);

			void OnComplete()
			{
				bgImage.gameObject.SetActive(false);
				mainImage.gameObject.SetActive(false);
				onComplete?.Invoke();
			}
		}

		private void ShowElement(RectTransform element, float duration, int direction, Action onComplete = null)
		{
			const float NORMALISED_DISTANCE = 0.025f;
			float inOutDuration = duration * 0.2f;
			duration -= (inOutDuration * 2f);

			Vector3 anchoredPos = element.anchoredPosition;
			anchoredPos.x += (ViewController.Area.HalfWidth + bgImage.rectTransform.rect.width) * direction;
			element.anchoredPosition = anchoredPos;

			// Intro
			element.DOAnchorPosX(anchoredPos.x * NORMALISED_DISTANCE, inOutDuration).SetEase(Ease.InSine).onComplete = OnIntroComplete;
			void OnIntroComplete() =>
				element.DOAnchorPosX((anchoredPos.x * NORMALISED_DISTANCE) * -1, duration).SetEase(Ease.Linear).onComplete = OnMainComplete;
			void OnMainComplete() =>
				element.DOAnchorPosX(anchoredPos.x * -1, inOutDuration).SetEase(Ease.OutSine).onComplete = OnComplete;
			void OnComplete() => onComplete?.Invoke();
		}
	}
}
