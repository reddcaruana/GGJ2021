using UnityEngine;
using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.Constants
{
	public static class MaterialStatics
	{
		public static readonly Material STANDARD;
		public static readonly Material OVERLAY;
		public static readonly Material SHINY;
		static MaterialStatics()
		{
			STANDARD = AssetLoader.ME.Load<Material>("Materials/Standard_mat");
			OVERLAY = AssetLoader.ME.Load<Material>("Materials/ColorOverlay_mat");
			SHINY = AssetLoader.ME.Load<Material>("Materials/Shiny_mat");
		}
	}
}
