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
		private Vector3 spawnLocalPosition;

		public bool HasView => view != null;
		private FishView view; 

		protected override void SetInternal()
		{
			proximityFear = CalculateProximityFear(Size);
			proximityBite = CalculateProximityBite(Size);

			if (HasView)
			{
				view.Set(this);
				view.CreateDebugAreaView(proximityFear, Color.yellow, "Fear");
				view.CreateDebugAreaView(proximityBite, Color.green, "Bite");
			}
		}

		public void CreateView(Transform parent) =>
			view = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<FishView>("Prefabs/Fish/FishBase"), parent);

		public void RegisterToCoolDownComplete(Action<FishController> callback) =>
			onCoolDownComplete += callback;

		public void UnregiisterFromCoolDownComplete(Action<FishController> callback) =>
			onCoolDownComplete -= callback;

		public bool InFearProximity(Vector2 localPosition) => InProximity(proximityFear / 2f, localPosition);
		public bool InBiteProximity(Vector2 localPosition) => InProximity(proximityBite / 2f, localPosition);

		public override void OnCast(Vector3 localPosition)
		{
			if (InFearProximity(localPosition))
				Escape();
			else if (InBiteProximity(localPosition))
				ApproachFloat(localPosition);
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
				view.Escape(null);

			CoolDown();
		}

		private void ApproachFloat(Vector2 floatLocalPosition)
		{
			Debug.Log("[FishController] Approach");
			isAvailable = false;

			if (HasView)
				view.ApprochFloat(floatLocalPosition);
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

		public override Vector3 GetPosition() => spawnLocalPosition;

		public void SetSpawnPoint(Vector3 localPosition)
		{
			spawnLocalPosition = localPosition;

			if (HasView)
				view.Reposition(localPosition, proximityFear, proximityBite);
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
