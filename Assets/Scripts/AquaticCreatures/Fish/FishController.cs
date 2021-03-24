using System;
using UnityEngine;
using Assets.Scripts.Constants;
using Assets.Scripts.Framework;
using Assets.Scripts.Factories;
using Assets.Scripts.Controllers;
using Random = UnityEngine.Random;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.AquaticCreatures.Fish
{
	public class FishController : AquaticCreature<FishController>
	{
		private const float COOL_DOWN_MIN = 2f;
		private const float COOL_DOWN_MAX = 4f;

		private FightingModule fightingModule;
		private Action<FishController> onCoolDownComplete;
		private float proximityFear;
		private float proximityBite;
		private Vector3 spawnLocalPosition;

		public bool HasView => view != null;
		private FishViewPoolObject view;
		private Coroutine coolDownCoroutine;

		protected override void SetInternal()
		{
			proximityFear = CalculateProximityFear(Size);
			proximityBite = CalculateProximityBite(Size);

			if (HasView)
			{
				view.View.Set(this);
				view.View.SetStartAndEnd(proximityFear, proximityBite);
				DebugAreaView();
			}
		}

		public void CreateView(Transform parent)
		{
			view = FactoryManager.Fish.GetAvailable();
			view.Spawn(parent);
		}

		public void ReleaseView()
		{
			if (!HasView)
				return;

			if (coolDownCoroutine != null)
			{
				CoroutineRunner.HaltCoroutine(coolDownCoroutine);
				coolDownCoroutine = null;
			}

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
				view.View.Reposition(localPosition, proximityFear, proximityBite);
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
				view.View.Appear();
		}

		protected override void EscapeInternal()
		{
			if (HasView)
				view.View.Escape(null);

			Reset();
		}

		private void SpawnCoolDown()
		{
			coolDownCoroutine = CoroutineRunner.Wait(Random.Range(COOL_DOWN_MIN, COOL_DOWN_MAX), OnCoolDownComplete);

			void OnCoolDownComplete()
			{
				coolDownCoroutine = null;

				if (IsBoatFear())
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
				view.View.Caught();

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
				view.View.ApprochFloat(floatLocalPosition);
		}

		public override void Fight()
		{
			if (HasView)
				view.View.SetManualOverride(true);
			fightingModule = new FightingModule(this, GameController.ME.Rod);
		}

		public void ManualSwim(Vector3 newWorldPos) => ManualSwim(newWorldPos, newWorldPos);

		public void ManualSwim(Vector3 newWorldPos, Vector3 lookWorldPos, bool isFaceAhead = true)
		{
			if (HasView)
			{
				view.View.ManualOverride(newWorldPos, lookWorldPos, isFaceAhead);

				if (Energy.IsResting)
					view.View.PlayStaticAnimation();
				else
					view.View.PlayEscapeAimation();
			}
		}

		public override void ReelIn(float speed)
		{
			if (fightingModule == null)
				return;

			fightingModule.ReelIn(speed);
		}

		public override Vector3 GetViewWorldPosition() => HasView ? view.View.GetFishWorldPosition() : Statics.VECTOR3_ZERO;

		public void FearOfBoats()
		{
			if (IsBoatFear())
				Escape();
		}

		public bool IsBoatFear() =>
			!SeasonCinematicController.IsCinematic &&
			MathUtils.AreCirclesOverlaping(ViewController.ScreenBottom, SeasonScrollController.FEAR_OF_THE_BOAT, view.View.transform.position, proximityBite);

		public Vector3 GetTutorialCastPosition()
		{
			Vector3 result = GetLocalSpawnPosition();
			result.y -= (proximityFear / 2f) + (proximityBite / 4f);
			return view.View.transform.parent.TransformPoint(result);
		}

		[System.Diagnostics.Conditional("RDEBUG")]
		private void DebugAreaView()
		{
			if (!HasView)
				return;

			Color color = Color.yellow;
			color.a = 0.4f;
			RDebugUtils.CreateCircularDebugAreaView(proximityFear, color, "Fear", -1, view.View.transform);
			color = Color.green;
			color.a = 0.4f;
			RDebugUtils.CreateCircularDebugAreaView(proximityBite, color, "Bite", -2, view.View.transform);
		}

		[System.Diagnostics.Conditional("RDEBUG")]
		private void CleanDebugAreaView()
		{
			if (!HasView)
				return;

			MonoBehaviour.Destroy(view.View.transform.Find("Fear").gameObject);
			MonoBehaviour.Destroy(view.View.transform.Find("Bite").gameObject);
		}
	}
}
