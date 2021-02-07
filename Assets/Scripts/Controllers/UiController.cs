using System;
using UnityEngine;
using Assets.Scripts.Ui;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.AquaticCreatures.Fish;

namespace Assets.Scripts.Controllers
{
	public class UiController
	{
		private Canvas canvas;
		CaughtFishScreen caughtFishScreen;

		public void Init()
		{
			canvas = GameObject.FindObjectOfType<Canvas>();
			LoadCaughtScreen();
		}

		private void LoadCaughtScreen()
		{
			GameObject objPrefab = AssetLoader.ME.Loader<GameObject>("Prefabs/Ui/CaughtFishScreen");
			caughtFishScreen = MonoBehaviour.Instantiate(objPrefab, canvas.transform).AddComponent<CaughtFishScreen>();
		}

		public void CaughtFish(FishTypeData data, Action onComplete = null) => caughtFishScreen.Show(data, onComplete);
	}
}
