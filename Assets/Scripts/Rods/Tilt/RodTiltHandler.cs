using System;

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
		}

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
	}
}
