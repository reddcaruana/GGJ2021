using System;
using UnityEngine;
using DG.Tweening;
using Assets.Scripts.Framework;
using Assets.Scripts.Controllers;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Framework.Tools;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Framework.AssetsManagers;
using LoopType = Assets.Scripts.Framework.Tools.LoopType;

namespace Assets.Scripts.Views.Fish
{
	public class FishView : MonoBehaviour
	{
		public const float AHEAD = 500f;
		private const int SWIMMING_FPS = 8;
		private const int STATIC_FPS = (int)(SWIMMING_FPS * 0.3f);
		private const int ESCAPE_FPS = SWIMMING_FPS * 4;

		private static readonly Sprite[] FishSilhouetteSprites = new Sprite[8];
		public bool IsManualOverride { get; private set; }

		private SpriteRenderer spriteRenderer;
		private SpriteRenderer shadowSpriteRenderer;
		private float shadowAlphaMax;
		private Vector3 startPos = new Vector3();
		private Vector3 endPos = new Vector3();
		private bool isIdle;

		private RDAnimator animator;


		private void Awake()
		{
			if (FishSilhouetteSprites[0] == null)
			{
				for (int i = 0; i < FishSilhouetteSprites.Length; i++)
					FishSilhouetteSprites[i] = AssetLoader.ME.Load<Sprite>($"Sprites/FishSilhouette/fishSilhouette{i + 1}");
			}

			animator = new RDAnimator(FishSilhouetteSprites.Length, OnFrameUpdate);
			animator.Loop = LoopType.PingPong;

			spriteRenderer = transform.Find("SpriteFish").GetComponent<SpriteRenderer>();
			shadowSpriteRenderer = spriteRenderer.transform.Find("SpriteFishShadow").GetComponent<SpriteRenderer>();
			shadowAlphaMax = shadowSpriteRenderer.color.a;
			SpriteChange(0);
			DoFade(0, 0);
			GameController.ME.RegisterToUpdate(OnUpdate);
		}

		private void OnUpdate()
		{
			Idle();
		}

		public void Set(FishController controller)
		{
			name = $"Fish{controller.Data.Type.Alias}";
			// Change Image here
			spriteRenderer.transform.localScale = Statics.VECTOR3_ONE; // resets Bounds
			spriteRenderer.transform.localScale = Statics.VECTOR3_ONE * (controller.Size / spriteRenderer.bounds.size.y);
		}

		private void SpriteChange(int index) =>
			spriteRenderer.sprite = shadowSpriteRenderer.sprite = FishSilhouetteSprites[index];

		private void ColorChnage(Color color)
		{
			spriteRenderer.color = color;
			color.a = shadowAlphaMax;
			shadowSpriteRenderer.color = color;
		}

		private void DoFade(float value, float duration)
		{
			spriteRenderer.DOFade(value, duration).SetEase(Ease.InOutSine);
			shadowSpriteRenderer.DOFade(value * shadowAlphaMax, duration).SetEase(Ease.InOutSine);
		}

		public void Reposition(Vector3 localPosition, float proximityFear, float proximityBite)
		{
			transform.localPosition = localPosition;
			SetStartAndEnd(proximityFear, proximityBite);
		}

		public void SetStartAndEnd(float proximityFear, float proximityBite)
		{
			startPos.y = proximityBite / 2f;
			endPos.y = -(proximityFear / 2f);
		}

		public Vector3 BoundSize() => spriteRenderer.bounds.size;

		public Vector3 GetFishWorldPosition() => spriteRenderer.transform.position;

		public void Appear(Action onComplete = null)
		{
			AppearInternal(onComplete);
		}

		private void AppearInternal(Action onComplete = null)
		{
			const float duration = 2.5f;

			isIdle = true;
			IsManualOverride = false;

			PlaySwiAnimation();
			spriteRenderer.transform.localPosition = startPos;
			spriteRenderer.transform.DOLocalMoveY(endPos.y, duration)
				.SetEase(Ease.OutSine)
				.OnComplete(() => onComplete?.Invoke());

			Face(endPos, duration / 2f);
			DoFade(1, duration);
		}

		private void Idle()
		{
			if (!isIdle)
				return;

			spriteRenderer.transform.RotateAround(transform.position, Vector3.forward, -20 * Time.deltaTime);
		}

