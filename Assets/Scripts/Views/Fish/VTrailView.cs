using DG.Tweening;
using UnityEngine;
using Assets.Scripts.Framework.Tools;
using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.Views.Fish
{
	public class VTrailView : MonoBehaviour
	{
		public bool IsActive { get; private set; }

		private RDAnimator animator;
		private Sprite[] sprites = new Sprite[20];
		private SpriteRenderer spriteRenderer;

		private void Awake()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
			spriteRenderer.color = new Color(1f, 1f, 1f, 0f);

			for (int i = 0; i < sprites.Length; i++)
				sprites[i] = AssetLoader.ME.Load<Sprite>("Sprites/VTrail/VTrail" + i);

			animator = new RDAnimator(sprites.Length, OnFrameUpdate);
			animator.Loop = Framework.Tools.LoopType.Normal;
		}

		private void OnFrameUpdate(int index) => spriteRenderer.sprite = sprites[index];

		public void Show()
		{
			if (IsActive)
				return;

			IsActive = true;
			animator.Play();
			spriteRenderer.DOKill();
			spriteRenderer.DOFade(1, 0.2f);
		}

		public void Hide()
		{
			if (!IsActive)
				return;

			IsActive = false;
			spriteRenderer.DOKill();
			spriteRenderer.DOFade(0, 0.2f).onComplete = animator.Stop;
		}

	}
}
