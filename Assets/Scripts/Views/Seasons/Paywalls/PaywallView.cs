using UnityEngine;
using Assets.Scripts.Utils;
using Assets.Scripts.Constants;
using Assets.Scripts.Framework;
using Assets.Scripts.Controllers;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.Framework.Tools;
using Assets.Scripts.Seasons.Paywalls;

namespace Assets.Scripts.Views.Seasons.Paywalls
{
	public class PaywallView : MonoBehaviour
	{
		private SpriteRenderer mainSprite;
		private Sprite[] mainSpriteFrames = new Sprite[2];
		private Sprite mainSpriteFinal;
		private RDAnimator animator;

		private string seasonStr;
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
			DebugUtils.Assert(fishRequiredLocalPosition.Length == requiredData.Length, 
				$"[PaywallView] Invalid Count => Data: {requiredData.Length} vs Positions: {fishRequiredLocalPosition.Length}");

			for (int i = 0, s = 0; i < requiredData.Length; i++)
			{
				for (int j = 0; j < requiredData[i].Required; j++)
				{
					sprits[s].Set(requiredData[i], fishRequiredLocalPosition[i]);
					sprits[s++].Enable();
				}
			}

			for (int i = requiredData.Length; i < sprits.Length; i++)
				sprits[i].Disable();

			this.seasonStr = seasonStr;

			for (int i = 0; i < mainSpriteFrames.Length; i++)
				mainSpriteFrames[i] = AssetLoader.ME.Loader<Sprite>($"Sprites/Paywall/{seasonStr}/Paywall{seasonStr}{i}");

			mainSpriteFinal = AssetLoader.ME.Loader<Sprite>($"Sprites/Paywall/{seasonStr}/Paywall{seasonStr}Final");

			mainSprite.sprite = mainSpriteFrames[0];
			transform.localScale = Statics.VECTOR3_ONE * (ViewController.Area.Width / mainSprite.size.x);
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
			mainSpriteFrames = null;

			mainSprite.sprite = mainSpriteFinal;
			for (int i = 0; i < sprits.Length; i++)
				sprits[i].Disable();
		}


		public void Refresh()
		{
			for (int i = 0; i < sprits.Length; i++)
				sprits[i].Refresh();
		}

		private void OnFrameUpdate(int index) =>
			mainSprite.sprite = mainSpriteFrames[index];
	}

	public class PaywallRequiredView
	{
		private const float ALPHA = 0.5f;
		private readonly SpriteRenderer SpriteRenderer;
		private FishRequired required;

		public PaywallRequiredView(SpriteRenderer spriteRenderer)
		{
			SpriteRenderer = spriteRenderer;
		}

		public void Set(FishRequired required, Vector3 localPosition)
		{
			this.required = required;
			SpriteRenderer.transform.localPosition = localPosition;
			SpriteRenderer.sprite = FishWiki.GetSprite(required.Type);
			Refresh();
		}

		public void Refresh()
		{
			Color color;
			if (required.IsComplete)
				color = Statics.COLOR_WHITE;
			else
			{
				color = Statics.COLOR_GRAY;
				color.a = ALPHA;
			}
			
			SpriteRenderer.color = color;
		}

		public void Enable() => 
			SpriteRenderer.gameObject.SetActive(true);

		public void Disable() => 
			SpriteRenderer.gameObject.SetActive(false);
	}
}
