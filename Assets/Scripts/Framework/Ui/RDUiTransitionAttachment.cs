﻿using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Scripts.Framework.Ui
{
	public class RDUiTransitionAttachment
	{
		public enum State { Hidden, HideTransition, ShowTransition, Visible }

		private RectTransform rectTransform;
		public float showTime = 0.5f;
		public float hideTime = 0.5f;

		public bool isMovable;
		public Vector2 showPos;
		public Vector2 hidePos;

		public bool disableOnHide;

		public State CurrentState { get; private set; }
		public bool IsInMotion => CurrentState == State.HideTransition || CurrentState == State.ShowTransition;

		public virtual void Init(RectTransform target, State initState, bool autoGrabMissingPosFromAwake = true)
		{
			rectTransform = target;
			CurrentState = initState;

			if (autoGrabMissingPosFromAwake)
			{
				if (CurrentState == State.Visible)
					showPos = rectTransform.anchoredPosition;
				else if (CurrentState == State.Hidden)
					hidePos = rectTransform.anchoredPosition;
			}
		}

		public bool Show(Action onComplete = null) =>
			ShowOrHide(State.Visible, State.ShowTransition, State.HideTransition, showPos, showTime, onComplete);

		public bool Hide(Action onComplete = null) =>
			ShowOrHide(State.Hidden, State.HideTransition, State.ShowTransition, hidePos, hideTime, onComplete);

		private bool ShowOrHide(State targetState, State targetTransitionState, State oppositeTransitionState, Vector2 targetPos, float time, Action onComplete)
		{
			if (CurrentState == targetState || CurrentState == targetTransitionState)
				return false;

			if (CurrentState == oppositeTransitionState)
				StopCurrentTransition();
			
			else if (disableOnHide && targetState == State.Visible)
				rectTransform.gameObject.SetActive(true);

			CurrentState = targetTransitionState;
			OnStartTransition();

			if (isMovable)
				rectTransform.DOAnchorPos(targetPos, time).OnComplete(() =>
				{
					OnDoneTransition(targetState, onComplete);
				});

			return true;
		}

		public bool ShowInstantly() => ShowOrHideInstantly(State.Visible, showPos);
		public bool HideInstantly() => ShowOrHideInstantly(State.Hidden, hidePos);

		private bool ShowOrHideInstantly(State targetState, Vector2 targetPos)
		{
			if (CurrentState == targetState)
				return false;

			StopCurrentTransition();

			if (isMovable)
				rectTransform.anchoredPosition = targetPos;
			
			OnDoneTransition(targetState, null);
			return true;
		}

		protected virtual void OnStartTransition()
		{
		}

		protected virtual void OnDoneTransition(State newState, Action onComplete)
		{
			CurrentState = newState;

			if (disableOnHide && CurrentState == State.Hidden)
				rectTransform.gameObject.SetActive(false);

			else if (disableOnHide && CurrentState == State.Visible)
				rectTransform.gameObject.SetActive(true);

			onComplete?.Invoke();
		}

		void StopCurrentTransition() =>
			rectTransform.DOKill();
	}
}
