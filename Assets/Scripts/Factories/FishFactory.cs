using Assets.Scripts.Views.Fish;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.Factories
{
	public class FishFactory : RDOnDemandFactory<FishViewPoolObject, FishView>
	{
		protected override void UnloadPrefab() => FishViewPoolObject.UnloadPrefab();
	}
}
