namespace Assets.Scripts.Controllers
{
	public static class GamePausableTime
	{
		public static float time { get; private set; }

		public static void Init()
		{
			GameController.ME.RegisterToUpdate(TimeUpdate);
		}

		private static void TimeUpdate()
		{
			if (GameController.ME.IsPaused)
				return;
			time += UnityEngine.Time.deltaTime;
		}
	}
}
