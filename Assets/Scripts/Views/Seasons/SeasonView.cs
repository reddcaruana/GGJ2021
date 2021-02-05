using UnityEngine;

namespace Assets.Scripts.Views.Seasons
{
	public class SeasonView : MonoBehaviour
	{
		private SpriteRenderer spriteRenderer;

		private void Awake()
		{
			spriteRenderer = transform.Find("SpriteSeason").GetComponent<SpriteRenderer>();
		}

		public void Set()
		{
			float height = Camera.main.orthographicSize * 2.0f;
			float width = height * Screen.width / Screen.height;
			spriteRenderer.drawMode = SpriteDrawMode.Sliced;
			spriteRenderer.size = new Vector2(width, height);
		}

		public Vector3 BoundSize() => spriteRenderer.bounds.size;
	}
}
