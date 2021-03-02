using UnityEngine;

namespace Assets.Scripts.Framework.Ui.SafeArea
{
	public static class SafeAreaDetection
	{
		public delegate void SafeAreaChanged(Rect safeArea);
		public static event SafeAreaChanged OnSafeAreaChanged;

		private static Rect safeArea;

		public static void Init()
		{
			safeArea = Screen.safeArea;
		}

		public static void Update()
		{

			if (safeArea != Screen.safeArea)
			{
				safeArea = Screen.safeArea;
				OnSafeAreaChanged?.Invoke(safeArea);
			}
		}
	}
}