		public void Escape(Action onComplete = null)
		{
			isIdle = false;
			IsManualOverride = false;

			spriteRenderer.DOKill();
			PlayEscapeAimation();

			Vector3 target = spriteRenderer.transform.localPosition;
			float duration = Vector3.Distance(target, startPos) / Vector3.Distance(startPos, endPos);

			target.y = startPos.y;
			Face(target, duration / 2f);

			spriteRenderer.transform.DOLocalMoveY(startPos.y, duration)
				.SetEase(Ease.InOutSine)
				.onComplete = OnComplete;

			DoFade(0, duration);

			void OnComplete()
			{
				animator.Stop();
				onComplete?.Invoke();
			}
		}


		public void ApprochFloat(Vector3 localPosition, Action onComplete = null)
		{
			const float duration = 1f;

			isIdle = false;
			IsManualOverride = false;

			localPosition = localPosition - transform.localPosition;
			Face(localPosition, duration / 2f);
			spriteRenderer.transform.DOLocalMove(localPosition, duration)
				.SetEase(Ease.InOutSine)
				.onComplete = OnComplete;

			void OnComplete()
			{
				PlayStaticAnimation();
				onComplete?.Invoke();
			}
		}

		public void Caught()
		{
			spriteRenderer.color = Statics.COLOR_ZERO;
		}

		private void FaceAHead(Vector2 localPosition, float duration, Action onComplete = null)
		{
			Vector2 spriteLocalPos = spriteRenderer.transform.localPosition;
			Vector3 aheadLocalPos = ((localPosition - spriteLocalPos) * AHEAD) + localPosition;
			Face(aheadLocalPos, duration, onComplete);
		}

		private void Face(Vector2 localPosition, float duration, Action onComplete = null)
		{
			Vector3 rotation = new Vector3();
			rotation.z = MathUtils.AimAtTarget2D(spriteRenderer.transform.localPosition, localPosition) + 90f;
			spriteRenderer.transform.DOLocalRotateQuaternion(Quaternion.Euler(rotation), duration)
				.SetEase(Ease.InOutSine)
				.onComplete = () => onComplete?.Invoke();
			
			DebugPositionTarget(localPosition);
		}

		public void SetManualOverride(bool value)
		{
			IsManualOverride = value;
			if (IsManualOverride)
				isIdle = false;
		}

		public void ManualOverride(Vector2 newWorldPos) => ManualOverride(newWorldPos, newWorldPos);

		public void ManualOverride(Vector2 newWorldPos, Vector3 lookWorldPosition, bool isFaceAHead = true)
		{
			if (!IsManualOverride)
				return;

			if (lookWorldPosition.y > spriteRenderer.transform.position.y)
			{
				if (isFaceAHead)
					FaceAHead(transform.InverseTransformPoint(lookWorldPosition), 0.1f);
				else
					Face(transform.InverseTransformPoint(lookWorldPosition), 0.1f);
			}

			spriteRenderer.transform.position = newWorldPos;
		}

		public void PlaySwiAnimation() => PlayAnimation(SWIMMING_FPS);
		public void PlayStaticAnimation() => PlayAnimation(STATIC_FPS);
		public void PlayEscapeAimation() => PlayAnimation(ESCAPE_FPS);

		private void PlayAnimation(int fps)
		{
			animator.FPS = fps;
			animator.Play();
		}

		private void OnFrameUpdate(int frame) => SpriteChange(frame);

		Transform debugTarget;

		[System.Diagnostics.Conditional("RDEBUG")]
		void DebugPositionTarget(Vector3 localPos)
		{
			if (debugTarget == null)
			{
				debugTarget = new GameObject("DebugTarget" + gameObject.name).transform;
				debugTarget.SetParent(spriteRenderer.transform.parent);
				debugTarget.ResetTransforms();
				SpriteRenderer renderer = debugTarget.gameObject.AddComponent<SpriteRenderer>();
				renderer.sprite = AssetLoader.ME.Load<Sprite>("Sprites/Circle");
				renderer.color = Color.red;
				renderer.sortingOrder = 100;
			}

			debugTarget.localPosition = localPos;
		}
	}
}
