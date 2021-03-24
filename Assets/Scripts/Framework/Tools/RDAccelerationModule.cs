﻿using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Framework.Tools
{
	public class RDAccelerationModule
	{

		public bool HasBounds => checkPosition != null;
		public Vector2 DirectionVector { get; private set; } = new Vector2(0f, 1f);
		public bool IsMoving => speedVector != Statics.VECTOR2_ZERO;
		public bool KillSpeedVectorOnOutOfBounds { get; set; } = false;
		private bool isActive;
		public bool IsActive 
		{
			get => isActive;
			set
			{
				isActive = value;

				if (value && coroutine == null)
					coroutine = CoroutineRunner.RunCoroutine(UpdateCoroutine());

				else if (!value && coroutine != null)
				{
					CoroutineRunner.HaltCoroutine(coroutine);
					coroutine = null;
				}
			} 
		}

		private Action<Vector3> moveUpdate;
		private Func<Vector3> getWorldPosition;
		private Func<Vector3, Vector3> checkPosition;
		
		public float Speed { get; private set; }
		private float drag = 0.99f;
		private Vector2 speedVector = new Vector2();
		private float speedVectorTrashHold = 0.0001f;

		private Coroutine coroutine;

		public RDAccelerationModule(Action<Vector3> moveUpdate, Func<Vector3> getWorldPosition)
		{
			this.moveUpdate = moveUpdate;
			this.getWorldPosition = getWorldPosition;
		}

		public void SetBounds(Func<Vector3, Vector3> checkPosition) =>
			this.checkPosition = checkPosition;

		public void SetDirection(float angle)
		{
			Vector3 result = MathUtils.FindCoordinatesFromAngle(angle, 1f);
			SetDirection(result);
		}

		public void SetDirection(Vector3 directionVector) => DirectionVector = directionVector;
		
		public void SetSpeedVectorTrashHold(float trashHold) => speedVectorTrashHold = trashHold;

		public void SetSpeed(float speed) => this.Speed = speed;

		public void SetDrag(float normalized) => drag = normalized;

		private IEnumerator UpdateCoroutine()
		{
			while (IsActive)
			{
				speedVector += DirectionVector * Speed;
				speedVector *= drag;

				TrashHold();

				Vector2 pos = getWorldPosition();

				if (IsMoving)
				{
					pos += speedVector;

					if (HasBounds)
					{
						Vector2 boundsPos = checkPosition(pos);

						if (KillSpeedVectorOnOutOfBounds)
						{
							if (pos.x != boundsPos.x)
								speedVector.x = 0;
							if (pos.y != boundsPos.y)
								speedVector.y = 0;
						}

						pos = boundsPos;
					}
				}

				moveUpdate(pos);

				Speed = 0;
				yield return null;
			}
		}

		private void TrashHold()
		{	
			speedVector[0] = Check(speedVector[0]) ? 0 : speedVector[0];
			speedVector[1] = Check(speedVector[1]) ? 0 : speedVector[1];

			bool Check(float value) => 
				MathUtils.InRangeFloat(value, -speedVectorTrashHold, speedVectorTrashHold);
		}

		public void ResetSpeed()
		{
			speedVector.x = 0;
			speedVector.y = 0;
			Speed = 0;
		}
	}
}
