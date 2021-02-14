using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Framework.Utils.Data;
using Assets.Scripts.Framework;
using Assets.Scripts.Controllers;

namespace Assets.Scripts.Ui.Screens
{
	public class MainMenuScreen : ScreenBase
	{
		private static readonly Quaternion HookHiddenRotation = Quaternion.Euler(new Vector3(0, 0, -114));
		private static readonly Quaternion k1HiddenRotation = Quaternion.Euler(new Vector3(0, 0, 121));
		private static readonly Quaternion k2HiddenRotation = Quaternion.Euler(new Vector3(0, 0, -58));
		private static readonly Quaternion k3HiddenRotation = Quaternion.Euler(new Vector3(0, 0, -160));
		private static readonly Vector3 k4HiddenAnchorPos3D = new Vector3(0, -449, 0);
		private static readonly Quaternion k5HiddenRotation = Quaternion.Euler(new Vector3(0, 0, 118));
		private static readonly Quaternion k6HiddenRotation = Quaternion.Euler(new Vector3(0, 0, -86));
		private static readonly Quaternion k7HiddenRotation = Quaternion.Euler(new Vector3(0, 0, 60));

		private Image bgImage;
		private AnimationHelper helperButtonPlay;
		private AnimationHelper helperButtonSound;
		private AnimationHelper helperButtonLogBook;

		private AnimationHelper helperHook;
		private AnimationHelper helperK1;
		private AnimationHelper helperK2;
		private AnimationHelper helperK3;
		private AnimationHelper helperK4;
		private AnimationHelper helperK5;
		private AnimationHelper helperK6;
		private AnimationHelper helperK7;
		private AnimationHelper helperTitle;

		protected override void Awake()
		{
			base.Awake();

			helperButtonPlay = FindButton("ButtonPlay", delay: 0, durationShow: 1, durationHide: 1, callback: OnPlayButtonMsg);
			helperButtonSound = FindButton("ButtonSound", delay: 0, durationShow: 1, durationHide: 1, callback: OnSoundButtonMsg);
			helperButtonLogBook = FindButton("ButtonLogBook", delay: 0, durationShow: 1, durationHide: 1, callback: OnLogBookButtonMsg);

			AnimationHelper FindButton(string name, float delay, float durationShow, float durationHide, UnityAction callback)
			{
				Button button = transform.Find(name).gameObject.GetComponent<Button>();
				button.onClick.AddListener(callback);
				return new AnimationHelper((RectTransform)button.gameObject.transform, delay, durationShow, durationHide);
			}

			bgImage = transform.Find("ImageBgWater").gameObject.GetComponent<Image>();
			helperHook = new AnimationHelper((RectTransform)transform.Find("ImageHook").transform, 0, 1, 0.4f);

			helperK1 = new AnimationHelper((RectTransform)transform.Find("ImageK1").transform, 0, 1, 0.4f);
			helperK2 = new AnimationHelper((RectTransform)transform.Find("ImageK2").transform, 0, 1, 0.4f);
			helperK3 = new AnimationHelper((RectTransform)transform.Find("ImageK3").transform, 0, 1, 0.4f);
			helperK4 = new AnimationHelper((RectTransform)transform.Find("ImageK4").transform, 0, 1, 0.4f);
			helperK5 = new AnimationHelper((RectTransform)transform.Find("ImageK5").transform, 0, 1, 0.4f);
			helperK6 = new AnimationHelper((RectTransform)transform.Find("ImageK6").transform, 0, 1, 0.4f);
			helperK7 = new AnimationHelper((RectTransform)transform.Find("ImageK7").transform, 0, 1, 0.4f);
			helperTitle = new AnimationHelper((RectTransform)transform.Find("ImageTitle").transform, 0, 1, 0.4f);
		}

		private void OnPlayButtonMsg()
		{
			ViewController.UiController.ShowHideMainMenu(false);
		}

		private void OnSoundButtonMsg()
		{

		}

		private void OnLogBookButtonMsg()
		{

		}

