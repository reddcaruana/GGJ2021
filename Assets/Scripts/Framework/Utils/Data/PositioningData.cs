
using UnityEngine;

namespace Assets.Scripts.Framework.Utils.Data
{
	[System.Serializable]
	public struct PositioningData
	{
		[SerializeField] Vector3 anchoredPos3D;
		[SerializeField] Vector3 position;
		[SerializeField] Vector2 sizeDelta;
		[SerializeField] Quaternion localRotation;
		[SerializeField] Quaternion rotation;
		[SerializeField] Vector3 scale;

		public Vector3 AnchoredPos3D => anchoredPos3D;
		public Vector3 Position => position;
		public Vector2 SizeDelta => sizeDelta;
		public Quaternion LocalRotation => localRotation;
		public Quaternion Rotation => rotation;
		public Vector3 Scale => scale;

		public PositioningData(RectTransform rectTransform) :
			this
			(
				rectTransform.anchoredPosition3D,
				rectTransform.position,
				rectTransform.sizeDelta,
				rectTransform.localRotation,
				rectTransform.rotation,
				rectTransform.localScale
			)
		{
		}

		public PositioningData
			(
				Vector3 anchoredPos3D,
				Vector3 position,
				Vector2 sizeDelta,
				Quaternion localRotation,
				Quaternion rotation,
				Vector3 scale
			)
		{
			this.anchoredPos3D = anchoredPos3D;
			this.position = position;
			this.sizeDelta = sizeDelta;
			this.localRotation = localRotation;
			this.rotation = rotation;
			this.scale = scale;
		}

		public void Transfer(RectTransform rectTransform, bool isWorld = false)
		{
			if (isWorld)
			{
				rectTransform.position = position;
				rectTransform.rotation = rotation;
			}
			else
			{
				rectTransform.anchoredPosition3D = anchoredPos3D;
				rectTransform.localRotation = LocalRotation;
			}

			rectTransform.sizeDelta = SizeDelta;
			rectTransform.localScale = Scale;
		}
	}
}
