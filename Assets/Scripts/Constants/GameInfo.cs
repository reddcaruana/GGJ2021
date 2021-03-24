using Assets.Scripts.Player;
using Assets.Scripts.Ui.Screens;

namespace Assets.Scripts.Constants
{
	public static class GameInfo
	{
		public const string VERSION = "v0.1.20";
		public const string CHANGE_LOG =
			"- ";

		public static void VersionCheck()
		{
			if (!PlayerData.IsPlayingNewVersion)
				return;

			ModalScreen.ShowModalDialog("Warning", $"New Version detected: {VERSION} vs last played {PlayerData.LastPlayedVersion}.\n\n" +
				"** Things Have Changed in Players Data **\n\n" +
				"DO NOT PLAY IF NOT DELETED\n\n" +
				"BTW after Successfully Deleting Data, this Dialog will reappear. Ignore...\n\n" +
				"More Info\n- Long Press App Icon.\n- App Info\n- Storage & cache\n-Clear Storage");
		}
	}
}

/* /////// History \\\\\\\\
 * ----- V0.1.19
			"- Implemented Game Pause\n" +
			"- Implemented Tutorials\n";
 *
 * ----- V0.1.18
			"- Log Book now displays amount of fish in inventory\n" +
			"- Paywall no longer depends on the logbook but instead on the inventory\n" +
			"- Unlocking a paywall will now subtract fish from your inventory\n" +
			"- Outro Paywall cinematic now waits for remaining required fish to be notified before opening\n" +
			"- Fish now spawn based on Rarity\n" +
			"- Fixed steam lines animation when scrolling back down\n" +
			"- Fixed issue during cinematic where paywall will jump over or not load\n" +
			"- Introduced Two new buttons in the Pause screen for quick debugging:\n" +
			"-- Delete Player Data\n" +
			"-- Complete Current Paywall\n" +
			"- Balanced Fight Module:\n" +
			"-- If Fish is resting, rod recovers all energy\n" +
			"-- If fish is resting rod does not lose energy\n" +
			"-- If fish is resting acceleration in favor of the fish is reseted\n" +
			"-- Fish now get tired!\n" +
			"-- After Each resting period, Maximum fish Energy is reduced by 20%" +
			"- Named Fish\n" +
			"- Balanced Fish weight and force\n" +
			"- limited AMximum and Minimum fish size vs weight\n" +
			"- Improved scrolling acceleration\n" +
			"- Fixed issue with screen not scrolling if faster then 0.2sec";
 * 
 * ----- V0.1.17
 * 			"- Fixed issue with outro cinematic showing every catch\n" +
			"- Implemented Bounce animation for just caught requirements when approaching a paywall\n" +
			"- Implemented Bounce animation for newly caught fish in Log Book\n" +
			"- Fixed issue with scrolling while transitioning to another season\n" +
			"- Removed redundant buttons from main menu\n" +
			"- Updated Close Button\n" +
			"- Added Glow to Shiny and newly caught fish in the Caught Animation\n";
 * 
 */
