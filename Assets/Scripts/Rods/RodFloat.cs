using System;
using UnityEngine;
using Assets.Scripts.Views.Rods;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.Framework.Utils;
using Random = UnityEngine.Random;
using System.Collections;
using Assets.Scripts.Framework;

namespace Assets.Scripts.Rods
{
	public class RodFloat
	{
		public bool IsCasted { get; private set; }
		public bool IsNibbling { get; private set; }
		public bool CanCatch { get; private set; }

		private bool isReseting;
		private Vector3 startWorldPos;

		public bool HasView => view != null;
		RodFloatView view;

		public void CreateView(Transform parent)
		{
			view = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<RodFloatView>("Prefabs/Rods/RodFloatBaseView"), parent);
			startWorldPos = view.transform.position;
		}

		public void Cast(Vector3 worldPos, float duration, Action onComplete)
		{
			IsCasted = true;
			worldPos.z = startWorldPos.z;

			if (HasView)
				view.Cast(worldPos, duration, onComplete);
			else
				CoroutineRunner.Wait(duration, onComplete);
		}

		public void Nibble(float duration, Action onComplete = null)
		{
			IsNibbling = true;
			int totalNibbles = Random.Range(2, 6);
			float oneNibbleDuration = duration / totalNibbles;
			NibbleRecursively(totalNibbles, oneNibbleDuration, onComplete: OnComplete);

			void OnComplete()
			{
				IsNibbling = false;
				onComplete?.Invoke();
			}
		}

		public void StopNibble() => IsNibbling = false;

		private void NibbleRecursively(int totalNibbles, float oneNibbleDuration, int currentnibbles = 0, Action onComplete = null)
		{
			if (currentnibbles == totalNibbles)
			{
				if (IsNibbling)
					onComplete?.Invoke();
				return;
			}

			// Pause
			float randomValue = Random.value;
			float pauseDuration = oneNibbleDuration * randomValue;
			float sinkAndRiseDuration = ((1f - randomValue) * oneNibbleDuration) / 2f;

			CoroutineRunner.RunCoroutine(NibbleWait(pauseDuration, Sink));

			void Sink()
			{
				if (HasView)
					view.Sink(sinkAndRiseDuration);

				CoroutineRunner.RunCoroutine(NibbleWait(sinkAndRiseDuration, Rise));
			}

			void Rise()
			{
				if (HasView)
					view.Rise(sinkAndRiseDuration);

				CoroutineRunner.RunCoroutine(NibbleWait(sinkAndRiseDuration, OnRise));
			}

			IEnumerator NibbleWait(float waiTime, Action onWaitComplete)
			{
				float startTime = Time.time;
				while (IsNibbling && Time.time - startTime <= waiTime)
					yield return null;
				onWaitComplete();
			}

			void OnRise() =>
				NibbleRecursively(totalNibbles, oneNibbleDuration, ++currentnibbles, onComplete);
		}


		public void Hooked()
		{
			CanCatch = false;
		}

		public void CatchWindow(float duration, Action onComplete = null)
		{
			CanCatch = true;
			
			if (HasView)
				view.SinkAndDisapear(0.2f);

			CoroutineRunner.Wait(duration, OnComplete);

			void OnComplete()
			{
				CanCatch = false;
				onComplete?.Invoke();
			}
		}

		public void Reset()
		{
			if (isReseting)
				return;

			isReseting = true;

			if (HasView)
				view.Reset(startWorldPos, 0.5f, Continue);
			else
				Continue();

			void Continue()
			{
				IsCasted = false;
				IsNibbling = false;
				CanCatch = false;
				isReseting = false;
			}

		}
	}
}
