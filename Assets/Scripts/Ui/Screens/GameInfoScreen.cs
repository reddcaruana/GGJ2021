using TMPro;
using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Assets.Scripts.Constants;
using UnityEngine.EventSystems;
using Assets.Scripts.Controllers;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Framework.Ui.Screens;

namespace Assets.Scripts.Ui.Screens
{
	public class GameInfoScreen : RDSimpleScreenBase, IPointerClickHandler
	{
		public override int Index { get; } = (int)ScreenType.GameInfo;

		private Image bgImage;
		private TextMeshProUGUI versionText;
		private TextMeshProUGUI changeLogText;

		protected override void Awake()
		{
			base.Awake();

			bgImage = transform.Find("ImageBg").GetComponent<Image>();

			versionText = transform.Find("TextVersion").GetComponent<TextMeshProUGUI>();
			versionText.text = GameInfo.VERSION;

			changeLogText = transform.Find("TextChangeLog").GetComponent<TextMeshProUGUI>();
			changeLogText.text = GameInfo.CHANGE_LOG;

		}

		public override bool Show(Action onComplete = null)
		{
			if (!ShowInstantly())
				return false;

			Color initColor = bgImage.color;
			initColor.a = 0;
			bgImage.color = initColor;

			initColor = versionText.color;
			initColor.a = 0;
			versionText.color = initColor;

			initColor = changeLogText.color;
			initColor.a = 0;
			changeLogText.color = initColor;

			bgImage.DOFade(1, 0.2f).SetEase(Ease.InOutSine).onComplete = OnBgFadeComplete;

			void OnBgFadeComplete()
			{
				versionText.DOFade(1, 0.2f).SetEase(Ease.InOutSine);
				CoroutineRunner.Wait(0.1f, () => 
				{
					changeLogText.DOFade(1, 0.2f).SetEase(Ease.InOutSine);
				});
			}

			return true;
		}

		public override bool Hide(Action onComplete = null)
		{
			changeLogText.DOFade(0, 0.2f).SetEase(Ease.InOutSine);
			CoroutineRunner.Wait(0.1f, () =>
			{
			versionText.DOFade(0, 0.2f).SetEase(Ease.InOutSine).onComplete = OnTextFadeComplete;
			void OnTextFadeComplete() => bgImage.DOFade(0, 0.2f).SetEase(Ease.InOutSine).onComplete = OnBgFadeComplete;
			void OnBgFadeComplete()
			{
				HideInstantly();
				onComplete?.Invoke();
				}
			});
			return true;
		}

		public void OnPointerClick(PointerEventData eventData) =>
			ScreenManager.ME_.SwitchScreens(ScreenType.MainMenu);

	}
}
