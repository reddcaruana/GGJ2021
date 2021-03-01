using System;
using UnityEngine;
using Assets.Scripts.Rods;
using Assets.Scripts.Framework;
using Assets.Scripts.Framework.Tools;
using Assets.Scripts.Framework.Input;

namespace Assets.Scripts.Controllers
{
	public class GameController : RDMonobehaviourSingleton<GameController>
	{
		public RodBase Rod { get; private set; }
		public bool IsDragging { get; private set; }

		private Action onUpdate;
		private Action<bool> onPause;

		protected override void Awake()
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = 60;

			base.Awake();
			MakePersistent();

			GameFactory.Start();
		}

		private void Start()
		{
			ViewController.Init();

			Rod = new BasicRod();
			Rod.CreateView(ViewController.MainCamera.transform.Find("Boat"));
			Rod.RegisterToOnCastComnplete(ViewController.OnCast);

			InputManager.onClickPosition += OnClick;

			InputManager.onDragStart += OnDragStart;
			InputManager.onDrag += OnDrag;
			InputManager.onDragEnd += OnDragEnd;
		}

		private void Update()
		{
			onUpdate?.Invoke();

			if (InputManager.IsDragging)
				InputManager.DragUpdate();
		}
		public void RegisterToUpdate(Action callback) => onUpdate += callback;
		public void UnregisterToUpdate(Action callback) => onUpdate -= callback;

		private void OnApplicationPause(bool pause) => onPause?.Invoke(pause);
		public void RegisterToPause(Action<bool> callback) => onPause += callback;
		public void UnregisterToPause(Action<bool> callback) => onPause -= callback;

		private void OnClick(Vector3 worldPosition)
		{
			Rod.ReelIn();
			Rod.TryCast(worldPosition);
		}

		private void OnDragStart(DragData data)
		{
			IsDragging = true;
			ViewController.FollowStart();
		}

		private void OnDrag(DragData data)
		{
			if (data.FilteredDirection().y == 0)
				return;

			ViewController.FollowUpdate(data.AxisDistance.y * -1);
		}

		private void OnDragEnd(SwipeData data)
		{
			IsDragging = false;
			ViewController.FollowStop();

			Vector3 filteredDirection = data.FilteredDirection();

			if (!TryAccelerateBoat(filteredDirection, data) && !Rod.IsBusy)
				ShowHideLogBook(filteredDirection);
		}

		private bool TryAccelerateBoat(Vector3 filteredDirection, SwipeData data)
		{
			if (filteredDirection.y == 0)
				return false;

			ViewController.Accelerate(((data.Distance / data.Time) * filteredDirection.y) * 0.01f);
			return true;
		}

		private void ShowHideLogBook(Vector3 filteredDirection)
		{
			if (filteredDirection.x == 0)
				return;

			ViewController.UiController.ShowHideLogBook(filteredDirection.x < 0);
		}
	}
}
