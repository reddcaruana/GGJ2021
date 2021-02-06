using UnityEngine;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Views.Rods
{
	public class RodBaseView : MonoBehaviour
	{
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

			Vector3 localPos = new Vector3();
			localPos.y += spriteRenderer.bounds.size.y / 2f;
			spriteRenderer.transform.localPosition = localPos;
		}

		public void SetPosition(Vector3 worldPosition) => transform.position = worldPosition;

		public void Rotate(float angle)
		{
			currentRotation.z = angle;
			pivotTransform.localRotation = Quaternion.Euler(currentRotation);
		}
	}
}
