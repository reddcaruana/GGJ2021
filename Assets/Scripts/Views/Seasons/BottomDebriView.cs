using UnityEngine;

namespace Assets.Scripts.Views.Seasons
{
	public class BottomDebriView : MonoBehaviour
	{
		public bool IsSpawned { get; private set; }
		private SpriteRenderer spriteRenderer;

		private void Awake()
		{
			spriteRenderer = transform.Find("SpriteDebri").GetComponent<SpriteRenderer>();
			gameObject.SetActive(false);
		}

		public void SetSprite(Sprite sprite) => spriteRenderer.sprite = sprite;
		public void SetColor(Color color) => spriteRenderer.color = color;
	}
}
