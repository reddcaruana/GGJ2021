using System;
using UnityEngine;
using Assets.Scripts.Input;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.Framework.Input
{
    public static class InputManager
    {
		private static Camera mainCamera;
        public static Vector3 LastScreenPos { get; private set; }
		public static bool IsDragging => scrollManager.IsActive;

		public static Action<Vector3> onClickPosition;
		public static Action onClick;
		public static Action<DragData> onDragStart;
		public static Action<DragData> onDrag;
		public static Action<SwipeData> onDragEnd;

		private static readonly InputData inputData = new InputData();
		//private static readonly SwipeManager swipeManager = new SwipeManager();
		private static readonly ScrollManager scrollManager = new ScrollManager();

        public static void Init(Camera mainCamera)
		{
			InputManager.mainCamera = mainCamera;
			inputData.Player.Click.performed += ClickPosition;
			inputData.Player.Click.performed += Click;
			inputData.Player.PrimaryContact.started += PrimaryTouchStart;
			inputData.Player.PrimaryContact.canceled += PrimaryTouchEnd;

			scrollManager.onDragStart = OnDragStart;
			scrollManager.onDrag = OnDragDetection;
			scrollManager.onDragEnd = OnDragEnd;
		}

		public static void Enable(bool value)
		{
			if (value)
				inputData.Enable();
			else
				inputData.Disable();
		}

		public static void EnableGyro(bool value)
		{
			if (value)
				InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
			else
				InputSystem.DisableDevice(UnityEngine.InputSystem.Gyroscope.current);
		}

		public static Vector3 GetGyro() => inputData.Player.Gyro.ReadValue<Vector3>();

		public static void EnableAttitude(bool value)
		{
			if (value)
				InputSystem.EnableDevice(AttitudeSensor.current);
			else
				InputSystem.DisableDevice(AttitudeSensor.current);
		}

		public static Quaternion GetAttitude() => inputData.Player.Attitude.ReadValue<Quaternion>();

		public static Vector2 GetScreenPosition() => inputData.Player.Position.ReadValue<Vector2>();
		public static Vector3 GetWorldPosition() => mainCamera.ScreenToWorldPoint(GetScreenPosition());
		 
		private static void ClickPosition(CallbackContext context) => onClickPosition?.Invoke(GetWorldPosition());
		private static void Click(CallbackContext context) => onClick?.Invoke();

		private static void PrimaryTouchStart(CallbackContext context) => scrollManager.OnPrimaryContactStarted(GetWorldPosition(), Time.time);
		private static void PrimaryTouchEnd(CallbackContext context) => scrollManager.OnPrimaryContactEnded(GetWorldPosition(), Time.time);

		private static void OnDragStart(DragData data) => onDragStart?.Invoke(data);
		private static void OnDragDetection(DragData data) => onDrag?.Invoke(data);
		private static void OnDragEnd(SwipeData data) => onDragEnd?.Invoke(data);

		public static void DragUpdate() => scrollManager.OnPosition(GetWorldPosition());
	}

}
