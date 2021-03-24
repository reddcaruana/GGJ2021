using UnityEngine;
using System.Collections;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Generic
{
	public class Energy
	{
		private const float RECOVERY_PER_SECOND = 3.5f; // 3.5 energy per 1 second

		public float Max { get; private set; }
		public float Value { get; private set; }
		public float NormalisedBlowBack { get; private set; }
		public bool IsResting { get; private set; }
		public bool IsPaused { get; set; }

		/// <summary>
		/// Passive Damage
		/// </summary>
		public float BlowBack { get; private set; }

		public Energy(float max, float normalisedBlowBack)
		{
			Value = max;
			ResetProperties(max, normalisedBlowBack);
		}

		public void ResetProperties(float max) => ResetProperties(max, NormalisedBlowBack);

		public void ResetProperties(float max, float normalisedBlowBack)
		{
			Max = max;
			NormalisedBlowBack = normalisedBlowBack;
			BlowBack = Max * normalisedBlowBack;
		}

		public void Consume(float value, bool autoRest = true)
		{
			if (IsResting || IsPaused)
				return;

			Value = Mathf.Max(Value - value, 0);

			if (autoRest && Value == 0)
				Rest();
		}

		public void Rest()
		{
			if (IsResting || IsPaused)
				return;

			IsResting = true;
			CoroutineRunner.RunCoroutine(RestCoroutine());
		}

		private IEnumerator RestCoroutine()
		{
			while(IsResting && Value < Max)
			{
				Recover(Time.deltaTime * RECOVERY_PER_SECOND);
				yield return null;
			}
		}

		public void Recover(float value)
		{
			if (IsPaused)
				return;

			Value = Mathf.Min(Max, Value + value);
			if (Value == Max)
				IsResting = false;
		}

		public void StopResting() => IsResting = false;

		public void Reset() => Recover(Max - Value);
	}
}
