using UnityEngine;
using Assets.Scripts.Controllers;
using Assets.Scripts.Framework.Input;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Rods.Tilt
{
	public class RodTiltHandlerTouch : RodTiltHandler
	{
		private const float HEIGHT_PERCENTAGE = 0.3f;
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
		}

		protected override void StartInternal()
		{
		}

		protected override void StopInternal()
		{
		}

		protected void OnClick(Vector3 worldPosition)
		{
			if (!IsActive || !MathUtils.IsInRectArea(clickArea, worldPosition))
				return;

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
	}
}
