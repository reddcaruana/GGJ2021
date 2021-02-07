using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Player;

namespace Assets.Scripts.Paywalls
{
	public class FishRequired
	{
		public readonly FishTypeData Type;
		public readonly int Required;
		public uint Obtained => Index == -1 ? 0 : PlayerData.LogBook.GetDataAt(Index).Total;
		public bool IsComplete => Obtained >= Required;
		
		private int index = -1;
		private int Index 
		{
			get
			{
				if (index == -1)
					index = PlayerData.LogBook.IndexOf(Type);
				return index;
			}
		}

		public FishRequired(FishTypeData type, int count)
		{
			Type = type;
			Required = count;
		}
	}
}