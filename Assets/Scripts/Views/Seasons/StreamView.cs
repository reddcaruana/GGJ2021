using UnityEngine;
using Assets.Scripts.Framework.Tools;
using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.Views.Seasons
{
	public class StreamView : MonoBehaviour
	{
		private readonly static Sprite[] StreamSprites = new Sprite[129];

		private SpriteRenderer spriteRenderer;
		private SpriteRenderer shadowSpriteRenderer;
		private RDAnimator animator;

		private void Awake()
		{
			if (StreamSprites[0] == null)
			{
				for (int i = 0; i < StreamSprites.Length; i++)
					StreamSprites[i] = AssetLoader.ME.Load<Sprite>("Sprites/Stream/stream" + (i + 1));
			}

			spriteRenderer = transform.Find("SpriteStream").GetComponent<SpriteRenderer>();
			shadowSpriteRenderer = spriteRenderer.transform.Find("SpriteStreamShadow").GetComponent<SpriteRenderer>();
			ChangeSprite(0);
			animator = new RDAnimator(StreamSprites.Length, OnFrameUpdate);
			animator.Loop = LoopType.Normal;
		}

		private void ChangeSprite(int index) =>
			spriteRenderer.sprite = shadowSpriteRenderer.sprite = StreamSprites[index];

		public void PlayAnimation() => animator.Play();

		public void StopAnimation() => animator.Stop();

		private void OnFrameUpdate(int frameIndex) => ChangeSprite(frameIndex);

		public Vector2 Bounds() => spriteRenderer.bounds.size;
	}
}
