using TMPro;
using System;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Scripts.Constants;
using Assets.Scripts.Framework;
using Assets.Scripts.Framework.Ui;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.AquaticCreatures.Fish;

namespace Assets.Scripts.Views.LogBook
{
	public class LogBookElementView : RDUiObject, IPointerClickHandler
	{
		public static Action<FishType> showDetails;

		public bool toNotify;
		public bool valueIsZero;

		private FishType type;
		private Image fishimage;
		private TextMeshProUGUI inventoryAmountText;

		private void Awake()
		{
			fishimage = GetComponent<Image>();
			fishimage.material = MaterialStatics.OVERLAY;

			inventoryAmountText = fishimage.transform.Find("TextInventoryAmount").GetComponent<TextMeshProUGUI>();
		}

		public void Init(FishData data)
		{
			fishimage.sprite = FishWiki.GetSprite(data.Type);
			type = data.Type.Type;
		}

		public void RefreshMaterial(bool caught)
		{
			if (caught)
				fishimage.material = null;
		}

		public void InventoryAmountUpdate(uint value)
		{
			valueIsZero = value == 0;
			InventoryTextShowHide();
			inventoryAmountText.text = "x" + value.ToString();
		}
			
		private void InventoryTextShowHide() => inventoryAmountText.gameObject.SetActive(!valueIsZero && fishimage.material != MaterialStatics.OVERLAY);

		public void Notify(float duration, float delay)
		{
			toNotify = false;

			if (delay > 0)
				CoroutineRunner.Wait(delay, Continue);
			else
				Continue();

			void Continue()
			{
				RefreshMaterial(true);
				float targetScale = fishimage.transform.localScale.y;
				fishimage.transform.localScale = Statics.VECTOR3_ZERO;
				fishimage.transform.DOScale(targetScale, duration).SetEase(Ease.OutElastic).
					onComplete = InventoryTextShowHide;
			}
		}

		public bool Compare(FishLogData data) => type == data.Type.Type;

		public void OnPointerClick(PointerEventData eventData) => showDetails(type);
	}
}
