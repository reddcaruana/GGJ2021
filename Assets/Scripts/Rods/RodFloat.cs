﻿using System;
using UnityEngine;
using Assets.Scripts.Views.Rods;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Rods
{
	public class RodFloat
	{
	 	public Vector3 WorldPosition { get; private set; }

		public bool HasView => view != null;
		RodFloatView view;

		public void CreateView(Transform parent)
		{
			view = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<RodFloatView>("Prefabs/Rods/RodFloatBase"), parent);
		}

		public void Cast(Vector3 worldPos, float duration, Action onComplete)
		{
			WorldPosition = worldPos;

			if (HasView)
				view.Cast(worldPos, duration, onComplete);
			else
				CoroutineRunner.Wait(duration, onComplete);
		}
	}
}
