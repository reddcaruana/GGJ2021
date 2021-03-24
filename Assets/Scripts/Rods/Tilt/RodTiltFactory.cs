using UnityEngine;

namespace Assets.Scripts.Rods.Tilt
{
	public static class RodTiltFactory
	{
		public static RodTiltHandler Create()
		{
			if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
				return new RodTiltHandlerTouch();
			else
				return new RodTiltHandlerTouch();
		}
	}
}
