using System;
using UnityEngine;
using Assets.Scripts.Utils;
using Assets.Scripts.Generic;
using Assets.Scripts.Constants;
using Assets.Scripts.Views.Rods;
using Assets.Scripts.Controllers;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.AquaticCreatures;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Player;

namespace Assets.Scripts.Rods
{
	public abstract class RodBase
	{
		public abstract string NiceName { get; }
		private readonly RodFloat RodFloat = new RodFloat();
		public readonly RodNet Net;
		public readonly Energy Energy;

		private Func<Vector3, IAquaticCreature> onCastComplete;

		public bool HasVeiw => view != null;
		private RodBaseView view;

		private IAquaticCreature potentialCatch;
		public PullState PullState = PullState.None;

		public RodBase()
		{
			Net = new RodNet(5);
			Energy = new Energy(20, 0.1f);
		}

		public void CreateView(Transform parent)
		{
			view = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<RodBaseView>("Prefabs/Rods/RodBaseView"), parent);
			view.Set(NiceName);
			view.SetPosition(Net.CenterWorldPos);
			RodFloat.CreateView(view.transform);
			DebugUtils.CreateDebugAreaView(Net.Radius, Color.red, "Net", -5, view.transform);
		}

		public void RegisterToOnCastComnplete(Func<Vector3, IAquaticCreature> callback) =>
			onCastComplete += callback;

		public void UnregisterFromOnCastComnplete(Func<Vector3, IAquaticCreature> callback) =>
			onCastComplete -= callback;

		public bool TryCast(Vector3 screenPosition)
		{
			if (RodFloat.IsCasted)
				return false;
			return Cast(screenPosition);
		}

		private bool Cast(Vector2 screenPosition)
		{
			if (!ViewController.CurrentSeason.ValidatePosition(screenPosition, out Vector3 worldPos))
				return false;

			RodFloat.Cast(worldPos, 0.5f, OnCastCompolete);
			
			void OnCastCompolete() => 
				CheckForPotentialCatch(onCastComplete?.Invoke(worldPos));
			
			return true;
		}

		private void CheckForPotentialCatch(IAquaticCreature creature)
		{
			if (creature == null)
				return;

			potentialCatch = creature;
			RodFloat.Nibble(6f, CatchWindow);
		}

		void CatchWindow()
		{
			RodFloat.CatchWindow(2f);
		}

		public void ReelIn()
		{
			if (!RodFloat.IsCasted)
				return;

			// Fish Escapes if you reel in while Nibbling
			if (RodFloat.IsNibbling)
			{
				potentialCatch.Escape();
				RodFloat.StopNibble();
				RodFloat.Reset();
				potentialCatch = null;
			}
			// Fish is Caught
			else if (RodFloat.CanCatch)
			{
				RodFloat.Hooked();
				potentialCatch.Fight();
			}
			// Pull Catch
			else if (potentialCatch != null)
			{
				potentialCatch.ReelIn(-0.06f);
			}
			//Reel in float
			else
			{
				RodFloat.Reset();
			}
		}

		public void FishEscaped()
		{
			RodFloat.Reset();
		}

		public void CaughtFish(FishLogData fishLogData)
		{
			PlayerData.LogBook.TryAdd(fishLogData);
			ViewController.UiController.CaughtFish(fishLogData.Type, onComplete: RodFloat.Reset);
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
	}
}
