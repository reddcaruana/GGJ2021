using UnityEngine;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.Framework.Utils;
using DG.Tweening;

namespace Assets.Scripts.Views.Rods
{
	public class RodBaseView : MonoBehaviour
	{
		private const float REEL_PADDING = -0.6f;

		private Transform pivotTransform;
		private SpriteRenderer spriteRenderer;
		private Vector3 currentRotation;

		private void Awake()
		{
			pivotTransform = new GameObject("Pivot").transform;
			pivotTransform.SetParent(transform);
			pivotTransform.ResetTransforms();

			spriteRenderer = transform.Find("SpriteRod").GetComponent<SpriteRenderer>();
			spriteRenderer.transform.SetParent(pivotTransform);
		}

		public void Set(string typeStr)
		{
			spriteRenderer.sprite = AssetLoader.ME.Loader<Sprite>($"Sprites/Rods/Rod{typeStr}");

			Vector3 localPos = new Vector3();
			localPos.y += (spriteRenderer.bounds.size.y / 2f) + REEL_PADDING;
			spriteRenderer.transform.localPosition = localPos;
		}

		public void SetPosition(Vector3 worldPosition) => transform.position = worldPosition;

		public void Rotate(float angle, float duration, bool flip = false)
		{
			spriteRenderer.flipX = flip;
			currentRotation.z = angle;
			pivotTransform.DOLocalRotateQuaternion(Quaternion.Euler(currentRotation), duration);
		}

	}
}
