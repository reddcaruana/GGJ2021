using System;
using UnityEngine;
using Assets.Scripts.Framework.Tools;
using System.Collections;

namespace Assets.Scripts.Framework.Utils
{
	/// <summary>
	/// This is a Coroutine Utility where it allows you to run coroutines when a Monobehavior is not available,
	/// or you want the coroutine to live while switching scenes.
	/// </summary>
	public class CoroutineRunner : RDMonobehaviourSingleton<CoroutineRunner>
	{

		protected override void Awake()
		{
			base.Awake();
			MakePersistent();
		}

		public static Coroutine RunCoroutine(IEnumerator routine)
		{
			return ME.StartCoroutine(routine);
		}

		public static void HaltCoroutine(Coroutine routine)
		{
			ME.StopCoroutine(routine);
		}

		public static Coroutine Wait(float delay, Action onComplete)
		{
			return RunCoroutine(WaitCoroutine(delay, onComplete));
		}

		static IEnumerator WaitCoroutine(float delay, Action onComplete)
		{
			yield return new WaitForSeconds(delay);
			onComplete();
		}

		public static Coroutine WaitUntil(Func<bool> predicate, Action onComplete)
		{
			return RunCoroutine(WaitUntilCoroutine(predicate, onComplete));
		}

		public static Coroutine WaitWhile(Func<bool> predicate, Action onComplete)
		{
			return RunCoroutine(WaitWhileCoroutine(predicate, onComplete));
		}

		static IEnumerator WaitUntilCoroutine(Func<bool> predicate, Action onComplete)
		{
			yield return new WaitUntil(predicate);
			onComplete?.Invoke();
		}

		static IEnumerator WaitWhileCoroutine(Func<bool> predicate, Action onComplete)
		{
			yield return new WaitWhile(predicate);
			onComplete?.Invoke();
		}

		public static Coroutine WaitForAFrame(Action onComplete)
		{
			return RunCoroutine(WaitForAFrameCoroutine(onComplete));
		}

		static IEnumerator WaitForAFrameCoroutine(Action onComplete)
		{
			yield return null;
			onComplete();
		}
	}
}
