using UnityEngine;

namespace Assets.Scripts.AssetsManagers
{
	public class ResourceAssetLoader : AssetLoader
	{
		public override T Loader<T>(string path) 
		{
			return Resources.Load<T>(path);
		}
	}
}
