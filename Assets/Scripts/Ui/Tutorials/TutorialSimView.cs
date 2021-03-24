using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Rods;
using Assets.Scripts.Framework.Ui;
using System.Collections.Generic;
using Assets.Scripts.Framework;

namespace Assets.Scripts.Ui.Tutorials
{
	public class TutorialSimView : RDUiObject
	{
		public const float INTERVAL = 0.5f;
		public const float STEP = 0.15f;

		private Action onCloseButtonMsg;

		private TutorialSimFrame[] sims;
		private int prevSimIndex;
		private int currentSimIndex;

		private Button closeButton;
		private Button leftButton;
		private Button rightButton;
		private List<Image> knobs = new List<Image>();
		private SimAssetHelper assetHelper;

		private void Awake()
		{
			assetHelper = new SimAssetHelper(transform.Find("ImageBoarder"));

			knobs.Add(transform.Find("ImageKnob").GetComponent<Image>());

			leftButton = transform.Find("ButtonLeft").GetComponent<Button>();
			leftButton.onClick.AddListener(PrevSim);
			rightButton = transform.Find("ButtonRight").GetComponent<Button>();
			rightButton.onClick.AddListener(NextSim);
			closeButton = transform.Find("ButtonClose").GetComponent<Button>();
			closeButton.onClick.AddListener(OnCloseButtoMsg);
		}

		public void Show(Action onComplete = null)
		{
			onCloseButtonMsg = onComplete;
			gameObject.SetActive(true);
		}
		public void Hide() => gameObject.SetActive(false);

		private void OnCloseButtoMsg()
		{
			for (int i = 0; i < sims.Length; i++)
			{
				sims[i].Stop();
				sims[i].Release();
			}

			sims = null;

			Hide();
			onCloseButtonMsg?.Invoke();
			onCloseButtonMsg = null;
		}

		public void LoadSims(TutorialSimFrame[] sims)
		{
			for (int i = 0; i < sims.Length; i++)
				sims[i].LendAssets(assetHelper);

			this.sims = sims;

			prevSimIndex = -1;
			currentSimIndex = 0;
			KnobCreation();
			OnChangedCurrentSim();
		}

		private void ButtonsStateUpdate()
		{
			if (sims.Length == 1)
			{
				leftButton.gameObject.SetActive(false);
				rightButton.gameObject.SetActive(false);
				closeButton.gameObject.SetActive(true);
				return;
			}

			leftButton.gameObject.SetActive(currentSimIndex > 0);
			rightButton.gameObject.SetActive(currentSimIndex < sims.Length - 1);
			closeButton.gameObject.SetActive(currentSimIndex >= sims.Length - 1);
		}

		private void KnobCreation()
		{
			for (int i = 0; i < sims.Length; i++)
			{
				if (i < knobs.Count)
					knobs.Add(Instantiate(knobs[0], knobs[0].transform.parent));
				else
					knobs[i].gameObject.SetActive(true);

				knobs[i].color = Statics.COLOR_GRAY;
			}


			float width = knobs[0].rectTransform.rect.width;
			float spacing = width / 2f;
			Vector2 startPos = knobs[0].rectTransform.anchoredPosition;
			startPos.x = -(((width * sims.Length) + spacing * (sims.Length - 1)) / 2f);
			for (int i = 0; i < knobs.Count; i++)
			{
				if (i >= sims.Length)
					knobs[i].gameObject.SetActive(false);
				else
				{
					knobs[i].rectTransform.anchoredPosition = startPos;
					startPos.x += width + spacing;
				}
			}
		}

		private void KnobsUpdate()
		{
			knobs[currentSimIndex].color = Statics.COLOR_WHITE;

			if (prevSimIndex > -1)
				knobs[prevSimIndex].color = Statics.COLOR_GRAY;
		}

		private void NextSim()
		{
			if (currentSimIndex == sims.Length - 1)
				return;
			MoveSim(1);
		}

		private void PrevSim()
		{
			if (currentSimIndex == 0)
				return;
			MoveSim(-1);
		}

		private void MoveSim(int direction)
		{
			prevSimIndex = currentSimIndex;
			currentSimIndex += direction;

			sims[prevSimIndex].Stop();
			OnChangedCurrentSim();
		}

		private void OnChangedCurrentSim()
		{
			sims[currentSimIndex].Play();

			KnobsUpdate();
			ButtonsStateUpdate();
		}
	}

	public class SimAssetHelper
	{
		public readonly RodLine RodLine = new RodLine();
		public Image fishImage;
		public Image vTrailImage;
		public Image rodImage;
		public Transform startLinePoint;

		public SimAssetHelper(Transform parent)
		{
			fishImage = parent.Find("ImageFish").GetComponent<Image>();
			vTrailImage = fishImage.transform.Find("ImageVtrail").GetComponent<Image>();
			rodImage = parent.Find("ImageRod").GetComponent<Image>();
			startLinePoint = rodImage.transform.Find("RodPoint");

			RodLine.CreateView(parent);
			RodLine.DisplayViewInUI();
			RodLine.PositionUpdate(startLinePoint.position, fishImage.transform.position);
		}
	}
}
