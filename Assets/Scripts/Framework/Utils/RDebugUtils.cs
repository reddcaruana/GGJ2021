using UnityEngine;
using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.Framework.Utils
{
	public static class RDebugUtils
	{
		[System.Diagnostics.Conditional("RDEBUG"),
		System.Diagnostics.Conditional("RDEBUG_LOG_ONLY")]
		public static void Log(string msg) =>
			Debug.Log(msg);

		[System.Diagnostics.Conditional("RDEBUG"),
		System.Diagnostics.Conditional("RDEBUG_LOG_ONLY")]
		public static void LogError(string msg) =>
			Debug.LogError(msg);

		[System.Diagnostics.Conditional("RDEBUG"),
		System.Diagnostics.Conditional("RDEBUG_LOG_ONLY")]
		public static void Assert(bool condition, string msg) =>
			Debug.Assert(condition, msg);

		[System.Diagnostics.Conditional("RDEBUG")]
		public static void CreateCircularDebugAreaView(float size, Color color, string name, int layer, Transform parent, Vector3 localPos = default)
		{
			SpriteRenderer debugArea = new GameObject(name).AddComponent<SpriteRenderer>();
			debugArea.transform.SetParent(parent);
			debugArea.transform.localPosition = localPos;
			debugArea.sprite = AssetLoader.ME.Load<Sprite>("Sprites/Circle");
			debugArea.color = color;
			debugArea.sortingOrder = layer;
			debugArea.transform.localScale = Statics.VECTOR2_ONE * (size / debugArea.bounds.size.y);
		}


		[System.Diagnostics.Conditional("RDEBUG")]
		public static void CreateRectangularDebugAreaView(Area2D area, Color color, string name, int layer, Transform parent)
		{
			SpriteRenderer debugArea = new GameObject(name).AddComponent<SpriteRenderer>();
			debugArea.drawMode = SpriteDrawMode.Sliced;
			debugArea.transform.SetParent(parent);
			debugArea.transform.ResetTransforms();
			debugArea.transform.localPosition = new Vector3(area.Center.x, area.Center.y, 10);
			debugArea.sprite = AssetLoader.ME.Load<Sprite>("Sprites/Square");
			debugArea.color = color;
			debugArea.sortingOrder = layer;
			debugArea.size = area.Size;
		}
	}
}
