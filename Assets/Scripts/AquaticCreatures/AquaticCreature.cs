using UnityEngine;
using Assets.Scripts.Constants;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Framework;

namespace Assets.Scripts.AquaticCreatures
{
	public abstract class AquaticCreature<T>
		where T : AquaticCreature<T>
	{
		public FishData Data { get; private set; }
		public float Weight { get; private set; }
		public float Size { get; private set; }
		public bool IsReadyToSet { get; protected set; } = true;
		protected bool isAvailable;
		public bool CanCatch => !IsReadyToSet && isAvailable;

		public void Set(FishData data)
		{
			Data = data;
			Weight = Data.GetRandomWeight();
			Size = Weight * FishWiki.SIZE_PER_WEIGHT_RATIO;

			SetInternal();

			IsReadyToSet = false;
		}

		protected abstract void SetInternal();

		protected bool InProximity(float distance, Vector2 localPosition) =>
			MathUtils.IsInCircualarArea(Statics.VECTOR2_ZERO, distance, localPosition);

		public abstract Vector3 GetPosition();

		public abstract void OnCast(Vector3 floatPosition);

		public void Appear()
		{
			isAvailable = true;
			AppearInternal();
		}

		protected abstract void AppearInternal();
	}
}
