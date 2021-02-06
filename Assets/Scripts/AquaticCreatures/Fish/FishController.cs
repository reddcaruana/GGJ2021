using System;
using UnityEngine;
using Assets.Scripts.Constants;
using Assets.Scripts.Views.Fish;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.AquaticCreatures.Fish
{
	public class FishController : AquaticCreature<FishController>
	{
		private const float COOL_DOWN = 6f;

		private Action<FishController> onCoolDownComplete;
		private float proximityFear;
		private float proximityBite;
		private Vector3 spawnPosition;

		public bool HasView => view != null;
		private FishView view; 

		protected override void SetInternal()
		{
			proximityFear = CalculateProximityFear(Size);
			proximityBite = CalculateProximityBite(Size);

			if (HasView)
			{
				view.Set(this);
				view.CreateDebugAreaView(proximityFear, Color.red, "Fear");
				view.CreateDebugAreaView(proximityBite, Color.green, "Bite");
			}
		}

		public void CreateView(Transform parent) =>
			view = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<FishView>("Prefabs/Fish/FishBase"), parent);

		public void RegiisterToCoolDownComplete(Action<FishController> callback) =>
			onCoolDownComplete += callback;

		public void UnregiisterFromCoolDownComplete(Action<FishController> callback) =>
			onCoolDownComplete -= callback;

		public bool InFearProximity(Vector2 position) => InProximity(proximityFear, position);
		public bool InBiteProximity(Vector2 position) => InProximity(proximityBite, position);

		public override void OnCast(Vector3 floatPosition)
		{
			if (InFearProximity(floatPosition))
				Escape();
			else if (InBiteProximity(floatPosition))
				ApproachFloat(floatPosition);
		}

		protected override void AppearInternal()
		{
			if (HasView)
				view.Appear();
		}

		private void Escape()
		{
			Debug.Log("[FishController] Escape");

			if (HasView)
				view.Escape();

			CoolDown();
		}

		private void ApproachFloat(Vector2 floatPosition)
		{
			Debug.Log("[FishController] Approach");
			isAvailable = false;
		}

		private void CoolDown()
		{
			CoroutineRunner.Wait(COOL_DOWN, OnCoolDownComplete);

			void OnCoolDownComplete()
			{
				IsReadyToSet = true;
				onCoolDownComplete?.Invoke(this);
			}
		}

		public override Vector3 GetPosition() => spawnPosition;

		public void SetSpawnPoint(Vector3 position)
		{
			spawnPosition = position;

			if (HasView)
				view.Reposition(position, proximityFear, proximityBite);
		}

		private static float CalculateProximityFear(float size)
		{
			const float MinFear = 4f;
			return CalculateProximity(size, MinFear, FishWiki.PROXIMITY_FEAR);
		}

		private static float CalculateProximityBite(float size)
		{
			const float MinBite = 8f;
			return CalculateProximity(size, MinBite, FishWiki.PROXIMITY_BITE);
		}

		private static float CalculateProximity(float size, float min, float range)
		{
			float result = size + (size * range);
			return result < min ? min : result;
		}
	}
}
