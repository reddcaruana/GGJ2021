using System;
using UnityEngine;

namespace Assets.Scripts.AquaticCreatures.Fish
{
	[Serializable]
	public class FishLogDataBase
	{
		[SerializeField] protected FishTypeData type;
		[SerializeField] protected uint total;

		public FishTypeData Type => type;
		public uint Total => total;

		public FishLogDataBase(FishTypeData type, uint total)
		{
			this.type = type;
			this.total = total;
		}

		public virtual bool TryCombine(FishLogDataBase b)
		{
			if (Type.Type != b.Type.Type)
				return false;

			total += b.total;
			return true;
		}


		public string ToJson() => JsonUtility.ToJson(this);

		public static FishLogDataBase FromJson(string json) => JsonUtility.FromJson<FishLogDataBase>(json);
	}
}
