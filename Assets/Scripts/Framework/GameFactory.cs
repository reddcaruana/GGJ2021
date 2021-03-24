using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.Framework
{
	public static class GameFactory
	{
		public static void Start()
		{
			AssetLoader assetLoader = new ResourceAssetLoader();
		}
	}
}
