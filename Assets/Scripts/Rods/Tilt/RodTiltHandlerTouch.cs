using UnityEngine;
using Assets.Scripts.Controllers;
using Assets.Scripts.Framework.Input;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Rods.Tilt
{
	public class RodTiltHandlerTouch : RodTiltHandler
	{
		public const float HEIGHT_PERCENTAGE = 0.3f;
		private Area2D clickArea;
		private float limitLeftToCenter;
		private float limitCenterToRight;

		protected override void InitInternal()
		{
			float height = ViewController.Area.Height * HEIGHT_PERCENTAGE;
			Vector2 center = ViewController.ScreenBottom;
			center.y += height / 2f;
			clickArea = new Area2D(ViewController.Area.Width, height, center);

			float limitDistance = ViewController.Area.Width / 3f;
			limitLeftToCenter = clickArea.TopLeftCorner.x + limitDistance;
			limitCenterToRight = limitLeftToCenter + limitDistance;

			InputManager.onClickPosition += OnClick;

			DrawDebugAreas(limitDistance, height);
		}

		protected override void StartInternal()
		{
		}

		protected override void StopInternal()
		{
		}

		protected void OnClick(Vector3 worldPosition)
		{
			//DebugUtils.Log($"[Tilt] IsActive: {IsActive}, IsInArea: {MathUtils.IsInRectArea(clickArea, worldPosition)}");

			if (!GameController.ME.CanClick || !IsActive || !MathUtils.IsInRectArea(clickArea, worldPosition))
				return;

			//DebugUtils.Log($"[Tilt] Tilting: L: {IsLeft(worldPosition.x)}, C:{IsCenter(worldPosition.x)}, R: {IsRight(worldPosition.x)}");
			if (IsLeft(worldPosition.x))
				tiltLeft();
			else if (IsCenter(worldPosition.x))
				tiltCenter();
			else if (IsRight(worldPosition.x))
				tiltRight();
		}

		private bool IsLeft(float x) => MathUtils.InRangeFloat(x, clickArea.TopLeftCorner.x, limitLeftToCenter);
		private bool IsCenter(float x) => MathUtils.InRangeFloat(x, limitLeftToCenter, limitCenterToRight);
		private bool IsRight(float x) => MathUtils.InRangeFloat(x, limitCenterToRight, clickArea.BottomRightCorner.x);

		public override bool InReservedArea(Vector3 worldPosition) => MathUtils.IsInRectArea(clickArea, worldPosition);

		[System.Diagnostics.Conditional("RDEBUG")]
		private void DrawDebugAreas(float limitDistance, float height)
		{
			Color color = Color.blue;
			color.a = 0.5f;
			Area2D area = new Area2D(limitDistance, height, new Vector2(clickArea.TopLeftCorner.x - limitLeftToCenter, clickArea.Center.y));
			RDebugUtils.CreateRectangularDebugAreaView(area, color, "Left", 25, ViewController.MainCamera.transform);

			color = Color.yellow;
			color.a = 0.5f;
			area = new Area2D(limitDistance, height, new Vector2(limitCenterToRight + limitLeftToCenter, clickArea.Center.y));
			RDebugUtils.CreateRectangularDebugAreaView(area, color, "Center", 25, ViewController.MainCamera.transform);

			color = Color.magenta;
			color.a = 0.5f;
			area = new Area2D(limitDistance, height, new Vector2(clickArea.BottomRightCorner.x - limitCenterToRight, clickArea.Center.y));
			RDebugUtils.CreateRectangularDebugAreaView(area, color, "Right", 25, ViewController.MainCamera.transform);
		}
	}
}
