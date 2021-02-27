using System;
using UnityEngine;
using Assets.Scripts.Utils;
using Assets.Scripts.Player;
using Assets.Scripts.Generic;
using Assets.Scripts.Constants;
using Assets.Scripts.Rods.Tilt;
using Assets.Scripts.Views.Rods;
using Assets.Scripts.Controllers;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.AquaticCreatures;
using Assets.Scripts.AquaticCreatures.Fish;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Rods
{
	public abstract class RodBase
	{
		public abstract Vector3 LineStartLocalPos { get; }
		public abstract string NiceName { get; }

		private readonly RodTiltHandler RodTiltHandler = RodTiltFactory.Create();
		private readonly RodLine RodLine = new RodLine();
		private readonly RodFloat RodFloat = new RodFloat();
		public readonly RodNet Net;
		public readonly Energy Energy;
		public PullState PullState = PullState.None;
		public bool IsCasted => RodFloat.IsCasted;
		public bool IsBusy => IsCasted && (RodFloat.IsNibbling || RodFloat.CanCatch || potentialCatch != null);
		public bool IsLineSlack { get; private set; }

		protected readonly float ReelInCoolDownTime = 0.4f;
		private float startedReelInCoolDownTime = 1f;
		private bool isCoolDown => Time.time - startedReelInCoolDownTime <= ReelInCoolDownTime;

		private Func<Vector3, IAquaticCreature> onCastComplete;

		public bool HasVeiw => view != null;
		private RodBaseView view;

		private IAquaticCreature potentialCatch;

		public RodBase()
		{
			Net = new RodNet(5);
			Energy = new Energy(20, 0.05f);
			RodFloat.Reset();
			RodTiltHandler.Init(PullLeft, PullRight, NoPull);
		}

		public void CreateView(Transform parent)
		{
			view = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<RodBaseView>("Prefabs/Rods/RodBaseView"), parent);
			view.Set(NiceName, LineStartLocalPos);
			view.SetPosition(Net.CenterWorldPos);
			RodLine.CreateView(view.transform);
			RodFloat.CreateView(view.transform);
			RodTiltHandler.Start();

			GameController.ME.RegisterToPause(OnPause);

			GameController.ME.RegisterToUpdate(OnGameControllerUpdate);

			DebugUtils.CreateDebugAreaView(Net.Radius, Color.red, "Net", -5, view.transform);
		}

		private void OnPause(bool pause)
		{
			if (pause)
				RodTiltHandler.Stop();
			else
				RodTiltHandler.Start();
		}

		public void RegisterToOnCastComnplete(Func<Vector3, IAquaticCreature> callback) =>
			onCastComplete += callback;

		public void UnregisterFromOnCastComnplete(Func<Vector3, IAquaticCreature> callback) =>
			onCastComplete -= callback;

		public bool TryCast(Vector3 worldPosition)
		{
			if (RodFloat.IsCasted || !ViewController.CanCast(worldPosition))
				return false;

			Cast(worldPosition);
			return true;
		}

		private void Cast(Vector3 worldPosition)
		{
			DebugUtils.Log("Casting ........./Z");
			RodFloat.Cast(worldPosition, 0.5f, OnCastCompolete);
			
			void OnCastCompolete()
			{
				if (!RodFloat.IsReseting)
					CheckForPotentialCatch(onCastComplete?.Invoke(worldPosition));
			}
		}

		private void CheckForPotentialCatch(IAquaticCreature creature)
		{
			if (creature == null)
				return;

			DebugUtils.Log($"Found Fish: {creature.Data.Type.NiceName}  ~~~~ >sSD");

			potentialCatch = creature;
			RodFloat.Nibble(CatchWindow);
			
			void CatchWindow() => RodFloat.CatchWindow(2f, FishEscaped);
		}


		public void ReelIn()
		{
			if (!RodFloat.IsCasted)
				return;

			// Fish Escapes if you reel in while Nibbling
			if (RodFloat.IsNibbling)
			{
				DebugUtils.Log("Reeled in while Nibbling ........./!");
				FishEscaped();
			}
			// Fish is Caught
			else if (RodFloat.CanCatch)
			{
				DebugUtils.Log("Hooked Fish ........./Z >sSD");

				RodFloat.Hooked();
				potentialCatch.Fight();
			}
			// Pull Catch
			else if (potentialCatch != null)
			{
				if (isCoolDown)
					return;

				DebugUtils.Log("Pull Fish ........./! >sSD");
				startedReelInCoolDownTime = Time.time;
				potentialCatch.ReelIn(-0.06f);
			}
			//Reel in float
			else
			{
				RodFloat.Reset();
			}
		}

		public void LineSlack(bool value)
		{
			IsLineSlack = value;
			RodLine.LineSlackWarning(value, 0.2f);
		}

		public void LineSnap()
		{
			RodTiltHandler.Stop();
			RodLine.LineSnap(OnSnapComplete);

			void OnSnapComplete()
			{
				RodTiltHandler.Start();
				FishEscaped(isInstant: true);
			}
		}

		public void FishEscaped() => FishEscaped(isInstant: false);

		public void FishEscaped(bool isInstant)
		{
			potentialCatch.Escape();
			Reset(isInstant);
		}

		public void CaughtFish(FishLogData fishLogData)
		{
			PlayerData.LogBook.TryAdd(fishLogData);
			ViewController.UiController.CaughtFish(fishLogData.Type);
			Reset(false);
		}

		private void Reset(bool isInstant)
		{
			DebugUtils.Log("reset Rod .........*/!*");

			potentialCatch = null;
			Energy.Reset();
			RodLine.Reset();

			if (isInstant)
				RodFloat.InstantReset();
			else
				RodFloat.Reset();
		}

		public void PullLeft() => Pull(PullState.Left);

		public void PullRight() => Pull(PullState.Right);

		public void NoPull() => Pull(PullState.None);

		private void Pull(PullState state)
		{
			PullState = state;

			if (HasVeiw)
			{
				float angle;
				switch (state)
				{
					case PullState.Left: angle = 45; break;
					case PullState.None: angle = 0; break;
					case PullState.Right: angle = -45; break;
					default: angle = 0; break;
				}
				view.Rotate(angle, 0.1f, flip: state == PullState.Right);
			}
		}

		private void OnGameControllerUpdate()
		{
			if (!HasVeiw)
				return;

			Vector3 endPosittion = potentialCatch == null || RodFloat.IsNibbling ? RodFloat.GetViewWorldPosition() : potentialCatch.GetViewWorldPosition();
			RodLine.PositionUpdate(view.GetLineStartWorldPosition(), endPosittion);
			RodLine.ProgressBarUpdate(1 - (Energy.Value / Energy.Max), Energy.IsResting, 0.5f);
		}
	}
}
