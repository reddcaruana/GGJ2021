using UnityEngine;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Framework.Ui.SafeArea
{
	public class SafeAreaPanel : MonoBehaviour
	{
		private RectTransform rectTransform;

		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
			rectTransform.ResetTransforms();
			rectTransform.SetAnchor(AnchorPresets.StretchAll);
			rectTransform.offsetMin = Statics.VECTOR2_ZERO;
			rectTransform.offsetMax = Statics.VECTOR2_ZERO;

			RefreshPanel(Screen.safeArea);
		}

		private void OnEnable()
		{
			SafeAreaDetection.OnSafeAreaChanged += RefreshPanel;
		}

		private void OnDisable()
		{
			SafeAreaDetection.OnSafeAreaChanged -= RefreshPanel;
		}

		private void RefreshPanel(Rect safeArea)
		{

			Vector2 anchorMin = safeArea.position;
			Vector2 anchorMax = safeArea.position + safeArea.size;

			anchorMin.x /= Screen.width;
			anchorMin.y /= Screen.height;
			anchorMax.x /= Screen.width;
			anchorMax.y /= Screen.height;

			rectTransform.anchorMin = anchorMin;
			rectTransform.anchorMax = anchorMax;
		}
	}
}
