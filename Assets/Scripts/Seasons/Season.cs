using UnityEngine;
using Assets.Scripts.Views.Seasons;
using Assets.Scripts.AssetsManagers;
using static Assets.Scripts.Constants.EnumList;

namespace Assets.Scripts.Seasons
{
	public class Season
	{
		public readonly SeasonAreaType Type;
		private SeasonView view;

		public Season(SeasonAreaType type)
		{
			Type = type;
		}

		public void CreateView(Transform parent)
		{
			view = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<SeasonView>("Prefabs/Seasons/SeasonView"), parent);
			view.Set();
		}
	}
}
