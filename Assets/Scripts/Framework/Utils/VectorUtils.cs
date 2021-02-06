using UnityEngine;

namespace Assets.Scripts.Framework.Utils
{
    public static class VectorUtils
    {
        // Vector2 -------------------------------------------------------------------
        public static Vector2 RadianToVector2(float radian)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }

        public static Vector2 DegreeToVector2(float degree)
        {
            return RadianToVector2(degree * Mathf.Deg2Rad);
        }

        // Vector3 -------------------------------------------------------------------

        public static Vector3 Multiply(Vector3 a, Vector3 b)
        {
            return new Vector3()
            {
                x = a.x * b.x,
                y = a.y * b.y,
                z = a.z * b.z
            };
        }

        public static Vector3 Devide(Vector3 a, Vector3 b)
        {
            Vector3 result = new Vector3();
            for (int i = 0; i < 3; i++)
            {
                if (a[i] == 0 || b[i] == 0)
                    result[i] = 0;
                else
                    result[i] = a[i] / b[i];
            }

            return result;
        }

        /// <summary>
		/// Returns the Absolute values of a Vector3
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Vector3 Abs(Vector3 value) =>
            new Vector3(Mathf.Abs(value.x), Mathf.Abs(value.y), Mathf.Abs(value.z));

        public static string Print(Vector3 source) => $"({source[0]}, {source[1]}, {source[2]})";
    }
}
