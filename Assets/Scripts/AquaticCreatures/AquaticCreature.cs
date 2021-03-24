﻿using UnityEngine;
using Assets.Scripts.Generic;
using Assets.Scripts.Constants;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Controllers;

namespace Assets.Scripts.AquaticCreatures
{
	public abstract class AquaticCreature<T> : IAquaticCreature
		where T : AquaticCreature<T>
	{
		public FishData Data { get; private set; }
		public float Weight { get; private set; }
		public float Size { get; private set; }
		public Energy Energy { get; private set; }

		public bool IsReadyToSet { get; protected set; } = true;
		protected bool isAvailable;
		public bool CanCatch => !IsReadyToSet && isAvailable;

		public void Set(FishData data)
		{
			Data = data;
			Weight = Data.GetRandomWeight();
			Size = FishWiki.SizeCalculator(Weight);
			Energy = new Energy((Weight / Data.IdealWeight()) * Data.BaseEnergy, 0.5f);

			SetInternal();

			IsReadyToSet = false;
		}

		protected abstract void SetInternal();

		protected bool InProximity(float distance, Vector2 localPosition) =>
			MathUtils.IsInCircualarArea(GetLocalSpawnPosition(), distance, localPosition);

		public abstract Vector3 GetLocalSpawnPosition();

		public abstract IAquaticCreature OnCast(Vector3 floatPosition);

		public void Appear()
		{
			isAvailable = true;
			AppearInternal();
		}

		protected abstract void AppearInternal();

		public void Escape()
		{
			EscapeInternal();
		}

		protected abstract void EscapeInternal();

		public void ApproachFloat(Vector2 floatLocalPosition)
		{
			isAvailable = false;
			ApproachFloatInternal(floatLocalPosition);
		}

		public abstract void ApproachFloatInternal(Vector2 floatLocalPosition);

		public abstract void Fight();

		public abstract void ReelIn(float speed);

		public FishLogData Caught()
		{
			FishLogData data = new FishLogData(this, ViewController.CurrentSeason, (uint)(Random.value <= FishWiki.SHINY_CHANCE ? 1 : 0));
			CaughtInternal();
			return data;
		}

		protected abstract void CaughtInternal();

		public abstract Vector3 GetViewWorldPosition();
	}
}
