using UnityEngine;
using Assets.Scripts.Controllers;
using Assets.Scripts.Views.Seasons;
using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.Seasons
{
	public class StreamViewPoolObject : PoolObject<StreamView>
	{
		private const float SPEED_MIN = 1;
		private const float SPEED_MAX = 5;

		private static float SpawnTopY = -1f;
		private static float SpawnBottomY;
		private static float DespawnTop;
		private static float DespawnBottom;

		private static StreamView prefab;
		public static StreamView Prefab
		{
			get
			{
				if (prefab == null)

					prefab = AssetLoader.ME.Load<StreamView>("Prefabs/Stream/StreamView");
				return prefab;
			}
		}

		private float speed;
		Vector3 targetPos;

		public StreamViewPoolObject() : base(MonoBehaviour.Instantiate(Prefab, null))
		{
		}

		public static void UnloadPrefab() => prefab = null;

		public override void Spawn(Transform parent)
		{
			base.Spawn(parent);

			TryPrepare();

			StartFromTop();
			View.PlayAnimation();
			GameController.ME.RegisterToUpdate(OnUpdate);
		}

		private void TryPrepare() 
		{
			if (SpawnTopY != -1)
				return;

			SpawnTopY = ViewController.ScreenTop.y + View.Bounds().y;
			SpawnBottomY = ViewController.ScreenBottom.y;
			DespawnTop = SpawnTopY + View.Bounds().y;
			DespawnBottom = ViewController.ScreenBottom.y - (View.Bounds().y / 2f);
		}

		public override void Despawn()
		{
			GameController.ME.UnregisterToUpdate(OnUpdate);
			View.StopAnimation();
			base.Despawn();
		}

		private void OnUpdate()
		{
			if (View.transform.position.y < DespawnBottom)
				StartFromTop();
			else if (View.transform.position.y > DespawnTop)
				StartFromBottom();

			View.transform.position = Vector3.MoveTowards(View.transform.position, targetPos, speed * Time.deltaTime);
		}

		private void StartFromTop() => StartAt(SpawnTopY);

		private void StartFromBottom() => StartAt(SpawnBottomY);

		private void StartAt(float yPosition)
		{
			ViewController.CurrentSeason.ParentToView(View.transform);

			speed = Random.Range(SPEED_MIN, SPEED_MAX);
			Vector3 initPos = new Vector2()
			{
				x = Random.Range(0, ViewController.CurrentSeason.GetVisualSize().x) - (ViewController.CurrentSeason.GetVisualSize().x / 2f),
				y = yPosition
			};

			View.transform.position = initPos;
			targetPos = initPos;
			targetPos.y = -9999999;
		}
	}
}
