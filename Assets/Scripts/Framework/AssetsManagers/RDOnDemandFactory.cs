using UnityEngine;
using Assets.Scripts.Utils;

namespace Assets.Scripts.Framework.AssetsManagers
{
	public abstract class RDOnDemandFactory<T, V>
		where T : PoolObject<V>, new()
		where V : MonoBehaviour
	{
		private T[] pool;
		public void Init(int totalCount) =>
			pool = new T[totalCount];

		public T GetAvailable()
		{
			for (int i = 0; i < pool.Length; i++)
			{
				if (pool[i] == null)
				{
					pool[i] = new T();

					if (i == pool.Length - 1)
						UnloadPrefab();

					return pool[i];
				}
				else if (!pool[i].IsSpawned)
					return pool[i];
			}

			DebugUtils.LogError($"[Factory] Ran out of {typeof(V)}.. This Should NEVER Happen");
			return null;
		}

		protected abstract void UnloadPrefab();
	}
}
