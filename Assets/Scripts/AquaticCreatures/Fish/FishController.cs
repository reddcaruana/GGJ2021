using System;
using UnityEngine;
using Assets.Scripts.Utils;
using Assets.Scripts.Constants;
using Assets.Scripts.Framework;
using Assets.Scripts.Controllers;
using Assets.Scripts.Framework.Utils;

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
		private FishPoolView view; 

		protected override void SetInternal()
		{
			proximityFear = CalculateProximityFear(Size);
			proximityBite = CalculateProximityBite(Size);

			if (HasView)
			{
				view.FishView.Set(this);
				view.FishView.SetStartAndEnd(proximityFear, proximityBite);
				DebugAreaView();
			}
		}

		public void CreateView(Transform parent)
		{
			view = FishFactory.Spawn();
			view.Spawn(parent);
		}

		public void ReleaseView()
		{
			if (!HasView)
				return;

			view.Despawn();
			view = null;
		}

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
				view.FishView.Reposition(localPosition, proximityFear, proximityBite);
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
				view.FishView.Appear();
		}

		protected override void EscapeInternal()
		{
			if (HasView)
				view.FishView.Escape(null);

			Reset();
		}

		private void SpawnCoolDown()
		{
			CoroutineRunner.Wait(COOL_DOWN, OnCoolDownComplete);

			void OnCoolDownComplete()
			{
				if (IsBaotFear())
				{
					SpawnCoolDown();
					return;
				}

				IsReadyToSet = true;
				onCoolDownComplete?.Invoke(this);
			}
		}

		protected override void CaughtInternal()
		{
			if (HasView)
				view.FishView.Caught();

			Reset();
		}

		private void Reset()
		{
			isAvailable = false;

			if (HasView)
				CleanDebugAreaView();

			if (fightingModule != null)
				fightingModule = null;

			SpawnCoolDown();
		}

		public override void ApproachFloatInternal(Vector2 floatLocalPosition)
		{
			if (HasView)
				view.FishView.ApprochFloat(floatLocalPosition);
		}

		public override void Fight()
		{
			if (HasView)
				view.FishView.SetManualOverride(true);
			fightingModule = new FightingModule(this, GameController.ME.Rod);
		}

		public void ManualSwim(Vector3 newWorldPos) => ManualSwim(newWorldPos, newWorldPos);

		public void ManualSwim(Vector3 newWorldPos, Vector3 lookWorldPos, bool isFaceAhead = true)
		{
			if (HasView)
			{
				view.FishView.ManualOverride(newWorldPos, lookWorldPos, isFaceAhead);

				if (Energy.IsResting)
					view.FishView.PlayStaticAnimation();
				else
					view.FishView.PlayEscapeAimation();
			}
		}

		public override void ReelIn(float speed)
		{
			if (fightingModule == null)
				return;

			fightingModule.ReelIn(speed);
		}

		public override Vector3 GetViewWorldPosition() => HasView ? view.FishView.GetFishWorldPosition() : Statics.VECTOR3_ZERO;

		public void FearOfBoats()
		{
			if (IsBaotFear())
				Escape();
		}

		private bool IsBaotFear() =>
			MathUtils.AreCirclesOverlaping(ViewController.ScreenBottom, SeasonScrollController.FEAR_OF_THE_BOAT, view.FishView.transform.position, proximityBite);

		[System.Diagnostics.Conditional("RDEBUG")]
		private void DebugAreaView()
		{
			if (!HasView)
				return;

			Color color = Color.yellow;
			color.a = 0.4f;
			DebugUtils.CreateDebugAreaView(proximityFear, color, "Fear", -1, view.FishView.transform);
			color = Color.green;
			color.a = 0.4f;
			DebugUtils.CreateDebugAreaView(proximityBite, color, "Bite", -2, view.FishView.transform);
		}

		[System.Diagnostics.Conditional("RDEBUG")]
		private void CleanDebugAreaView()
		{
			if (!HasView)
				return;

			MonoBehaviour.Destroy(view.FishView.transform.Find("Fear").gameObject);
			MonoBehaviour.Destroy(view.FishView.transform.Find("Bite").gameObject);
		}
	}
}
