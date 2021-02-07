using UnityEngine;

namespace Assets.Scripts.Framework.Utils
{
	public static class MathUtils
	{
		/// <summary>
		/// Returns true if point is in the circle defined.
		/// </summary>
		/// <param name="center"></param>
		/// <param name="radius"></param>
		/// <param name="point"></param>
		/// <returns></returns>
		public static bool IsInCircualarArea(Vector2 center, float radius, Vector2 point)
		{
			float distance = Vector2.Distance(center, point);
			return distance <= radius;
		}

		public static bool IsInRectArea(Area2D area, Vector2 position) => 
			IsInRectArea(area.TopLeftCorner, area.BottomRightCorner, position);

		/// <summary>
		/// Return true if the position is in the rectangle defined
		/// </summary>
		/// <param name="topLeftCorner"></param>
		/// <param name="bottomRightCorner"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static bool IsInRectArea(Vector2 topLeftCorner, Vector2 bottomRightCorner, Vector2 position) =>
			(position.x >= topLeftCorner.x && position.x <= bottomRightCorner.x) &&
			(position.y <= topLeftCorner.y && position.y >= bottomRightCorner.y);

		/// <summary>
		/// Returns the hypotenuse of a right angled triangle, given that you know two sides.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static float HypotenuseOfRightAngleTriangle(float x, float y)
		{
			return Mathf.Sqrt((Mathf.Pow(y, 2f) + Mathf.Pow(x, 2f)) - (2f * y * x * Mathf.Cos(90f * Mathf.Deg2Rad)));
		}

		/// <summary>
		/// Calculates the angle from 0 to 360 of given Coordinates.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static float CalculateAngleFromGivenCoordinates(float x, float y)
		{
			float hypotenuse = HypotenuseOfRightAngleTriangle(x, y);
			if (hypotenuse == 0)
				return 0;

			float angle;
			float newY = y / hypotenuse;
			float newX = x / hypotenuse;
			angle = Mathf.Acos(newX) * Mathf.Rad2Deg;
			if (newY < 0)
				angle = 360f - angle;

			return angle;
		}

		public static Vector2 FindCoordinatesFromAngle(float angle, float distace)
		{
			// Assuming that we always start from origin

			float x = Mathf.Cos(Mathf.Deg2Rad * angle);
			float y = Mathf.Sin(Mathf.Deg2Rad * angle);
			return new Vector2(x * distace, y * distace);
		}

		/// <summary>
		/// Returns the linear distance between axis. Result can be negative
		/// </summary>
		/// <param name="startPos"></param>
		/// <param name="targetPosition"></param>
		/// <returns></returns>
		public static Vector3 DistanceOnAxis(Vector3 startPos, Vector3 targetPosition) =>
			targetPosition - startPos;

		/// <summary>
		/// Aim at a 2D target
		/// </summary>
		/// <param name="fromPosition"></param>
		/// <param name="targetPosition"></param>
		/// <param name=""></param>
		/// <returns></returns>
		public static float AimAtTarget2D(Vector2 fromPosition, Vector2 targetPosition)
		{
			Vector2 distanceOnAxis = DistanceOnAxis(fromPosition, targetPosition);
			return CalculateAngleFromGivenCoordinates(distanceOnAxis[0], distanceOnAxis[1]);
		}

		public static int ClampIndex(int currentIndex, int lenght)
		{
			if (currentIndex < 0)
				currentIndex = lenght + currentIndex;
			else if (currentIndex >= lenght)
				currentIndex -= lenght;

			return currentIndex;
		}

		public static bool InRangeInt(int value, int min, int max) => value >= min && value <= max;
		public static bool InRangeFloat(float value, float min, float max) => value >= min && value <= max;
		public static bool ValidateIndex(int index, int lenght) => InRangeInt(index, 0, lenght - 1);

		public static int LoopIndex(int value, int max, int min = 0) =>
			value >= max ? value - max : value < min ? max + value : value;
	}

	public readonly struct Area2D
	{
		public readonly Vector2 TopLeftCorner;
		public readonly Vector2 BottomRightCorner;
		public readonly Vector2 Center;
		public readonly float Width;
		public readonly float Height;
		public float Area => Width * Height;
		public Vector2 Size => new Vector2(Width, Height);

		public Area2D(float width, float height, Vector2 center)
		{
			Width = width;
			Height = height;
			Center = center;
			TopLeftCorner = center;
			TopLeftCorner.x -= Width / 2f;
			TopLeftCorner.y += Height / 2f;
			BottomRightCorner = TopLeftCorner;
			BottomRightCorner.x += Width;
			BottomRightCorner.y -= Height;
		}

	}
}
