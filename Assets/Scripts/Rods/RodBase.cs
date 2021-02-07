using System;
using UnityEngine;
using Assets.Scripts.Utils;
using Assets.Scripts.Views.Rods;
using Assets.Scripts.Controllers;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.AquaticCreatures;

namespace Assets.Scripts.Rods
{
	public abstract class RodBase
	{
		public abstract string NiceName { get; }
		private readonly RodFloat rodFloat = new RodFloat();
		private readonly RodNet rodNet;

		private Func<Vector3, IAquaticCreature> onCastComplete;

		public bool HasVeiw => view == null;
		private RodBaseView view;
		private IAquaticCreature potentialCatch;

		public RodBase()
		{
			rodNet = new RodNet(5, new Vector3(0f, -ViewController.Area.Height / 2f));
		}

		public void CreateView(Transform parent)
		{
			view = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<RodBaseView>("Prefabs/Rods/RodBaseView"), parent);
			view.Set(NiceName);
			view.SetPosition(rodNet.CenterWorldPos);
			rodFloat.CreateView(view.transform);
			DebugUtils.CreateDebugAreaView(rodNet.Radius, Color.red, "Net", -5, view.transform);
		}

		public void RegisterToOnCastComnplete(Func<Vector3, IAquaticCreature> callback) =>
			onCastComplete += callback;

		public void UnregisterFromOnCastComnplete(Func<Vector3, IAquaticCreature> callback) =>
			onCastComplete -= callback;

		public bool TryCast(Vector3 screenPosition)
		{
			if (rodFloat.IsCasted)
				return false;
			return Cast(screenPosition);
		}

		private bool Cast(Vector2 screenPosition)
		{
			if (!ViewController.CurrentSeason.ValidatePosition(screenPosition, out Vector3 worldPos))
				return false;

			rodFloat.Cast(worldPos, 0.5f, OnCastCompolete);
			
			void OnCastCompolete() => 
				CheckForPotentialCatch(onCastComplete?.Invoke(worldPos));
			
			return true;
		}

		private void CheckForPotentialCatch(IAquaticCreature creature)
		{
			if (creature == null)
				return;

			potentialCatch = creature;
			rodFloat.Nibble(6f, CatchWindow);
		}

		void CatchWindow()
		{
			rodFloat.CatchWindow(2f);
		}

		public void ReelIn()
		{
			if (!rodFloat.IsCasted)
				return;

			// Fish Escapes if you reel in while Nibbling
			if (rodFloat.IsNibbling)
			{
				potentialCatch.Escape();
				rodFloat.StopNibble();
				rodFloat.Reset();
				potentialCatch = null;
			}
			// Fish is Caught
			else if (rodFloat.CanCatch)
			{
				Debug.Log($"!!! Caught {potentialCatch.Data.Type.NiceName} !!!");
				rodFloat.Hooked();
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
				rodFloat.Reset();
			}
		}
	}
}
