using System;
using UnityEngine;
using Assets.Scripts.Views.Rods;
using Assets.Scripts.Controllers;
using Assets.Scripts.AssetsManagers;

namespace Assets.Scripts.Rods
{
	public abstract class RodBase
	{
		private Action<Vector3> onCastComplete;
		private RodFloat rodFloat = new RodFloat();

		public bool HasVeiw => view == null;
		private RodBaseView view;

		public void CreateView(Transform parent)
		{
			view = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<RodBaseView>("Prefabs/Rods/RodBaseView"), parent);
			view.SetPosition(new Vector3(0f, -ViewController.Height / 2f));
			rodFloat.CreateView(view.transform);
		}

		public void RegisterToOnCastComnplete(Action<Vector3> callback) =>
			onCastComplete += callback;

		public void UnregisterFromOnCastComnplete(Action<Vector3> callback) =>
			onCastComplete -= callback;

		public bool TryCast(Vector3 screenPosition)
		{
			if (rodFloat.IsCasted)
				return false;
			return Cast(screenPosition);
		}

		private bool Cast(Vector2 screenPosition)
		{
			if (!GameController.ME.CurrentSeason.ValidatePosition(screenPosition, out Vector3 worldPos))
				return false;

			rodFloat.Cast(worldPos, 1f, OnCastCompolete);
			void OnCastCompolete() => onCastComplete?.Invoke(worldPos);
			return true;
		}

	}
}
