using UnityEngine;
using Assets.Scripts.Framework;
using Assets.Scripts.AssetsManagers;

namespace Assets.Scripts.Utils
{
	public static class DebugUtils
	{
		[System.Diagnostics.Conditional("RDEBUG")]
		public static void Log(string msg) =>
			Debug.Log(msg);

		[System.Diagnostics.Conditional("RDEBUG")]
		public static void LogError(string msg) =>
			Debug.LogError(msg);

		[System.Diagnostics.Conditional("RDEBUG")]
		public static void CreateDebugAreaView(float size, Color color, string name, int layer, Transform parent, Vector3 localPos = default)
		{
			SpriteRenderer debugArea = new GameObject(name).AddComponent<SpriteRenderer>();
			debugArea.transform.SetParent(parent);
			debugArea.transform.localPosition = localPos;
			debugArea.sprite = AssetLoader.ME.Loader<Sprite>("Sprites/Circle");
			debugArea.color = color;
			debugArea.sortingOrder = layer;
			debugArea.transform.localScale = Statics.VECTOR2_ONE * (size / debugArea.bounds.size.y);
		}

	}
}
