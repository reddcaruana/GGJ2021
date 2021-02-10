using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Framework.Tools
{
	public enum AnimationState { Play, Pause, Stop}
	public class RDAnimator
	{
		public float FPS { get; set; }
		public bool IsLoop { get; set; }
		public AnimationState State { get; private set; } = AnimationState.Stop;

		private Action<int> onFrameUpdate;
		private int frames;
		private int currentFrame = 0;

		public RDAnimator(int frames, Action<int> onFrameUpdate)
		{
			this.frames = frames;
			this.onFrameUpdate = onFrameUpdate;
		}

		public void Play()
		{
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
			currentFrame = 0;
		}

		private IEnumerator PlaybackCoroutine()
		{
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

				onFrameUpdate(currentFrame++);

				currentFrame = MathUtils.LoopIndex(currentFrame, frames);

				if (currentFrame == 0 && !IsLoop)
					Stop();
			}
		}
	}
}
