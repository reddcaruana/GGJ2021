using UnityEngine;
using Assets.Scripts.Controllers;
using Assets.Scripts.Views.Seasons;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.Seasons
{
	public class StreamViewPoolObject : PoolObject<StreamView>
	{
		private const float SPEED_MIN = 1;
		private const float SPEED_MAX = 5;

		private static StreamView prefab;
		public static StreamView Prefab
		{
			get
			{
				if (prefab == null)
					prefab = AssetLoader.ME.Loader<StreamView>("Prefabs/Stream/StreamView");
				return prefab;
			}
		}

		private readonly float BoundY;
		private float speed;
		Vector3 targetPos;

		public StreamViewPoolObject() : base(MonoBehaviour.Instantiate(Prefab, null))
		{
			BoundY = View.Bounds().y;
		}

		public static void UnloadPrefab() => prefab = null;

		public override void Spawn(Transform parent)
		{
			base.Spawn(parent);

			Start();
			View.PlayAnimation();
			GameController.ME.RegisterToUpdate(OnUpdate);
		}

		public override void Despawn()
		{
			GameController.ME.UnregisterToUpdate(OnUpdate);
			View.StopAnimation();
			base.Despawn();
		}

		private void OnUpdate()
		{
			if (View.transform.position.y < ViewController.ScreenBottom.y)
				Start();

			View.transform.position = Vector3.MoveTowards(View.transform.position, targetPos, speed * Time.deltaTime);
		}

		private void Start()
		{
			ViewController.CurrentSeason.ParentToView(View.transform);

			speed = Random.Range(SPEED_MIN, SPEED_MAX);
			Vector3 initPos = ViewController.ScreenTop;
			initPos.y += BoundY;
			initPos.x = Random.Range(0, ViewController.CurrentSeason.GetVisualSize().x) - (ViewController.CurrentSeason.GetVisualSize().x / 2f);
			View.transform.position = initPos;
			targetPos = initPos;
			targetPos.y = -9999999;

		}
	}
}
