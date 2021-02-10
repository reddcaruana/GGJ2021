using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Framework.Tools
{
	public class RDAccelerationModule
	{
		private Action<Vector3> moveUpdate;
		private Func<Vector3> getWorldPosition;
		private Func<Vector3, Vector3> checkPosition;

		public bool IsActive { get; set; } = true;
		public bool HasBounds => checkPosition != null;

		private float speed;
		private float drag = 0.99f;
		private Vector2 directionVector = new Vector2(0f, 1f);
		private Vector2 speedVector = new Vector2();

		private Coroutine coroutine;

		public RDAccelerationModule(Action<Vector3> moveUpdate, Func<Vector3> getWorldPosition)
		{
			this.moveUpdate = moveUpdate;
			this.getWorldPosition = getWorldPosition;
			coroutine = CoroutineRunner.RunCoroutine(UpdateCoroutine());
		}

		public void SetBounds(Func<Vector3, Vector3> checkPosition) =>
			this.checkPosition = checkPosition;

		public void SetDirection(float angle)
		{
			Vector3 result = MathUtils.FindCoordinatesFromAngle(angle, 1f);
			SetDirection(result);
		}

		public void SetDirection(Vector3 directionVector) => this.directionVector = directionVector;

		public void SetSpeed(float speed) 
		{
			if (!IsActive)
				return;

			this.speed = speed; 
		}

		private IEnumerator UpdateCoroutine()
		{
			while (true)
			{
				speedVector += directionVector * speed;
				speedVector *= drag;
				Vector2 pos = getWorldPosition();
				pos += speedVector;

				if (HasBounds)
					pos = checkPosition(pos);

				moveUpdate(pos);

				speed = 0;
				yield return null;
			}
		}

		public void ForceStop()
		{
			IsActive = false;
			if (coroutine != null)
			{
				CoroutineRunner.HaltCoroutine(coroutine);
				coroutine = null;
			}
		}
	}
}
