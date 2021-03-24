using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Constants;
using Assets.Scripts.Framework;
using Assets.Scripts.Controllers;
using Assets.Scripts.Framework.Ui.Screens;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.Ui.Screens
{
	public class LogBookDetailsScreen : RDSimpleScreenBase
	{
		private static readonly Color NON_SHINY = new Color(0.2f, 0.2f, 0.2f, 0.7f);
		private static readonly Sprite[] RaritySprites = new Sprite[(int)Rarity.MAX];

		public override int Index { get; } = (int)ScreenType.LogBookDetails;
		protected override bool UseHistory { get; } = true;

		public static FishLogData fishData;

		private Image fishImage;
		private Image rarityMedalImage;
		private Image shinyMedalImage;
		private TextMeshProUGUI amountLabel;
		private TextMeshProUGUI typeLabel;
		private TextMeshProUGUI weightLabel;
		private TextMeshProUGUI locationLabel;
		private TextMeshProUGUI dateLabel;

		private bool hasShiny;
		private uint normalTotal;
		private uint shinyTotal;

		protected override void Awake()
		{
			base.Awake();
			fishImage = transform.Find("ImageIcon").GetComponent<Image>();
			amountLabel = fishImage.transform.Find("TextAmount").GetComponent<TextMeshProUGUI>();
			typeLabel = fishImage.transform.Find("TextType").GetComponent<TextMeshProUGUI>();
			rarityMedalImage = fishImage.transform.Find("ImageRarity").GetComponent<Image>();
			shinyMedalImage = fishImage.transform.Find("ImageShiny").GetComponent<Image>();
			weightLabel = fishImage.transform.Find("TextIconElementWeight/TextType").GetComponent<TextMeshProUGUI>();
			locationLabel = fishImage.transform.Find("TextIconElementLocation/TextType").GetComponent<TextMeshProUGUI>();
			dateLabel = fishImage.transform.Find("TextIconElementDate/TextType").GetComponent<TextMeshProUGUI>();

			transform.Find("ButtonClose").GetComponent<Button>().onClick.AddListener(OnCloseButtonMsg);
			fishImage.transform.Find("ButtonShinySwitch").GetComponent<Button>().onClick.AddListener(OnShinySwitchButtonMsg);

			if (RaritySprites[0] == null)
			{
				for (int i = 0; i < RaritySprites.Length; i++)
					RaritySprites[i] = AssetLoader.ME.Load<Sprite>("Sprites/Rarity/" + (Rarity)i);
			}
		}

		protected override void HidePosition()
		{
			Vector2 hiddenPos = rectTransform.anchoredPosition;
			hiddenPos.x += rectTransform.rect.width;
			HidePos = hiddenPos;
		}

		public override bool Show(Action onComplete = null)
		{
			Set(fishData);
			return base.Show(onComplete);
		}

		private void Set(FishLogData data)
		{
			fishImage.sprite = FishWiki.GetSprite(data.Type);

			rarityMedalImage.sprite = RaritySprites[(int)data.Rarity];

			normalTotal = data.Total;
			shinyTotal = data.Shiny;
			hasShiny = shinyTotal > 0;

			shinyMedalImage.color = hasShiny ? Statics.COLOR_WHITE : NON_SHINY;
			Shiny(false);

			typeLabel.text = data.Type.NiceName;
			weightLabel.text = data.Weight.ToString("0.##") + "Kg";
			locationLabel.text = data.SeasonStr;
			dateLabel.text = data.Date;
		}

		private void ToggleShiny() => Shiny(fishImage.material != MaterialStatics.SHINY);

		private void Shiny(bool value)
		{
			if (hasShiny && value)
			{
				fishImage.material = MaterialStatics.SHINY;
				UpdateAmount(shinyTotal);
			}
			else
			{
				fishImage.material = null;
				UpdateAmount(normalTotal);
			}
		}

		private void UpdateAmount(uint amount) => amountLabel.text = "x" + amount.ToString();

		private void OnCloseButtonMsg() => ScreenManager.ME.ShowHideScreen(this, false);
		private void OnShinySwitchButtonMsg() => ToggleShiny();
	}
}
