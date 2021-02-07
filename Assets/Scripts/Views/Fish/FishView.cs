using System;
using UnityEngine;
using DG.Tweening;
using Assets.Scripts.Framework;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.AquaticCreatures.Fish;

namespace Assets.Scripts.Views.Fish
{
	public class FishView : MonoBehaviour
	{
		public bool IsManualOverride { get; private set; }

		private static readonly Color SOLID_COLOR = Statics.COLOR_BLACK;
		private SpriteRenderer spriteRenderer;
		private Vector3 startPos = new Vector3();
		private Vector3 endPos = new Vector3();
		private bool isIdle;


		private void Awake()
		{
			spriteRenderer = transform.Find("SpriteFish").GetComponent<SpriteRenderer>();
			spriteRenderer.color = Statics.COLOR_ZERO;
		}

		private void Update()
		{
			Idle();
		}

		public void Set(FishController controller)
		{
			name = $"Fish{controller.Data.Type.NiceName}";
			// Change Image here
			spriteRenderer.transform.localScale = Statics.VECTOR3_ONE; // resets Bounds
			spriteRenderer.transform.localScale = Statics.VECTOR3_ONE * (controller.Size / spriteRenderer.bounds.size.y);
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

			Face(endPos, duration / 2f);

			spriteRenderer.transform.localPosition = startPos;
			spriteRenderer.transform.DOLocalMoveY(endPos.y, duration)
				.SetEase(Ease.InOutSine)
				.OnComplete(() => onComplete?.Invoke());

			spriteRenderer.color = Statics.COLOR_ZERO;
			spriteRenderer.DOColor(SOLID_COLOR, duration)
				.SetEase(Ease.InOutSine);
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

			Vector3 target = spriteRenderer.transform.localPosition;
			float duration = Vector3.Distance(target, startPos) / Vector3.Distance(startPos, endPos);

			target.y = startPos.y;
			Face(target, duration / 2f);

			spriteRenderer.transform.DOLocalMoveY(startPos.y, duration)
				.SetEase(Ease.InOutSine)
				.OnComplete(() => onComplete?.Invoke());

			spriteRenderer.color = SOLID_COLOR;
			spriteRenderer.DOColor(Statics.COLOR_ZERO, duration)
				.SetEase(Ease.InOutSine);
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
				.OnComplete(() => onComplete?.Invoke());
		}

		public void Caught()
		{
			spriteRenderer.color = Statics.COLOR_ZERO;
		}

		private void Face(Vector2 localPosition, float duration, Action onComplete = null)
		{
			Vector3 rotation = new Vector3();
			rotation.z = MathUtils.AimAtTarget2D(spriteRenderer.transform.localPosition, localPosition) + 90f;
			spriteRenderer.transform.DOLocalRotateQuaternion(Quaternion.Euler(rotation), duration)
				.SetEase(Ease.InOutSine)
				.onComplete = () => onComplete?.Invoke();
		}

		public void SetManualOverride(bool value)
		{
			IsManualOverride = value;
			if (IsManualOverride)
				isIdle = false;
		}

		public void ManualOverride(Vector2 newWorldPos)
		{
			if (!IsManualOverride)
				return;

			if (newWorldPos.y > spriteRenderer.transform.position.y)
				Face(transform.InverseTransformPoint(newWorldPos), 0.1f);
			spriteRenderer.transform.position = newWorldPos;
		}
	}
}
