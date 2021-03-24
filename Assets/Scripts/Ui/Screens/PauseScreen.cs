using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Player;
using Assets.Scripts.Constants;
using Assets.Scripts.Controllers;
using Assets.Scripts.AquaticCreatures.Fish;

namespace Assets.Scripts.Ui.Screens
{
	public class PauseScreen : TabScreen
	{
		public override int Index { get; } = (int)ScreenType.PauseMenu;

		protected override void Awake()
		{
			base.Awake();
			transform.Find("Debug/ButtonDeletePlayerData").GetComponent<Button>().onClick.AddListener(OnDeletePlayerDataButtonMsg);
			transform.Find("Debug/ButtonUnlockPayWall").GetComponent<Button>().onClick.AddListener(OnUnlockPayWallButtonMsg);
		}

		public override bool Show(Action onComplete = null)
		{
			GameController.ME.IsPaused = base.Show(onComplete);
			return GameController.ME.IsPaused;
		}

		public override bool Hide(Action onComplete = null)
		{
			GameController.ME.IsPaused = !base.Hide(onComplete);
			return !GameController.ME.IsPaused;
		}

		protected override void OnButtonMsg() => ScreenManager.ME.ToggleScreen(this);
		private void OnDeletePlayerDataButtonMsg()
		{
			PlayerPrefs.DeleteAll();
			Application.Quit();
		}

		private void OnUnlockPayWallButtonMsg()
		{
			if (ViewController.CurrentSeason.PayWall.AllComplete())
				return;

			FishLogData temp;
			for (int i = 0; i < ViewController.CurrentSeason.PayWall.Required.Length; i++)
			{
				temp = new FishLogData(
					ViewController.CurrentSeason.Type,
					ViewController.CurrentSeason.Alias,
					ViewController.CurrentSeason.PayWall.Required[i].Type,
					0,
					"",
					(uint)ViewController.CurrentSeason.PayWall.Required[i].Required,
					Rarity.Common,
					0);

				PlayerData.LogBook.TryAdd(temp);
				PlayerData.FishInventory.TryAdd(temp);
			}
		}


	}
}
