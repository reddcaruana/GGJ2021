using System;
using UnityEngine;
using Assets.Scripts.Player;
using Assets.Scripts.Constants;
using Assets.Scripts.Controllers;
using Assets.Scripts.Views.LogBook;
using Assets.Scripts.Seasons.Paywalls;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Framework.AssetsManagers;
using static Assets.Scripts.Framework.Ui.RDUiTransitionAttachment;

namespace Assets.Scripts.Ui.Screens
{
	public class LogBookScreen : TabScreen
	{
		private const int PADDING_Y = 60;
		private const int AMOUNT_X = 3;
		private readonly LogBookElementView[] Elements = new LogBookElementView[FishWiki.ARRAY.Length];

		protected override void Awake()
		{
			LogBookElementView.showDetails = OnShowDetails;
			base.Awake();
		}

		public override int Index { get; } = (int)ScreenType.LogBook;

		private void Start()
		{
			PrepareElements();
		}

		public void TryNotify()
		{
			int newCount = 0;
			for (int i = 0; i < Elements.Length; i++)
			{
				if (!Elements[i].toNotify)
					continue;

				Elements[i].Notify(FishRequired.NOTIFICATION_DURATION, FishRequired.DELAY_PER_NOTIFICATION * newCount);
				newCount++;
			}

			if (newCount > 0)
				PlayerData.LogBook.Save();
		}

		private void PrepareElements()
		{
			RectTransform contentTransform = (RectTransform)transform.Find("ImageBg/Scroll View/Viewport/Content");
			LogBookElementView elementPrefab = AssetLoader.ME.Load<GameObject>("Prefabs/Ui/LogBookElementView").GetComponent<LogBookElementView>();

			float dynamicPadding = (contentTransform.rect.width - (elementPrefab.rectTransform.rect.width * AMOUNT_X)) / (AMOUNT_X + 1);

			LogBookElementView temp;
			Vector2 position = default;
			for (int i = 0; i < Elements.Length; i++)
			{
				temp = Instantiate(elementPrefab, contentTransform);
				temp.Init(FishWiki.ARRAY[i]);
				position = CalculateNextPosition(i, dynamicPadding, temp, position, contentTransform);
				temp.rectTransform.anchoredPosition = position;
				Elements[i] = temp;
			}

			Vector2 sizeDelta = contentTransform.sizeDelta;
			sizeDelta.y = Mathf.Abs(Elements[Elements.Length - 1].rectTransform.anchoredPosition.y - PADDING_Y - (elementPrefab.rectTransform.rect.height / 2));
			contentTransform.sizeDelta = sizeDelta;
		}

		private Vector2 CalculateNextPosition(int index, float dynamicPadding, LogBookElementView element, Vector2 position, RectTransform parent)
		{
			if (index == 0)
			{
				position = new Vector2()
				{
					x = InitialPositionX(),
					y = -(element.rectTransform.rect.height / 2f) - PADDING_Y
				};
			}
			else
			{
				if (index % AMOUNT_X == 0)
				{
					position.x = InitialPositionX();
					position.y -= PADDING_Y + element.rectTransform.rect.height;
				}
				else
					position.x += element.rectTransform.rect.width + dynamicPadding;
					
			}

			float InitialPositionX() => -(parent.rect.width / 2f) + (element.rectTransform.rect.width / 2f) + dynamicPadding;

			return position;
		}

		public override bool Show(Action onComplete = null)
		{
			bool result = base.Show(OnComplete);
			
			if (result)
				Log(PlayerData.LogBook.ToArray());

			void OnComplete()
			{
				TryNotify();
				onComplete?.Invoke();
			}

			return result;
		}

		public void Log(FishLogData[] data)
		{
			uint inventoryAmount;
			FishInventoryData inventoryData;
			bool caught;
			for (int i = 0; i < data.Length; i++)
			{
				for (int e = 0; e < Elements.Length; e++)
				{
					if (Elements[e].Compare(data[i]))
					{
						caught = data[i].Total > 0;
						if (caught && !data[i].SeenInLogBook)
						{
							Elements[e].toNotify = true;
							data[i].MarkAsSeenInLogBook();
						}
						else
							Elements[e].RefreshMaterial(caught);

						if (PlayerData.FishInventory.TryGetData(data[i].Type, out inventoryData))
							inventoryAmount = inventoryData.Total;
						else
							inventoryAmount = 0;

						Elements[e].InventoryAmountUpdate(inventoryAmount);
					}
				}
			}
		}

		protected override void OnButtonMsg()
		{
			if (CurrentState == State.Visible && ScreenManager.ME_.ScreenState(ScreenType.LogBookDetails) != State.Hidden)
				ScreenManager.ME_.ShowHideScreen(ScreenType.LogBookDetails, false, Continue);

			else
				Continue();

			void Continue() => ScreenManager.ME.ToggleScreen(this);
		}

		private void OnShowDetails(FishType fishType)
		{
			if (CurrentState != State.Visible ||
				ScreenManager.ME_.ScreenState(ScreenType.LogBookDetails) != State.Hidden ||
				!PlayerData.LogBook.TryGetData(fishType, out FishLogData data) ||
				data.Total <= 0)
				return;

			LogBookDetailsScreen.fishData = data;
			ScreenManager.ME_.ShowHideScreen(ScreenType.LogBookDetails, true);
		}
	}
}
