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
		public bool IsResting { get; private set; }

		/// <summary>
		/// Passive Damage
		/// </summary>
		public float BlowBack { get; private set; }

		public Energy(float max, float normalisedBlowBack)
		{
			Max = Value = max;
			BlowBack = Max * normalisedBlowBack;
		}

		public void Consume(float value, bool autoRest = true)
		{
			if (IsResting)
				return;

			Value = Mathf.Max(Value - value, 0);

			if (autoRest && Value == 0)
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
			while(IsResting && Value < Max)
			{
				startTime = Time.time;
				while(IsResting && Time.time - startTime < 1f)
					yield return null;

				if (IsResting)
					Recover(RECOVERY_PER_SECOND);
			}
		}

		public void Recover(float value)
		{
			Value = Mathf.Min(Max, Value + value);
			if (Value == Max)
				IsResting = false;
		}

		public void StopResting() => IsResting = false;

		public void Reset() => Recover(Max - Value);
	}
}
