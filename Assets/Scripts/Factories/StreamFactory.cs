using UnityEngine;
using Assets.Scripts.Seasons;
using Assets.Scripts.Views.Seasons;
using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.Factories
{
	public class StreamFactory : RDOnDemandFactory<StreamViewPoolObject, StreamView>
	{
		protected override void UnloadPrefab() => StreamViewPoolObject.UnloadPrefab();

		public void StartStream(int count, Transform parent)
		{
			StreamViewPoolObject temp;
			for (int i = 0; i < count; i++)
			{
				temp = GetAvailable();
				temp.Spawn(parent);
			}
		}
	}
}
