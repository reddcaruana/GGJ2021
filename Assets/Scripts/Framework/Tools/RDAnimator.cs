using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Framework.Tools
{
	public enum AnimationState { Play, Pause, Stop}
	public enum LoopType { None, Normal, PingPong}
	public class RDAnimator
	{
		public int Frames { get; set; }
		public float FPS { get; set; } = 30f;
		public LoopType Loop { get; set; } = LoopType.None;
		public AnimationState State { get; private set; } = AnimationState.Stop;
		public int CurrentFrame { get; private set; } = 0;

		private readonly Action<int> onFrameUpdate;
		private Action onNonLoopAnimationComplete;
		private int direction = 1;

		public RDAnimator(Action<int> onFrameUpdate) : this(0, onFrameUpdate)
		{
		}

		public RDAnimator(int frames, Action<int> onFrameUpdate)
		{
			Frames = frames;
			this.onFrameUpdate = onFrameUpdate;
		}

		public void RegisterToNoLoopAnimationCompletion(Action callback) => onNonLoopAnimationComplete += callback;
		public void UnregisterToNoLoopAnimationCompletion(Action callback) => onNonLoopAnimationComplete -= callback;

		public void Play()
		{
			if (State == AnimationState.Play)
				return;

			State = AnimationState.Play;
			CoroutineRunner.RunCoroutine(PlaybackCoroutine());
		}

		public void Pause()
		{
			State = AnimationState.Pause;
		}

		public void Stop()
		{
			State = AnimationState.Stop;
			CurrentFrame = 0;
			direction = 1;
		}

		private IEnumerator PlaybackCoroutine()
		{
			onFrameUpdate(CurrentFrame);

			int tempFrame;
			float startTime;
			while(State == AnimationState.Play)
			{
				startTime = Time.time;
				// Wait for FPS
				while (Time.time - startTime < 1f / FPS)
				{
					if (State != AnimationState.Play)
						yield break;
					yield return null;
				}

				onFrameUpdate(CurrentFrame);

				if (CurrentFrame == Frames - 1 && Loop == LoopType.None)
				{
					Stop();
					onNonLoopAnimationComplete?.Invoke();
					yield break;
				}

				tempFrame = CurrentFrame + direction;
				if (Loop == LoopType.PingPong && (tempFrame == Frames || tempFrame < 0))
				{
					direction *= -1;
					tempFrame = CurrentFrame + direction;
				}

				CurrentFrame = MathUtils.LoopIndex(tempFrame, Frames);
			}
		}
	}
}
