using System;
using UnityEngine;

namespace Assets.Scripts.Framework.Tools
{
	public class RDComponentAccelerationModule<T>
		where T : Component
	{
		private readonly T component;
		private readonly RDAccelerationModule accelerator;

		public RDComponentAccelerationModule(T component)
		{
			this.component = component;
			accelerator = new RDAccelerationModule(OnMove, GetWorldPos);
		}

		public void ApplySpped(float speed) => accelerator.SetSpeed(speed);
		public void SetBounds(Func<Vector3, Vector3> checkBounds) => accelerator.SetBounds(checkBounds);
		public void SetDirection(float angle) => accelerator.SetDirection(angle);
		public void SetDirection(Vector3 direction) => accelerator.SetDirection(direction);
		private void OnMove(Vector3 worldPos) => component.transform.position = worldPos;
		private Vector3 GetWorldPos() => component.transform.position;
	}
}
