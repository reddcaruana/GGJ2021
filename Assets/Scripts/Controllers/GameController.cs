using System;
using UnityEngine;
using Assets.Scripts.Rods;
using Assets.Scripts.Boats;
using Assets.Scripts.Framework.Tools;
using Assets.Scripts.Framework.Input;
using Assets.Scripts.Framework.Ui.SafeArea;

namespace Assets.Scripts.Controllers
{
	public class GameController : RDMonobehaviourSingleton<GameController>
	{
		public bool IsPaused { get; set; }
		public bool CanClick { get; set; } = true;
		public bool CanDrag { get; set; } = true;

		public Boat Boat { get; private set; }
		public RodBase Rod { get; private set; }
		public bool IsDragging { get; private set; }

		private Action onUpdate;
		private Action<bool> onPause;

		protected override void Awake()
		{
			base.Awake();
			MakePersistent();

			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = 60;

			GamePausableTime.Init();
			SafeAreaDetection.Init();

			GameFactory.Start();
		}

		private void Start()
		{
			ViewController.Init();

			Boat = new Boat();
			Boat.CreateView(ViewController.MainCamera.transform);

			Rod = new BasicRod();
			Rod.CreateView(Boat.View.transform);
			Rod.RegisterToOnCastComnplete(ViewController.OnCast);

			InputManager.onClickPosition += OnClick;

			InputManager.onDragStart += OnDragStart;
			InputManager.onDrag += OnDrag;
			InputManager.onDragEnd += OnDragEnd;

			if (!TutorialController.HasCompletedTutorials())
				new TutorialController().Init();
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
			if (!CanClick)
				return;

			Rod.ReelIn();
			Rod.TryCast(worldPosition);
		}

		private void OnDragStart(DragData data)
		{
			if (!CanDrag || Rod.InReserverdArea(data.WorldStartPosition))
				return;

			IsDragging = true;
			ViewController.FollowStart();
		}

		private void OnDrag(DragData data)
		{
			if (!CanDrag || data.FilteredDirection().y == 0)
				return;

			ViewController.FollowUpdate(data.AxisDistance.y * -1);
		}

		private void OnDragEnd(SwipeData data)
		{
			if (!CanDrag)
				return;

			IsDragging = false;

			ViewController.FollowStop();

			Vector3 filteredDirection = data.FilteredDirection();

			TryAccelerateBoat(filteredDirection, data);
		}

		private bool TryAccelerateBoat(Vector3 filteredDirection, SwipeData data)
		{
			if (filteredDirection.y == 0)
				return false;

			ViewController.Accelerate(((data.Distance / data.Time) * filteredDirection.y) * 0.01f);
			return true;
		}

		public void SetActiveInteractions(bool value)
		{
			CanClick = value;
			CanDrag = value;
		}
	}
}
