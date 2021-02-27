using UnityEngine;

namespace Assets.Scripts.Framework.AssetsManagers
{
	public abstract class PoolObject<T>
		where T : MonoBehaviour
	{
		public bool IsSpawned { get; private set; }
		public T View { get; private set; }

		protected PoolObject(T view)
		{
			View = view;
			Despawn();
		}

		public void Spawn(Transform parent, Vector3 position, bool isLocalPos = true)
		{
			Spawn(parent);

			if (isLocalPos)
				View.transform.localPosition = position;
			else
				View.transform.position = position;

		}

		public virtual void Spawn(Transform parent)
		{
			IsSpawned = true;

			if (parent != null)
				View.transform.SetParent(parent);
			View.gameObject.SetActive(IsSpawned);
		}

		public virtual void Despawn()
		{
			IsSpawned = false;
			View.gameObject.SetActive(IsSpawned);
		}
	}
}
