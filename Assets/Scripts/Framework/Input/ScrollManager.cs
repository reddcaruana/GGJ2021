using System;
using UnityEngine;

namespace Assets.Scripts.Framework.Input
{
	public class ScrollManager
	{
		private const float TRASH_HOLD_TIME_MIN = 0.2f;

		private Vector3 startWorldPosition;
		private float startTime;

		public bool IsActive { get; private set; }
		public bool IsScrolling { get; private set; }

		public Action<DragData> onDragStart;
		public Action<DragData> onDrag;
		public Action<SwipeData> onDragEnd;

		public void OnPrimaryContactStarted(Vector3 worldPosition, float time)
		{
			IsActive = true;
			PositionAndTimeUpdate(worldPosition, time);
		}

		private void PositionAndTimeUpdate(Vector3 worldPosition, float time)
		{
			startWorldPosition = worldPosition;
			startTime = time;
		}

		public void OnPosition(Vector3 worldPosition)
		{
			if (startTime == 0)
				return;

			if (!IsScrolling)
			{
				float deltaTime = Time.time - startTime;
				if (deltaTime <= TRASH_HOLD_TIME_MIN)
					return;
				else
				{
					IsScrolling = true;
					onDragStart?.Invoke(CreateDragData(worldPosition, Time.time));
				}
			}

			onDrag?.Invoke(CreateDragData(worldPosition, Time.time));

			PositionAndTimeUpdate(worldPosition, Time.time);
		}

		public void OnPrimaryContactEnded(Vector3 worldPosition, float time)
		{
			if (IsScrolling)
				onDragEnd?.Invoke(CreateSwipeData(worldPosition, time));

			startTime = 0;
			IsScrolling = false;
			IsActive = false;
		}

		private DragData CreateDragData(Vector3 worldPosition, float currentTime)
		{
			float deltaTime = currentTime - startTime;
			Vector2 direction = worldPosition - startWorldPosition;
			return new DragData(startWorldPosition, worldPosition, direction.normalized, deltaTime);
		}

		private SwipeData CreateSwipeData(Vector3 worldPosition, float currentTime)
		{
			float deltaTime = currentTime - startTime;
			Vector2 direction = worldPosition - startWorldPosition;
			return new SwipeData(direction.normalized, deltaTime, Vector3.Distance(startWorldPosition, worldPosition));
		}
	}

	public readonly struct DragData 
	{
		public readonly Vector3 WorldStartPosition;
		public readonly Vector3 WorldEndPosition;
		public readonly Vector3 Direction;
		public readonly float Time;

		public float Distance => Vector3.Distance(WorldStartPosition, WorldEndPosition);
		public Vector3 AxisDistance => WorldStartPosition - WorldEndPosition;

		public DragData(Vector3 worldStartPosition, Vector3 worldEndPosition, Vector3 direction, float time)
		{
			WorldStartPosition = worldStartPosition;
			WorldEndPosition = worldEndPosition;
			Direction = direction;
			Time = time;
		}

		/// <summary>
		/// Return the most powerful direction
		/// </summary>
		/// <returns></returns>
		public Vector3 FilteredDirection() => SwipeManager.FilterForPrimaryDirection(Direction);
	}
}
