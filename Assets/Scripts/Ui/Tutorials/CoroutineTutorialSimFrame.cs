using UnityEngine;
using System.Collections;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Ui.Tutorials
{
	public abstract class CoroutineTutorialSimFrame : TutorialSimFrame
	{
		private Coroutine simCoroutine;

		protected override void PlayInternal()
		{
			if (simCoroutine != null)
				return;

			simCoroutine = CoroutineRunner.RunCoroutine(SimCoroutine());
		}

		protected override void StopInternal()
		{
			if (simCoroutine == null)
				return;

			CoroutineRunner.HaltCoroutine(simCoroutine);
			simCoroutine = null;
		}

		protected abstract IEnumerator SimCoroutine();

	}
}
