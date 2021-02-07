using UnityEngine;
using Assets.Scripts.Paywalls;
using Assets.Scripts.Constants;
using Assets.Scripts.Framework;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.Framework.Tools;

namespace Assets.Scripts.Views
{
	public class PaywallView : MonoBehaviour
	{
		private SpriteRenderer mainSprite;
		private Sprite[] mainSpriteFrames = new Sprite[2];
		private Sprite mainSpriteFinal;
		private RAnimator animator;

		private string seasonStr;
		private PaywallRequiredView[] spritrs;

		private void Awake()
		{
			mainSprite = transform.Find("SpriteMain").GetComponent<SpriteRenderer>();
			Transform grp = transform.Find("Required_grp");
			SpriteRenderer[] renderers = grp.GetComponentsInChildren<SpriteRenderer>();
			
			animator = new RAnimator(mainSpriteFrames.Length, OnFrameUpdate);
			animator.FPS = 6f;
			animator.IsLoop = true;

			spritrs = new PaywallRequiredView[renderers.Length];
			for (int i = 0; i < renderers.Length; i++)
				spritrs[i] = new PaywallRequiredView(renderers[i]);
		}

		public void Set(FishRequired[] requiredData, string seasonStr)
		{
			int s = 0;
			for (int i = 0; i < requiredData.Length; i++)
			{
				for (int j = 0; j < requiredData[i].Required; j++)
					spritrs[s++].Set(requiredData[i]);
			}

			this.seasonStr = seasonStr;

			for (int i = 0; i < mainSpriteFrames.Length; i++)
				mainSpriteFrames[i] = AssetLoader.ME.Loader<Sprite>($"Sprites/Paywall/Paywall{seasonStr}{i}");

			mainSpriteFinal = AssetLoader.ME.Loader<Sprite>($"Sprites/Paywall/Paywall{seasonStr}Final");
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
			mainSprite.sprite = mainSpriteFinal;
			for (int i = 0; i < spritrs.Length; i++)
				spritrs[i].Disable();
		}


		public void Refresh()
		{
			for (int i = 0; i < spritrs.Length; i++)
				spritrs[i].Refresh();
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

		public void Set(FishRequired required)
		{
			this.required = required;
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

		public void Disable() => 
			SpriteRenderer.gameObject.SetActive(false);
	}
}
