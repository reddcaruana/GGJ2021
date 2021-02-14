using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Views.Rods;
using Random = UnityEngine.Random;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Utils;

namespace Assets.Scripts.Rods
{
	public class RodFloat
	{
		public bool IsCasted { get; private set; }
		public bool IsNibbling { get; private set; }
		public bool CanCatch { get; private set; }

		public bool IsReseting { get; private set; }
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

			CoroutineRunner.RunCoroutine(NibbleWait(pauseDuration, Nibble));

			void Nibble()
			{
				if (HasView)
					view.Nibble(sinkAndRiseDuration);

				CoroutineRunner.RunCoroutine(NibbleWait(sinkAndRiseDuration, Rise));
			}

			void Rise()
			{
				if (HasView)
					view.Float();

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

		public void CatchWindow(float duration, Action onEscape)
		{
			CanCatch = true;
			
			if (HasView)
				view.SinkAndDisapear(0.2f);

			CoroutineRunner.RunCoroutine(CatchWindowCoroutine(duration, onEscape));
		}

		private IEnumerator CatchWindowCoroutine(float duration, Action onEscape)
		{
			float startTime = Time.time;
			while (Time.time - startTime < duration)
			{
				// OnHooked CanCatch will be false
				if (!CanCatch)
					yield break;
				yield return null;
			}
			CanCatch = false;
			onEscape();
		}

		public void Reset()
		{
			if (IsReseting)
				return;

			DebugUtils.Log("Reeled In - Reset ........./!");

			IsReseting = true;

			if (HasView)
				view.Reset(startWorldPos, 0.5f, Continue);
			else
				Continue();

			void Continue()
			{
				IsCasted = false;
				IsNibbling = false;
				CanCatch = false;
				IsReseting = false;
			}

		}
	}
}
