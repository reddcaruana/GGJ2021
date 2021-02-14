using UnityEngine;

namespace Assets.Scripts.Rods.Tilt
{
	public static class RodTiltFactory
	{
		public static RodTiltHandler Create()
		{
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
				return new RodTiltHandlerGyro();
			else
				return new RodTiltHandlerMouse();
		}
	}
}
