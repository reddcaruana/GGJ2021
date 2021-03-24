using UnityEngine;

namespace Assets.Scripts.Framework.AssetsManagers
{
	public class ResourceAssetLoader : AssetLoader
	{
		public override T Load<T>(string path) 
		{
			T t = Resources.Load<T>(path);

			if (t == null)
				Debug.LogError("[AssetLoader] Unable to Find: " + path);

			return t;
		}
	}
}
