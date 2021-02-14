using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Views.LogBook;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.AquaticCreatures.Fish;

namespace Assets.Scripts.Ui.Screens
{
	public class LogBookScreen : ScreenBase
	{
		private const float PADDING = 60f;
		private static LogBookElementView elementPrefab;
		private static LogBookElementView ElementPrefab
		{
			get
			{
				if (elementPrefab == null)
					elementPrefab = AssetLoader.ME.Loader<GameObject>("Prefabs/Ui/LogBookElement").AddComponent<LogBookElementView>();
				return elementPrefab;
			}
		}

		private List<LogBookElementView> elements = new List<LogBookElementView>();

		protected override void HidePosition()
		{
			Vector2 hiddenPos = rectTransform.anchoredPosition;
			hiddenPos.x += rectTransform.rect.width;
			HidePos = hiddenPos;
		}

		public void Log(FishLogData[] data)
		{
			PrepareElements(data.Length);

			for (int i = 0; i < data.Length; i++)
				elements[i].Set(data[i]);
		}

		private void PrepareElements(int dataLenght)
		{
			if (elements.Count == dataLenght)
				return;

			LogBookElementView temp;
			for (int i = elements.Count; i < dataLenght; i++)
			{
				temp = Instantiate(ElementPrefab, transform);
				PositionElement(temp, i);
				elements.Add(temp);
			}
		}

		private void PositionElement(LogBookElementView element, int index)
		{
			Vector2 position;
			if (index == 0)
				position = new Vector2(0, (rectTransform.rect.height / 2f) - (ElementPrefab.rectTransform.rect.height / 2f) - PADDING);

			else
				position = new Vector2(0, elements[index - 1].rectTransform.anchoredPosition.y - ElementPrefab.rectTransform.rect.height - PADDING);

			element.rectTransform.anchoredPosition = position;
		}
	}
}
