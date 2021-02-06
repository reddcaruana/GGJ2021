using UnityEngine;

namespace Assets.Scripts.Framework.Utils
{
	public static class TransformUtils
	{
		public static void ResetTransforms(this Transform source, bool isWorld = true)
		{
			source.ResetLocalPos(isWorld);
			source.ResetRotattion(isWorld);
			source.ResetScale();
		}

		public static void ResetLocalPos(this Transform source, bool isWorld = true)
		{
			if (isWorld)
				source.position = Statics.VECTOR3_ZERO;
			else
				source.localPosition = Statics.VECTOR3_ZERO;
		}

		public static void ResetRotattion(this Transform source, bool isWorld = true)
		{
			if (isWorld)
				source.rotation = Quaternion.identity;
			else
				source.localRotation = Quaternion.identity;
		}

		public static void ResetScale(this Transform source) => source.localScale = Statics.VECTOR3_ONE;

		public static void MimicTransforms(this Transform source, Transform toCopy, bool isWorld = true)
		{
			source.localScale = toCopy.localScale;

			if (isWorld)
			{
				source.position = toCopy.position;
				source.rotation = toCopy.rotation;
			}
			else
			{
				source.localPosition = toCopy.localPosition;
				source.localRotation = toCopy.localRotation;
			}
		}
	}
}
