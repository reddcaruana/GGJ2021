using UnityEngine;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Views.Seasons
{
	public class SeasonView : MonoBehaviour
	{
		public Transform FishTank { get; private set; }
		private SpriteRenderer spriteRenderer;
		// Padding

		private void Awake()
		{
			spriteRenderer = transform.Find("SpriteSeason").GetComponent<SpriteRenderer>();
			FishTank = new GameObject("FishTank").transform;
			FishTank.SetParent(transform);
			FishTank.ResetTransforms();
		}

		public void Set(Vector2 tankSize)
		{
			
			spriteRenderer.drawMode = SpriteDrawMode.Sliced;
			spriteRenderer.size = tankSize;
		}

		public Vector3 BoundSize() => spriteRenderer.bounds.size;
	}
}
