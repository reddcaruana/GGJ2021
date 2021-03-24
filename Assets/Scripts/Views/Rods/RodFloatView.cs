using System;
using DG.Tweening;
using UnityEngine;
using Assets.Scripts.Framework;
using Assets.Scripts.Framework.Tools;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Framework.AssetsManagers;
using LoopType = Assets.Scripts.Framework.Tools.LoopType;

namespace Assets.Scripts.Views.Rods
{
	public class RodFloatView : MonoBehaviour
	{
		private enum AnimationType { Float, Nibble, Splash}
		private const int FLOATING_FPS = 10;
		private const int NIBBLING_FPS = 5;
		private const int SPLASH_FPS = 8;

		private readonly Vector3 StartScale = Vector3.one * 2f;
		private Vector3 PeakScale;
		private Vector3 FloatingScale;

		private SpriteRenderer spriteRenderer;
		private RDAnimator animator;
		private Sprite[] floatingSprites = new Sprite[34];
		private Sprite[] nibbleSprites = new Sprite[17];
		private Sprite[] splashSprites = new Sprite[11];
		private AnimationType animationType;

		private void Awake()
		{
			PeakScale = StartScale * 2f;
			FloatingScale = StartScale * 0.7f;

			spriteRenderer = transform.Find("SpriteRodFloat").GetComponent<SpriteRenderer>();

			for (int i = 0; i < floatingSprites.Length; i++)
				floatingSprites[i] = AssetLoader.ME.Load<Sprite>($"Sprites/RodFloat/Floating/floating{i + 1}");

			for (int i = 0; i < nibbleSprites.Length; i++)
				nibbleSprites[i] = AssetLoader.ME.Load<Sprite>($"Sprites/RodFloat/Nibble/floatnibble{i + 1}");

			for (int i = 0; i < splashSprites.Length; i++)
				splashSprites[i] = AssetLoader.ME.Load<Sprite>($"Sprites/RodFloat/Splash/splash{i + 1}");

			animator = new RDAnimator(floatingSprites.Length, OnFrameUpdate);
			spriteRenderer.sprite = floatingSprites[0];
		}

		public void Cast(Vector3 targetWorldPosition, float duration, Action onComplete)
		{
			const float springPercentage = 0.15f;
			float springDuration = duration * springPercentage;
			float haldDuration = (duration * (1 - springPercentage)) / 2f;
			transform.localScale = StartScale;
			transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 10f));
			transform.DOScale(PeakScale, haldDuration).SetEase(Ease.OutSine).onComplete = () => 
			{
				transform.DOScale(FloatingScale * 0.5f, haldDuration).SetEase(Ease.InSine).onComplete = () => 
				{
					transform.DOScale(FloatingScale, springDuration).SetEase(Ease.InOutSine);
				};
			};

			transform.DOLocalRotateQuaternion(Quaternion.identity, duration).SetEase(Ease.OutSine);
			transform.DOMove(targetWorldPosition, duration).onComplete = OnComplete;

			void OnComplete()
			{
				Float();
				onComplete?.Invoke();
			}
		}

		public void Nibble(float duration) => Animate(AnimationType.Nibble, nibbleSprites.Length, (int)(NIBBLING_FPS * duration));

		public void Float() => Animate(AnimationType.Float, floatingSprites.Length, FLOATING_FPS);

		private void HalfNibble(float duration) => Animate(AnimationType.Nibble, nibbleSprites.Length / 2, (int)(NIBBLING_FPS * duration), isLoop: false);

		private void Splash() => Animate(AnimationType.Splash, splashSprites.Length, SPLASH_FPS, isLoop: false);

		private void Animate(AnimationType type, int frames, int fps, bool isLoop = true)
		{
			animator.Stop();
			animationType = type;
			animator.Frames = frames;
			animator.FPS = fps;
			animator.Loop = isLoop ? LoopType.Normal : LoopType.None;
			animator.Play();
		}

		public void SinkAndDisapear(float duration)
		{
			HalfNibble(duration);
			spriteRenderer.DOFade(0, duration).onComplete = OnComplete;

			void OnComplete()
			{
				Splash();
				spriteRenderer.color = Statics.COLOR_WHITE;
				CoroutineRunner.WaitUntil(() => animator.State == Framework.Tools.AnimationState.Stop, () => 
				{
					spriteRenderer.DOFade(0, 0.1f);
				});
			}
		}

		public void Reset(Vector3 targetWorldPosition, float duration, Action onComplete = null)
		{
			animator.Stop();
			spriteRenderer.sprite = floatingSprites[0];
			spriteRenderer.color = Statics.COLOR_ZERO;
			spriteRenderer.DOColor(Statics.COLOR_WHITE, duration * 0.2f);

			transform.DOScale(StartScale, duration).SetEase(Ease.InOutSine);
			transform.DOMove(targetWorldPosition, duration).SetEase(Ease.InOutSine).onComplete = OnComplete;

			void OnComplete() => onComplete?.Invoke();
		}

		private void OnFrameUpdate(int frameIndex)
		{
			spriteRenderer.sprite =
				animationType == AnimationType.Float ? floatingSprites[frameIndex] :
				animationType == AnimationType.Nibble ? nibbleSprites[frameIndex] : splashSprites[frameIndex];
		}

	}
}
