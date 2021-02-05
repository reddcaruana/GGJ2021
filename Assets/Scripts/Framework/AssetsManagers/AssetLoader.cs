using UnityEngine;

namespace Assets.Scripts.AssetsManagers
{
	public abstract class AssetLoader
	{
		public static AssetLoader ME;

		protected AssetLoader()
		{
			ME = this;
		}

		public abstract T Loader<T>(string path) where T : Object;
	}
}
