using UnityEngine;
using DG.Tweening;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Views.Rods
{
	public class RodBaseView : MonoBehaviour
	{
		private const float REEL_PADDING = -0.6f;

		private Transform pivotTransform;
		private Transform lineStartTransform;
		private float lineStartLocalPositionX;
		private Vector3 lineStartCurrentLocalPosition;
		private SpriteRenderer spriteRenderer;
		private Vector3 currentRotation;

		private void Awake()
		{
			pivotTransform = new GameObject("Pivot").transform;
			pivotTransform.SetParent(transform);
			pivotTransform.ResetTransforms();

			lineStartTransform = new GameObject("LineStartPosition").transform;
			lineStartTransform.SetParent(pivotTransform);
			lineStartTransform.ResetTransforms();

			spriteRenderer = transform.Find("SpriteRod").GetComponent<SpriteRenderer>();
			spriteRenderer.transform.SetParent(pivotTransform);
		}

		public void Set(string typeStr, Vector3 lineStartLocalPosition)
		{
			lineStartLocalPositionX = lineStartLocalPosition.x;
			lineStartCurrentLocalPosition = lineStartLocalPosition;
			lineStartTransform.localPosition = lineStartLocalPosition;

			spriteRenderer.sprite = AssetLoader.ME.Loader<Sprite>($"Sprites/Rods/Rod{typeStr}");

			Vector3 localPos = new Vector3();
			localPos.y += (spriteRenderer.bounds.size.y / 2f) + REEL_PADDING;
			spriteRenderer.transform.localPosition = localPos;
		}

		public void SetPosition(Vector3 worldPosition) => transform.position = worldPosition;

		public void Rotate(float angle, float duration, bool flip = false)
		{
			spriteRenderer.flipX = flip;
			lineStartCurrentLocalPosition.x = flip ? -lineStartLocalPositionX : lineStartLocalPositionX;
			lineStartTransform.localPosition = lineStartCurrentLocalPosition;
			currentRotation.z = angle;
			pivotTransform.DOLocalRotateQuaternion(Quaternion.Euler(currentRotation), duration);
		}

		public Vector3 GetLineStartWorldPosition() => lineStartTransform.position;

	}
}
