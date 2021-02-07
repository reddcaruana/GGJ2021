using UnityEngine;
using System.Collections;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Utils;

namespace Assets.Scripts.Generic
{
	public class Energy
	{
		private const float RECOVERY_PER_SECOND = 3.5f; // 3.5 energy per 1 second

		public float Max { get; private set; }
		public float Current { get; private set; }
		public bool IsResting { get; private set; }

		/// <summary>
		/// Passive Damage
		/// </summary>
		public float BlowBack { get; private set; }

		public Energy(float max, float normalisedBlowBack)
		{
			Max = Current = max;
			BlowBack = Max * normalisedBlowBack;
		}

		public void Consume(float value, bool autoRest = true)
		{
			if (IsResting)
				return;

			Current = Mathf.Max(Current - value, 0);

			if (autoRest && Current == 0)
				Rest();
		}

		public void Rest()
		{
			if (IsResting)
				return;

			IsResting = true;
			CoroutineRunner.RunCoroutine(RestCoroutine());
		}

		private IEnumerator RestCoroutine()
		{
			float startTime;
			while(IsResting && Current < Max)
			{
				startTime = Time.time;
				while(IsResting && Time.time - startTime < 1f)
					yield return null;

				Recover(RECOVERY_PER_SECOND);
			}
		}

		public void Recover(float value) =>
			Current = Mathf.Min(Max, Current + value);

		public void StopResting() => IsResting = false;
	}
}
