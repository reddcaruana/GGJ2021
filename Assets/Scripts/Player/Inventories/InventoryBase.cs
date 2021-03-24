using System;
using UnityEngine;
using Assets.Scripts.Constants;
using System.Collections.Generic;
using Assets.Scripts.AquaticCreatures.Fish;

namespace Assets.Scripts.Player.Inventories
{
	[Serializable]
	public abstract class InventoryBase<T>
		where T : FishLogDataBase
	{
		protected abstract string PrefKey { get; }

		[SerializeField] protected List<T> log = new List<T>();
		protected Action onEntryUpdate;

		public bool TryAdd(T data)
		{
			int index = IndexOf(data.Type);

			if (index == -1)
				log.Add(data);

			else if (!log[index].TryCombine(data))
				return false;

			Save();
			onEntryUpdate?.Invoke();
			return true;
		}

		public void RegisterToEntryUpdate(Action onEntryUpdate) => this.onEntryUpdate += onEntryUpdate;
		public void UnregisterToEntryUpdtae(Action onEntryUpdate) => this.onEntryUpdate -= onEntryUpdate;

		public bool TryGetData(FishTypeData data, out T result) => TryGetData(data.Type, out result);

		public bool TryGetData(FishType type, out T result)
		{
			int index = IndexOf(type);
			if (index == -1)
			{
				result = default;
				return false;
			}
			result = log[index];
			return true;
		}

		public int IndexOf(FishTypeData data) => IndexOf(data.Type);

		public int IndexOf(FishType type)
		{
			for (int i = 0; i < log.Count; i++)
			{
				if (log[i].Type.Type == type)
					return i;
			}

			return -1;
		}

		public T GetDataAt(int index) => log[index];

		public T[] ToArray() => log.ToArray();

		public void Save()
		{
			string json = ToJson();
			PlayerPrefs.SetString(PrefKey, json);
		}
		public string ToJson() => JsonUtility.ToJson(this);
	}
}
