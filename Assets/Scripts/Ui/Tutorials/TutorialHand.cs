using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts.Rods.Tilt;
using Assets.Scripts.Controllers;
using Assets.Scripts.Framework.Tools;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Framework.AssetsManagers;
using LoopType = Assets.Scripts.Framework.Tools.LoopType;

namespace Assets.Scripts.Ui.Tutorials
{
	public class TutorialHand
	{
		private static readonly float rightPositionX = ViewController.Area.Width / 3f;
		private static Vector3 worldPos = new Vector3()
		{
			x = rightPositionX,
			y = ViewController.ScreenBottom.y + ((ViewController.Area.Height * RodTiltHandlerTouch.HEIGHT_PERCENTAGE) / 2f)
		};

		private readonly Sprite[] Sprites = new Sprite[2];
		private Image handImage;
		private RDAnimator animator;

		public bool IsActive => handImage.gameObject.activeSelf;
		private bool isDrag;

		public void Init()
		{
			Sprites[0] = AssetLoader.ME.Load<Sprite>("Sprites/Tutorial/Tap1");
			Sprites[1] = AssetLoader.ME.Load<Sprite>("Sprites/Tutorial/Tap2");

			animator = new RDAnimator(Sprites.Length, OnFrameUpdate);
			animator.Loop = LoopType.Normal;
			animator.FPS = 2;

			handImage = new GameObject("HandTutorial", typeof(RectTransform)).AddComponent<Image>();
			handImage.transform.SetParent(ViewController.UiController.Canvas.transform);
			handImage.transform.ResetTransforms();
			handImage.raycastTarget = false;
			handImage.maskable = false;
			handImage.rectTransform.pivot = new Vector2(0.25f, 0.25f);
			handImage.rectTransform.sizeDelta = new Vector2(200, 200);
			handImage.sprite = Sprites[0];
			handImage.gameObject.SetActive(false);
		}

		public void AnchoredPosition(Vector2 position) => handImage.rectTransform.anchoredPosition = position;
		public void WorldPosition(Vector3 position) => handImage.rectTransform.position = position;

		public void ClickHold() => Play(LoopType.None);

		public void ClickRepeat() => Play(LoopType.Normal);

		public void ClickFastRepeat() => Play(LoopType.Normal, 12);

		public void DragRepeat(Vector2 startWorldPos, Vector2 endWorldPos)
		{
			if (isDrag)
				return;
			CoroutineRunner.RunCoroutine(DragRepeatCoroutine(startWorldPos, endWorldPos));
		}

		private IEnumerator DragRepeatCoroutine(Vector2 startWorldPos, Vector2 endWorldPos)
		{
			const float DRAG_DURATION = 1f;
			isDrag = true;
			while (isDrag)
			{
				WorldPosition(startWorldPos);
				ClickHold();
				yield return new WaitForSeconds(1.2f);
				handImage.rectTransform.DOMove(endWorldPos, DRAG_DURATION);
				yield return new WaitForSeconds(DRAG_DURATION);
				animator.Stop();

				yield return null;
			}
		}

		private void Play(LoopType loop, int fps = 2)
		{
			if (animator.State == Framework.Tools.AnimationState.Play)
				return;

			handImage.gameObject.SetActive(true);
			animator.FPS = fps;
			animator.Loop = loop;
			animator.Play();
		}

		public void Stop()
		{
			isDrag = false;
			handImage.gameObject.SetActive(false);
			animator.Stop();
		}

		private void OnFrameUpdate(int frame) => handImage.sprite = Sprites[frame];

		public void PositionOnPullLeft()
		{
			worldPos.x = -rightPositionX;
			WorldPosition(worldPos);
		}

		public void PositionOnPullRight()
		{
			worldPos.x = rightPositionX;
			WorldPosition(worldPos);
		}
	}
}
