using UnityEngine;

namespace Assets.Scripts.Controllers
{
	public static class ViewController
	{
		static public Camera MainCamera = Camera.main;

		public static readonly float Height;
		public static readonly float Width;

		static ViewController()
		{
			Height = MainCamera.orthographicSize * 2.0f;
			Width = Height * Screen.width / Screen.height;
		}
	}
}
