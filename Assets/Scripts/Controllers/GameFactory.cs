using Assets.Scripts.Ui.Screens;
using Assets.Scripts.Framework.Ui.Screens;
using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.Controllers
{
	public static class GameFactory
	{
		public static void Start()
		{
			AssetLoader assetLoader = new ResourceAssetLoader();
			RDSimpleScreenManager screenManager = new ScreenManager();
		}
	}
}
