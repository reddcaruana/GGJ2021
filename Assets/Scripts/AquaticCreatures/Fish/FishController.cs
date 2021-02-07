using System;
using UnityEngine;
using Assets.Scripts.Utils;
using Assets.Scripts.Constants;
using Assets.Scripts.Views.Fish;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Framework;

namespace Assets.Scripts.AquaticCreatures.Fish
{
	public class FishController : AquaticCreature<FishController>
	{
		private const float COOL_DOWN = 6f;

		private FightingModule fightingModule;
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
				view.SetStartAndEnd(proximityFear, proximityBite);
				DebugAreaView();
			}
		}

		public void CreateView(Transform parent) =>
			view = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<FishView>("Prefabs/Fish/FishBase"), parent);

		public void RegisterToCoolDownComplete(Action<FishController> callback) =>
			onCoolDownComplete += callback;

		public void UnregiisterFromCoolDownComplete(Action<FishController> callback) =>
			onCoolDownComplete -= callback;

		public override Vector3 GetLocalSpawnPosition() => spawnLocalPosition;

		public bool InFearProximity(Vector2 localPosition) => InProximity(proximityFear / 2f, localPosition);
		public bool InBiteProximity(Vector2 localPosition) => InProximity(proximityBite / 2f, localPosition);

		public void SetSpawnPoint(Vector3 localPosition)
		{
			spawnLocalPosition = localPosition;

			if (HasView)
				view.Reposition(localPosition, proximityFear, proximityBite);
		}

		private static float CalculateProximityFear(float size)
		{
			const float MinFear = 2f;
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

		public override IAquaticCreature OnCast(Vector3 localPosition)
		{
			if (InFearProximity(localPosition))
				Escape();

			else if (InBiteProximity(localPosition))
				return this;

			return null;
		}

		protected override void AppearInternal()
		{
			if (HasView)
				view.Appear();
		}

		protected override void EscapeInternal()
		{
			if (HasView)
			{
				view.Escape(null);
				CleanDebugAreaView();
			}

			CoolDown();
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

		public override void ApproachFloatInternal(Vector2 floatLocalPosition)
		{
			if (HasView)
				view.ApprochFloat(floatLocalPosition);
		}

		public override void Fight()
		{
			if (HasView)
				view.SetManualOverride(true);
			fightingModule = new FightingModule(this);
		}

		public void ManualSwim(Vector3 newWorldPos)
		{
			if (HasView)
				view.ManualOverride(newWorldPos);
		}

		public override void ReelIn(float speed)
		{
			if (fightingModule == null)
				return;

			fightingModule.ReelIn(speed);
		}

		public Vector3 GetViewWorldPosition() => HasView ? view.GetFishWorldPosition() : Statics.VECTOR3_ZERO;

		[System.Diagnostics.Conditional("RDEBUG")]
		private void DebugAreaView()
		{
			if (!HasView)
				return;

			Color color = Color.yellow;
			color.a = 0.4f;
			DebugUtils.CreateDebugAreaView(proximityFear, color, "Fear", -1, view.transform);
			color = Color.green;
			color.a = 0.4f;
			DebugUtils.CreateDebugAreaView(proximityBite, color, "Bite", -2, view.transform);
		}

		[System.Diagnostics.Conditional("RDEBUG")]
		private void CleanDebugAreaView()
		{
			if (!HasView)
				return;

			MonoBehaviour.Destroy(view.transform.Find("Fear").gameObject);
			MonoBehaviour.Destroy(view.transform.Find("Bite").gameObject);
		}
	}
}
