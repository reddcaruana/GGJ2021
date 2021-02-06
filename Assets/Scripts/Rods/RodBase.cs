using UnityEngine;
using Assets.Scripts.Views.Rods;
using Assets.Scripts.Controllers;

namespace Assets.Scripts.Rods
{
	public abstract class RodBase
	{
		RodFloat rodFloat = new RodFloat();

		public bool HasVeiw => view == null;
		private RodBaseView view;

		public void CreateView()
		{

		}

		public void Cast(Vector2 screenPosition)
		{
			if (!GameController.ME.Current.ValidatePosition(screenPosition, out Vector3 worldPos))
				return;

			rodFloat.Cast(worldPos, 1f, null);
		}
	}
}
