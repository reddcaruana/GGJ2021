using System;
using UnityEngine;
using DG.Tweening;
using Assets.Scripts.Framework;
using Assets.Scripts.AquaticCreatures.Fish;

namespace Assets.Scripts.Views.Fish
{
	public class FishView : MonoBehaviour
	{
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
			spriteRenderer.transform.localScale = Statics.VECTOR2_ONE * (controller.Size / spriteRenderer.bounds.size.y);
		}

		public void Reposition(Vector3 localPosition, float proximityFear, float proximityBite)
		{
			transform.localPosition = localPosition;
			startPos.y += proximityBite / 2f;
			endPos.y -= proximityFear / 2f;
		}

		public Vector3 BoundSize() => spriteRenderer.bounds.size;

		public void Appear()
		{
			AppearInternal(() => isIdle = true);
		}

		private void AppearInternal(Action onComplete)
		{
			const float duration = 2.5f;

			spriteRenderer.transform.localPosition = startPos;
			spriteRenderer.transform.DOLocalMoveY(endPos.y, duration)
				.SetEase(Ease.InOutSine)
				.OnComplete(() => onComplete());

			spriteRenderer.color = Statics.COLOR_ZERO;
			spriteRenderer.DOColor(Statics.COLOR_BLACK, duration)
				.SetEase(Ease.InOutSine);
		}

		private void Idle()
		{
			if (!isIdle)
				return;

			spriteRenderer.transform.RotateAround(transform.position, Vector3.forward, 20 * Time.deltaTime);
		}

		public void Escape()
		{
			throw new NotImplementedException();
		}

		private int lastDebugLayer = -1;
		public void CreateDebugAreaView(float size, Color color, string name)
		{
			SpriteRenderer debugArea = new GameObject(name).AddComponent<SpriteRenderer>();
			debugArea.transform.SetParent(transform);
			debugArea.sprite = spriteRenderer.sprite;
			debugArea.color = color;
			debugArea.sortingOrder = lastDebugLayer--;
			debugArea.transform.localScale = Statics.VECTOR2_ONE *  (size / debugArea.bounds.size.y);
		}
	}
}
