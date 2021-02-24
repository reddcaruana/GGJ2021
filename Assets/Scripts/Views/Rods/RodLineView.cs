using UnityEngine;
using System;
using DG.Tweening;
using Assets.Scripts.Rods;
using Assets.Scripts.Framework;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Framework.Tools;

namespace Assets.Scripts.Views.Rods
{
	public class RodLineView : MonoBehaviour
	{
		private Action onLineSnapComplete;
		private RDAnimator animator;
		private readonly Sprite[] lineSnapSprites = new Sprite[13];

		private Transform transformGrp;
		private Vector3 transformGrpRotation = new Vector3();
		private Vector3 transformGrpScale = Vector3.one;

		private RodLineProgressBarView progressBarView;
		private SpriteRenderer rodSideSnapSpriteRenderer;
		private SpriteRenderer fishSideSnapSpriteRenderer;

		public void Awake()
		{
			for (int i = 0; i < lineSnapSprites.Length; i++)
				lineSnapSprites[i] = AssetLoader.ME.Loader<Sprite>("Sprites/RodLine/Xlief" + i);

			animator = new RDAnimator(lineSnapSprites.Length, OnFrameUpdate);
			animator.RegisterToNoLoopAnimationCompletion(OnSanpComplete);

			transformGrp = transform.Find("Transform_grp");
			
			progressBarView = transformGrp.Find("SpriteProgressBarBg").gameObject.AddComponent<RodLineProgressBarView>();
			progressBarView.Set(isVertical: false);
			progressBarView.ProgressUpdate(0);

			rodSideSnapSpriteRenderer = transform.Find("SpriteRodSide").GetComponent<SpriteRenderer>();
			fishSideSnapSpriteRenderer = transform.Find("SpriteFishSide").GetComponent<SpriteRenderer>();
		}

		public void PositionUpdate(Vector3 startWorldPosition, Vector3 endWorldPosition)
		{
			rodSideSnapSpriteRenderer.transform.position = startWorldPosition;
			transformGrpRotation.z = MathUtils.AimAtTarget2D(endWorldPosition, startWorldPosition) + 90f;
			Quaternion rotation = Quaternion.Euler(transformGrpRotation);
			rodSideSnapSpriteRenderer.transform.rotation = rotation;

			fishSideSnapSpriteRenderer.transform.position = endWorldPosition;
			transformGrpRotation.z = MathUtils.AimAtTarget2D(startWorldPosition, endWorldPosition) + 90f;
			rotation = Quaternion.Euler(transformGrpRotation);
			fishSideSnapSpriteRenderer.transform.rotation = rotation;

			transformGrp.position = startWorldPosition;

			transformGrp.rotation = rotation;

			transformGrpScale.y = Vector2.Distance(startWorldPosition, endWorldPosition) / progressBarView.GetBounds().y;
			transformGrp.localScale = transformGrpScale;
		}

		public void ProgressBarUpdate(float value) => progressBarView.ProgressUpdate(value);

		public void LineSnap(Action onComplete)
		{
			if (animator.State == Framework.Tools.AnimationState.Play)
				return;

			rodSideSnapSpriteRenderer.color = Statics.COLOR_WHITE;
			fishSideSnapSpriteRenderer.color = Statics.COLOR_WHITE;

			transformGrp.gameObject.SetActive(false);

			onLineSnapComplete = onComplete;
			animator.Play();
		}

		private void OnSanpComplete()
		{
			const float DURATION = 0.2f;
			rodSideSnapSpriteRenderer.DOFade(0, DURATION);
			fishSideSnapSpriteRenderer.DOFade(0, DURATION)
				.onComplete = OnComplete;

			void OnComplete() => onLineSnapComplete?.Invoke();
		}

		private void OnFrameUpdate(int index)
		{
			rodSideSnapSpriteRenderer.sprite = lineSnapSprites[index];
			fishSideSnapSpriteRenderer.sprite = lineSnapSprites[index];
		}

		public void Reset() => transformGrp.gameObject.SetActive(true);

	}
}
