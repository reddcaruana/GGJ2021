using UnityEngine;

namespace Assets.Scripts.Framework.Utils
{
	public enum AnchorPresets
	{
		TopLeft,
		TopCenter,
		TopRight,

		MiddleLeft,
		MiddleCenter,
		MiddleRight,

		BottomLeft,
		BottonCenter,
		BottomRight,
		BottomStretch,

		VertStretchLeft,
		VertStretchRight,
		VertStretchCenter,

		HorStretchTop,
		HorStretchMiddle,
		HorStretchBottom,

		StretchAll
	}

	public enum PivotPresets
	{
		TopLeft,
		TopCenter,
		TopRight,

		MiddleLeft,
		MiddleCenter,
		MiddleRight,

		BottomLeft,
		BottomCenter,
		BottomRight,
	}

	public static class RectTransformUtils
	{
		public static void SetAnchor(this RectTransform source, AnchorPresets allign, float offsetX = 0, float offsetY = 0)
		{
			source.anchoredPosition = new Vector3(offsetX, offsetY, 0);

			switch (allign)
			{
				case (AnchorPresets.TopLeft):
					{
						source.anchorMin = new Vector2(0, 1);
						source.anchorMax = new Vector2(0, 1);
						break;
					}
				case (AnchorPresets.TopCenter):
					{
						source.anchorMin = new Vector2(0.5f, 1);
						source.anchorMax = new Vector2(0.5f, 1);
						break;
					}
				case (AnchorPresets.TopRight):
					{
						source.anchorMin = new Vector2(1, 1);
						source.anchorMax = new Vector2(1, 1);
						break;
					}

				case (AnchorPresets.MiddleLeft):
					{
						source.anchorMin = new Vector2(0, 0.5f);
						source.anchorMax = new Vector2(0, 0.5f);
						break;
					}
				case (AnchorPresets.MiddleCenter):
					{
						source.anchorMin = new Vector2(0.5f, 0.5f);
						source.anchorMax = new Vector2(0.5f, 0.5f);
						break;
					}
				case (AnchorPresets.MiddleRight):
					{
						source.anchorMin = new Vector2(1, 0.5f);
						source.anchorMax = new Vector2(1, 0.5f);
						break;
					}

				case (AnchorPresets.BottomLeft):
					{
						source.anchorMin = new Vector2(0, 0);
						source.anchorMax = new Vector2(0, 0);
						break;
					}
				case (AnchorPresets.BottonCenter):
					{
						source.anchorMin = new Vector2(0.5f, 0);
						source.anchorMax = new Vector2(0.5f, 0);
						break;
					}
				case (AnchorPresets.BottomRight):
					{
						source.anchorMin = new Vector2(1, 0);
						source.anchorMax = new Vector2(1, 0);
						break;
					}

				case (AnchorPresets.HorStretchTop):
					{
						source.anchorMin = new Vector2(0, 1);
						source.anchorMax = new Vector2(1, 1);
						break;
					}
				case (AnchorPresets.HorStretchMiddle):
					{
						source.anchorMin = new Vector2(0, 0.5f);
						source.anchorMax = new Vector2(1, 0.5f);
						break;
					}
				case (AnchorPresets.HorStretchBottom):
					{
						source.anchorMin = new Vector2(0, 0);
						source.anchorMax = new Vector2(1, 0);
						break;
					}

				case (AnchorPresets.VertStretchLeft):
					{
						source.anchorMin = new Vector2(0, 0);
						source.anchorMax = new Vector2(0, 1);
						break;
					}
				case (AnchorPresets.VertStretchCenter):
					{
						source.anchorMin = new Vector2(0.5f, 0);
						source.anchorMax = new Vector2(0.5f, 1);
						break;
					}
				case (AnchorPresets.VertStretchRight):
					{
						source.anchorMin = new Vector2(1, 0);
						source.anchorMax = new Vector2(1, 1);
						break;
					}

				case (AnchorPresets.StretchAll):
					{
						source.anchorMin = new Vector2(0, 0);
						source.anchorMax = new Vector2(1, 1);
						break;
					}
			}
		}

		public static void SetPivot(this RectTransform source, PivotPresets preset)
		{
			switch (preset)
			{
				case (PivotPresets.TopLeft):
					{
						source.pivot = new Vector2(0, 1);
						break;
					}
				case (PivotPresets.TopCenter):
					{
						source.pivot = new Vector2(0.5f, 1);
						break;
					}
				case (PivotPresets.TopRight):
					{
						source.pivot = new Vector2(1, 1);
						break;
					}

				case (PivotPresets.MiddleLeft):
					{
						source.pivot = new Vector2(0, 0.5f);
						break;
					}
				case (PivotPresets.MiddleCenter):
					{
						source.pivot = new Vector2(0.5f, 0.5f);
						break;
					}
				case (PivotPresets.MiddleRight):
					{
						source.pivot = new Vector2(1, 0.5f);
						break;
					}

				case (PivotPresets.BottomLeft):
					{
						source.pivot = new Vector2(0, 0);
						break;
					}
				case (PivotPresets.BottomCenter):
					{
						source.pivot = new Vector2(0.5f, 0);
						break;
					}
				case (PivotPresets.BottomRight):
					{
						source.pivot = new Vector2(1, 0);
						break;
					}
			}
		}

		public static void SetParent(RectTransform child, Transform parent, bool resetPosition = true)
		{
			child.SetParent(parent);
			if (resetPosition)
				child.anchoredPosition3D = Statics.VECTOR2_ZERO;
			ResetScaleAndZ(child);
		}

		public static void ResetTransforms(this RectTransform source)
		{
			source.ResetAnchoredPos();
			source.ResetRotattion();
			source.ResetScale();
		}

		public static void ResetAnchoredPos(this RectTransform source) => source.anchoredPosition3D = Statics.VECTOR3_ZERO;

		public static void ResetRotattion(this RectTransform source) => source.localRotation = Quaternion.identity;

		public static void ResetScale(this RectTransform source) => source.localScale = Statics.VECTOR3_ONE;

		public static void ResetScaleAndZ(RectTransform rect)
		{
			rect.anchoredPosition3D = new Vector3(
				rect.anchoredPosition3D.x,
				rect.anchoredPosition3D.y,
				0);
			rect.localScale = Statics.VECTOR3_ONE;
		}

		public static bool OverlapRectTransform(RectTransform rectTrans1, RectTransform rectTrans2)
		{
			var rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
			var rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

			return OverlapRect(rect1, rect2);
		}

		public static bool OverlapRect(Rect rect1, Rect rect2)
		{
			return rect1.Overlaps(rect2);
		}

		public static void MimicTransforms(this RectTransform rect, RectTransform toCopy)
		{
			rect.localScale = toCopy.localScale;
			rect.position = toCopy.position;
			rect.rotation = toCopy.rotation;
		}
	}
}