		public override void Show(Action onComplete = null)
		{
			HidePropsInstantly();
			ShowInstantly();
			bgImage.DOFade(1, 1f).SetEase(Ease.InOutSine).onComplete = () => onComplete?.Invoke();

			helperButtonPlay.RectTransform.DOScale(helperButtonPlay.ShowData.Scale, helperButtonPlay.DurationShow).SetEase(Ease.InOutSine);
			helperButtonSound.RectTransform.DOScale(helperButtonSound.ShowData.Scale, helperButtonSound.DurationShow).SetEase(Ease.InOutSine);
			helperButtonLogBook.RectTransform.DOScale(helperButtonLogBook.ShowData.Scale, helperButtonLogBook.DurationShow).SetEase(Ease.InOutSine);

			helperHook.RectTransform.DOLocalRotateQuaternion(helperHook.ShowData.LocalRotation, helperHook.DurationShow).SetEase(Ease.InOutSine);
			helperK1.RectTransform.DOLocalRotateQuaternion(helperK1.ShowData.LocalRotation, helperK1.DurationShow).SetEase(Ease.InOutSine);
			helperK2.RectTransform.DOLocalRotateQuaternion(helperK2.ShowData.LocalRotation, helperK2.DurationShow).SetEase(Ease.InOutSine);
			helperK3.RectTransform.DOLocalRotateQuaternion(helperK3.ShowData.LocalRotation, helperK3.DurationShow).SetEase(Ease.InOutSine);
			helperK4.RectTransform.DOAnchorPos3D(helperK4.ShowData.AnchoredPos3D, helperK4.DurationShow).SetEase(Ease.InOutSine);
			helperK5.RectTransform.DOLocalRotateQuaternion(helperK5.ShowData.LocalRotation, helperK5.DurationShow).SetEase(Ease.InOutSine);
			helperK6.RectTransform.DOLocalRotateQuaternion(helperK6.ShowData.LocalRotation, helperK6.DurationShow).SetEase(Ease.InOutSine);
			helperK7.RectTransform.DOLocalRotateQuaternion(helperK7.ShowData.LocalRotation, helperK7.DurationShow).SetEase(Ease.InOutSine);

			helperTitle.RectTransform.DOScale(0, helperTitle.DurationHide);
		}

		//public override void Hide(Action onComplete = null)
		//{
		//	bgImage.DOFade(0, 1f).SetEase(Ease.InOutSine).onComplete = () => onComplete?.Invoke();

		//	helperButtonPlay.RectTransform.DOScale(0, helperButtonPlay.DurationHide).SetEase(Ease.InOutSine);
		//	helperButtonSound.RectTransform.DOScale(0, helperButtonSound.DurationHide).SetEase(Ease.InOutSine);
		//	helperButtonLogBook.RectTransform.DOScale(0, helperButtonLogBook.DurationHide).SetEase(Ease.InOutSine);

		//	helperHook.RectTransform.DOLocalRotateQuaternion(HookHiddenRotation, helperHook.DurationHide).SetEase(Ease.InOutSine);
		//	helperK1.RectTransform.DOLocalRotateQuaternion(HookHiddenRotation, helperK1.DurationHide).SetEase(Ease.InOutSine);
		//	helperK2.RectTransform.DOLocalRotateQuaternion(HookHiddenRotation, helperK2.DurationHide).SetEase(Ease.InOutSine);
		//	helperK3.RectTransform.DOLocalRotateQuaternion(HookHiddenRotation, helperK3.DurationHide).SetEase(Ease.InOutSine);
		//	helperK4.RectTransform.DOAnchorPos3D(k4HiddenAnchorPos3D, helperK4.DurationHide).SetEase(Ease.InOutSine);
		//	helperK5.RectTransform.DOLocalRotateQuaternion(HookHiddenRotation, helperK5.DurationHide).SetEase(Ease.InOutSine);
		//	helperK6.RectTransform.DOLocalRotateQuaternion(HookHiddenRotation, helperK6.DurationHide).SetEase(Ease.InOutSine);
		//	helperK7.RectTransform.DOLocalRotateQuaternion(HookHiddenRotation, helperK7.DurationHide).SetEase(Ease.InOutSine);

		//	helperTitle.RectTransform.DOScale(0, helperTitle.DurationHide);
		//}

		private void HidePropsInstantly()
		{
			bgImage.DOFade(0, 0);

			helperButtonPlay.RectTransform.localScale = Statics.VECTOR3_ZERO;
			helperButtonSound.RectTransform.localScale = Statics.VECTOR3_ZERO;
			helperButtonLogBook.RectTransform.localScale = Statics.VECTOR3_ZERO;

			helperHook.RectTransform.localRotation = HookHiddenRotation;
			helperK1.RectTransform.localRotation = k1HiddenRotation;
			helperK2.RectTransform.localRotation = k2HiddenRotation;
			helperK3.RectTransform.localRotation = k3HiddenRotation;
			helperK4.RectTransform.anchoredPosition3D = k4HiddenAnchorPos3D;
			helperK5.RectTransform.localRotation = k5HiddenRotation;
			helperK6.RectTransform.localRotation = k6HiddenRotation;
			helperK7.RectTransform.localRotation = k7HiddenRotation;

			helperTitle.RectTransform.localScale = Statics.VECTOR3_ZERO;
		}
	}

	readonly struct AnimationHelper
	{
		public readonly RectTransform RectTransform;
		public readonly PositioningData ShowData;
		public readonly float Delay;
		public readonly float DurationShow;
		public readonly float DurationHide;

		public AnimationHelper(RectTransform rectTransform, float delay, float durationShow, float durationHide)
		{
			RectTransform = rectTransform;
			ShowData = new PositioningData(rectTransform);
			Delay = delay;
			DurationShow = durationShow;
			DurationHide = durationHide;
		}

		public void DoDelay(Action onComplete) => CoroutineRunner.Wait(Delay, onComplete);
	}
}
