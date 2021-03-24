using UnityEngine;
using UnityEngine.U2D;

namespace Assets.Scripts.Framework.AssetsManagers
{
	public abstract class AssetLoader
	{
		public static AssetLoader ME;

		protected AssetLoader()
		{
			ME = this;
		}

		public abstract T Load<T>(string path) where T : Object;
		public Sprite LoadSprite(string name, SpriteAtlas atlas) => atlas.GetSprite(name);
	}
}
