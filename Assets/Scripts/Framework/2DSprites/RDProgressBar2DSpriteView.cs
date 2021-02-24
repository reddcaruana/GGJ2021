using UnityEngine;

namespace Assets.Scripts.Framework._2DSprites
{
	public abstract class RDProgressBar2DSpriteView : MonoBehaviour
	{
		private SpriteRenderer progressSpriteRenderer;

		private int scaleAxis;
		private float originalScale;
		private Vector3 currentScale;
		private Vector2 initialBounds;

		protected virtual void Awake()
		{
			progressSpriteRenderer = transform.Find(GetImagePath()).GetComponent<SpriteRenderer>();
		}

		public void Set(bool isVertical)
		{
			scaleAxis = isVertical ? 1 : 0;
			currentScale = progressSpriteRenderer.transform.localScale;
			originalScale = isVertical ? currentScale.y : currentScale.x;
			initialBounds = GetComponent<SpriteRenderer>().bounds.size;
		}

		protected abstract string GetImagePath();

		public void ProgressUpdate(float value)
		{
			currentScale[scaleAxis] = originalScale * value;
			progressSpriteRenderer.transform.localScale = currentScale;
		}

		public Vector2 GetBounds() => initialBounds;
	}
}
