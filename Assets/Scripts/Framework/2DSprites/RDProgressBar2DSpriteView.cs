using UnityEngine;

namespace Assets.Scripts.Framework._2DSprites
{
	public abstract class RDProgressBar2DSpriteView : MonoBehaviour
	{
		protected SpriteRenderer ProgressSpriteRenderer { get; private set; }

		protected int ScaleAxis { get; private set; }
		protected Vector3 currentScale;
		private float originalScale;
		private Vector2 initialBounds;

		protected virtual void Awake()
		{
			ProgressSpriteRenderer = transform.Find(GetImagePath()).GetComponent<SpriteRenderer>();
		}

		public void Set(bool isVertical)
		{
			ScaleAxis = isVertical ? 1 : 0;
			currentScale = ProgressSpriteRenderer.transform.localScale;
			originalScale = isVertical ? currentScale.y : currentScale.x;
			initialBounds = GetComponent<SpriteRenderer>().bounds.size;
			SetInternal();
		}

		protected abstract void SetInternal();

		protected abstract string GetImagePath();

		public void ProgressUpdate(float value)
		{
			currentScale[ScaleAxis] = originalScale * value;
			ProgressSpriteRenderer.transform.localScale = currentScale;
		}

		public Vector2 GetBounds() => initialBounds;
	}
}
