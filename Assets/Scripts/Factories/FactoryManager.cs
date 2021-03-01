namespace Assets.Scripts.Factories
{
	public static class FactoryManager
	{
		public static readonly FishFactory Fish = new FishFactory();
		public static readonly BottomDebriFactory BottomDebri = new BottomDebriFactory();
		public static readonly StreamFactory Stream = new StreamFactory();
		public static readonly TerrainFactory Terrain = new TerrainFactory();
	}
}
