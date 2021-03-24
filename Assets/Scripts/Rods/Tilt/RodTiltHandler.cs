using System;
using UnityEngine;

namespace Assets.Scripts.Rods.Tilt
{
	public abstract class RodTiltHandler
	{
		protected Action tiltLeft;
		protected Action tiltRight;
		protected Action tiltCenter;

		protected bool IsActive { get; private set; }

		public void Init(Action tiltLeft, Action tiltRight, Action tiltCenter)
		{
			this.tiltLeft = tiltLeft;
			this.tiltRight = tiltRight;
			this.tiltCenter = tiltCenter;

			InitInternal();
		}

		protected abstract void InitInternal();

		public void Start()
		{
			IsActive = true;
			StartInternal();
		}

		protected abstract void StartInternal();

		public void Stop()
		{
			IsActive = false;
			StopInternal();
		}

		protected abstract void StopInternal();

		public abstract bool InReservedArea(Vector3 worldPosition);
	}
}
