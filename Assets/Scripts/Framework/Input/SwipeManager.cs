using System;
using UnityEngine;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Framework.Input
{
	public class SwipeManager
	{
		private const float TRASH_HOLD_MIN = 0.2f;
		private const float TRASH_HOLD_MAX = 1f;
		private const float HOLD_RADUIS = 0.1f;

		private Vector3 startWorldPosition;
		private float startTime;

		public Action<SwipeData> onSwipe;

		public void OnPrimaryContactStarted(Vector3 worldPosition, float time)
		{
			startWorldPosition = worldPosition;
			startTime = time;
		}

		public void OnPrimaryContactEnded(Vector3 worldPosition, float time)
		{
			float deltaTime = time - startTime;
			if (deltaTime <= TRASH_HOLD_MIN || deltaTime >= TRASH_HOLD_MAX || Vector3.Distance(startWorldPosition, worldPosition) <= HOLD_RADUIS)
				return;

			Vector2 direction = worldPosition - startWorldPosition;
			SwipeData data = new SwipeData(direction.normalized, deltaTime, Vector3.Distance(worldPosition, startWorldPosition));
			onSwipe?.Invoke(data);
		}

		/// <summary>
		/// Return the most powerful direction
		/// </summary>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static Vector2 FilterForPrimaryDirection(Vector2 direction)
		{
			Vector2 result = new Vector2();
			Vector2 abs = VectorUtils.Abs(direction);
			if (abs.x > abs.y)
				result.x = 1 * (direction.x > 0 ? 1 : -1);
			else
				result.y = 1 * (direction.y > 0 ? 1 : -1);
			return result;
		}
	}

	public readonly struct SwipeData
	{
		public readonly Vector3 Direction;
		public readonly float Time;
		public readonly float Distance;

		public SwipeData(Vector3 direction, float time, float distance)
		{
			Direction = direction;
			Time = time;
			Distance = distance;
		}

		/// <summary>
		/// Return the most powerful direction
		/// </summary>
		/// <returns></returns>
		public Vector3 FilteredDirection() => SwipeManager.FilterForPrimaryDirection(Direction);
	}
}
