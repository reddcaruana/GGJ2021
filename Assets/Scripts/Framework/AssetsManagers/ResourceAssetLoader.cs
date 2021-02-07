using UnityEngine;

namespace Assets.Scripts.AssetsManagers
{
	public class ResourceAssetLoader : AssetLoader
	{
		public override T Loader<T>(string path) 
		{
			T t = Resources.Load<T>(path);

			if (t == null)
				Debug.LogError("[AssetLoader] Unable to Find: " + path);

			return t;
		}
	}
}
