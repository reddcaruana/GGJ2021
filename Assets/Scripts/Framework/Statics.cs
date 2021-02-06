using UnityEngine;

namespace Assets.Scripts.Framework
{
	public static class Statics
	{
        public static readonly Vector3 VECTOR3_ZERO = new Vector3(0, 0, 0);
        public static readonly Vector2 VECTOR3_HALF = new Vector3(0.5f, 0.5f);
        public static readonly Vector3 VECTOR3_ONE = new Vector3(1, 1, 1);
        public static readonly Vector2 VECTOR2_ZERO = new Vector2(0, 0);
        public static readonly Vector2 VECTOR2_HALF = new Vector2(0.5f, 0.5f);
        public static readonly Vector2 VECTOR2_ONE = new Vector2(1, 1);

        public static readonly Color COLOR_WHITE = Color.white;
        public static readonly Color COLOR_BLACK = Color.black;
        public static readonly Color COLOR_ZERO = new Color(0f, 0f, 0f, 0f);
    }
}
