using UnityEngine;
using Assets.Scripts.Framework;
using Assets.Scripts.Controllers;
using Assets.Scripts.Framework.Tools;
using Assets.Scripts.Seasons.Paywalls;
using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.Views.Seasons.Paywalls
{
	public class PaywallView : MonoBehaviour
	{
		private SpriteRenderer mainSprite;
		private Sprite[] mainSpriteFrames = new Sprite[2];
		private Sprite mainSpriteFinal;
		private RDAnimator animator;

		private PaywallRequiredView[] sprits;

		private void Awake()
		{
			mainSprite = transform.Find("SpriteMain").GetComponent<SpriteRenderer>();
			Transform grp = transform.Find("Required_grp");
			SpriteRenderer[] renderers = grp.GetComponentsInChildren<SpriteRenderer>();
			
			animator = new RDAnimator(mainSpriteFrames.Length, OnFrameUpdate);
			animator.FPS = 3f;
			animator.Loop = LoopType.Normal;

			sprits = new PaywallRequiredView[renderers.Length];
			for (int i = 0; i < renderers.Length; i++)
				sprits[i] = new PaywallRequiredView(renderers[i]);
		}

		public void Set(FishRequired[] requiredData, string seasonStr, Vector3[] fishRequiredLocalPosition)
		{
			int count = 0;
			for (int i = 0; i < requiredData.Length; i++)
			{
				for (int j = 0; j < requiredData[i].Required; j++)
				{
					sprits[count].Position(fishRequiredLocalPosition[count]);
					requiredData[i].AssignView(sprits[count]);
					sprits[count++].Enable();
				}
			}

			for (int i = count; i < sprits.Length; i++)
				sprits[i].Disable();

			for (int i = 0; i < mainSpriteFrames.Length; i++)
				mainSpriteFrames[i] = AssetLoader.ME.Load<Sprite>($"Sprites/Paywall/{seasonStr}/Paywall{seasonStr}{i}");

			mainSpriteFinal = AssetLoader.ME.Load<Sprite>($"Sprites/Paywall/{seasonStr}/Paywall{seasonStr}Final");

			mainSprite.sprite = mainSpriteFrames[0];
			transform.localScale = Statics.VECTOR3_ONE;
			transform.localScale = Statics.VECTOR3_ONE * (ViewController.Area.Width / mainSprite.bounds.size.x);
		}

		public void PlayAnimation(bool value)
		{
			if (value && animator.State == Framework.Tools.AnimationState.Stop)
				animator.Play();
			else if (!value && animator.State == Framework.Tools.AnimationState.Play)
				animator.Stop();
		}

		public void Finished()
		{
			mainSpriteFrames = new Sprite[2];

			mainSprite.sprite = mainSpriteFinal;
			for (int i = 0; i < sprits.Length; i++)
				sprits[i].Disable();
		}

		private void OnFrameUpdate(int index) =>
			mainSprite.sprite = mainSpriteFrames[index];
	}
}
