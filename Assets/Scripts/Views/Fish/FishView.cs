using Assets.Scripts.Fish;
using UnityEngine;

namespace Assets.Scripts.Views.Fish
{
	public class FishView : MonoBehaviour
	{
		private SpriteRenderer spriteRenderer;

		private void Awake()
		{
			spriteRenderer = transform.Find("SpriteFish").GetComponent<SpriteRenderer>();
		}

		public void Set(FishController controller)
		{
			// Change Image here
			spriteRenderer.transform.localScale = Vector3.one * controller.Size;
		}

		public Vector3 BoundSize() => spriteRenderer.bounds.size;
	}
}
