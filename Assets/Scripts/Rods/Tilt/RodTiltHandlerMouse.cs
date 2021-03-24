using UnityEngine;
using System.Collections;
using Assets.Scripts.Framework.Input;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Rods.Tilt
{
	public class RodTiltHandlerMouse : RodTiltHandler
	{
		protected override void InitInternal()
		{
		}

		protected override void StartInternal()
		{
			CoroutineRunner.RunCoroutine(MouseTrackingCorotine());
		}

		protected override void StopInternal()
		{
		}

		private IEnumerator MouseTrackingCorotine()
		{
			Vector2 mousePosition;
			while (IsActive)
			{
				mousePosition = InputManager.GetScreenPosition();

				float halfX = (Screen.width / 2);
				float padding = halfX * 0.2f;

				if (mousePosition.x < (halfX - padding))
					tiltLeft();

				else if (mousePosition.x > (halfX + padding))
					tiltRight();

				yield return null;
			}
		}

		public override bool InReservedArea(Vector3 worldPosition) => false;
	}
}
