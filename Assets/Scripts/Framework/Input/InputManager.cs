using System;
using UnityEngine;
using Assets.Scripts.Input;
using UnityEngine.InputSystem;
using Assets.Scripts.Framework.Utils;
using static UnityEngine.InputSystem.InputAction;
using Assets.Scripts.Controllers;

namespace Assets.Scripts.Framework.Input
{
    public static class InputManager
    {
		private static Camera mainCamera;
        public static Vector3 LastScreenPos { get; private set; }
        
		public static Action<Vector3> onClickPosition;
		public static Action onClick;
		public static Action<SwipeData> onSwipeDetaction;
		public static Action<bool> onHoldDetaction;

		private static readonly InputData inputData = new InputData();
		private static readonly SwipeManager swipeManager = new SwipeManager();

        public static void Init(Camera mainCamera)
		{
			InputManager.mainCamera = mainCamera;
			inputData.Player.Click.performed += ClickPosition;
			inputData.Player.Click.performed += Click;
			inputData.Player.PrimaryContact.started += PrimaryTouchStart;
			inputData.Player.PrimaryContact.canceled += PrimaryTouchEnd;
			inputData.Player.Hold.started += HoldStart;
			inputData.Player.Hold.canceled += HoldEnd;

			swipeManager.onSwipe = OnSwipeDetection;
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

		public static Vector2 GetScreenPosition() => inputData.Player.Position.ReadValue<Vector2>();
		public static Vector3 GetWorldPosition() => mainCamera.ScreenToWorldPoint(GetScreenPosition());
		public static Vector3 Gyro() => inputData.Player.Gyro.ReadValue<Vector3>();
		 
		private static void ClickPosition(CallbackContext context) => onClickPosition?.Invoke(GetWorldPosition());
		private static void Click(CallbackContext context) => onClick?.Invoke();

		private static void PrimaryTouchStart(CallbackContext context) => swipeManager.OnPrimaryContactStarted(GetWorldPosition(), Time.time);
		private static void PrimaryTouchEnd(CallbackContext context) => swipeManager.OnPrimaryContactEnded(GetWorldPosition(), Time.time);

		private static void OnSwipeDetection(SwipeData data) => onSwipeDetaction?.Invoke(data);
		private static void HoldStart(CallbackContext context) => onHoldDetaction(true);
		private static void HoldEnd(CallbackContext context) => onHoldDetaction(false);
	}

}
