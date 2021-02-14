using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Constants;
using Assets.Scripts.Framework.Ui;
using TMPro;
using UnityEngine.UI;

namespace Assets.Scripts.Views.LogBook
{
	public class LogBookElementView : RDUiObject
	{
		private Image fishimage; 
		private TextMeshProUGUI typeLabel;
		private TextMeshProUGUI weightLabel;
		private TextMeshProUGUI locationLabel;
		private TextMeshProUGUI dateLabel;

		private void Awake()
		{
			fishimage = transform.Find("ImageIcon").GetComponent<Image>();
			typeLabel = transform.Find("TextIconElementType/TextType").GetComponent<TextMeshProUGUI>();
			weightLabel = transform.Find("TextIconElementWeight/TextType").GetComponent<TextMeshProUGUI>();
			locationLabel = transform.Find("TextIconElementLocation/TextType").GetComponent<TextMeshProUGUI>();
			dateLabel = transform.Find("TextIconElementDate/TextType").GetComponent<TextMeshProUGUI>();
		}

		public void Set(FishLogData data)
		{
			fishimage.sprite = FishWiki.GetSprite(data.Type);
			typeLabel.text = data.Type.NiceName;
			weightLabel.text = $"{data.Weight}Kg";
			locationLabel.text = data.SeasonStr;
			dateLabel.text = data.Date.ToString();
		}
	}
}
