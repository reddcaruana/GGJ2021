using System;
using DG.Tweening;
using UnityEngine;
using Assets.Scripts.Seasons;
using Assets.Scripts.Views.Seasons;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Framework.Tools;
using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.Controllers
{
	/*
	 * // How should it work \\
	 * 
	 * - loads last unlocked screen or the one player closed the app on.
	 * - if current season bottom + screen view + half screen view > screen view bottom, load Previous season.
	 * - if current season top - screen view - half screen view < screen view Center, load Next season.
	 * -- if season view is already loaded and contains the same proposed season do not reload.
	 * - if upcoming season is unlocked allow scrolling into it.
	 * - if Cast is into Non current season and is unlocked switch current season. (Make no mistake between current season and currentView index).
	 * -- View index is only controlled when scrolling and loading new views.
	 * - if screen view bottom is in non current season (no cats happened) switch current season.
	 * 
	 */

	public class SeasonScrollController
	{
		public const float FEAR_OF_THE_BOAT = 1f;

		private const float FOLLOW_TRASH_HOLD = 0.01f;
		private const float FOLLOW_FACTOR = 0.1f;

		private readonly RDAccelerationModule acceleration;
		private readonly SeasonView[] seasonViews = new SeasonView[2];
		public bool IsMoving => acceleration.IsMoving || IsFollowing;

		private Area2D PayWallNotificationArea;

		private int currentViewIndex = 0;
		private int NextViewIndex => MathUtils.LoopIndex(currentViewIndex + 1, seasonViews.Length);
		private Season otherSeason;

		public bool IsFollowing => GameController.ME.IsDragging && ViewController.CanMove();
		private Vector3 followWorldTarget;

		public SeasonScrollController()
		{
			Vector3 boatWorldPosition = ViewController.MainCamera.transform.position;
			boatWorldPosition.y -= ViewController.Area.HalfHeight;



			acceleration = new RDAccelerationModule(MoveUpdate, GetWorldPosition);
			acceleration.SetBounds(CheckPosition);
			acceleration.SetSpeedVectorTrashHold(FOLLOW_TRASH_HOLD);
		}

		public void Init()
		{
			PayWallNotificationArea = new Area2D(
				width: ViewController.Area.Width,
				height: ViewController.Area.Height * 0.05f,
				center: new Vector2(0, ViewController.Area.HalfHeight - (ViewController.Area.Height * 0.1f)));
			RDebugUtils.CreateRectangularDebugAreaView(PayWallNotificationArea, new Color(1f,0f,1f,0.4f), "PayWallnotificationArea", 5, ViewController.MainCamera.transform);

			SeasonView prefab = AssetLoader.ME.Load<SeasonView>("Prefabs/Seasons/SeasonView");

			seasonViews[0] = MonoBehaviour.Instantiate(prefab, ViewController.MainParent);
			seasonViews[1] = MonoBehaviour.Instantiate(prefab, ViewController.MainParent);

			ViewController.CurrentSeason.AssignView(seasonViews[currentViewIndex], ViewController.ScreenBottom);
			otherSeason = ViewController.PeekNextSeason();
			otherSeason.AssignView(seasonViews[NextViewIndex], seasonViews[currentViewIndex].GetTopWorldPosition());

			acceleration.IsActive = true;

			followWorldTarget = seasonViews[currentViewIndex].transform.position;
		}

		public void FollowStart()
		{
			if (!ViewController.TryResetRodAndMove())
				return;

			acceleration.IsActive = false;
			followWorldTarget = seasonViews[currentViewIndex].transform.position;
		}

		public void FollowStop()
		{
			acceleration.ResetSpeed();
			acceleration.IsActive = true;
		}

		public void FollowUpdate(float followDistance)
		{
			if (!IsFollowing)
				return;

			if (!MathUtils.InRangeFloat(followDistance, -FOLLOW_TRASH_HOLD, FOLLOW_TRASH_HOLD))
				followWorldTarget.y += followDistance;

			MoveUpdate(CheckPosition(Vector3.Lerp(seasonViews[currentViewIndex].transform.position, followWorldTarget, FOLLOW_FACTOR)));
		}

		public void SetSpeed(float speed)
		{
			if (!ViewController.TryResetRodAndMove())
				return;

			acceleration.SetSpeed(speed);
		}

		private void MoveUpdate(Vector3 worldPosition)
		{

			seasonViews[currentViewIndex].transform.position = worldPosition;

			int nextViewIndex = NextViewIndex;

			if (seasonViews[nextViewIndex].transform.position.y > seasonViews[currentViewIndex].transform.position.y)
				seasonViews[nextViewIndex].PositionFromBase(seasonViews[currentViewIndex].GetTopWorldPosition());
			else
				seasonViews[nextViewIndex].PositionFromTop(seasonViews[currentViewIndex].GetBottomWorldPosition());

			ViewController.CurrentSeason.OnBoatMovement();
		}

		private Vector3 GetWorldPosition() => seasonViews[currentViewIndex].transform.position;

		private Vector3 CheckPosition(Vector3 newPosition)
		{
			newPosition = SeasonAreaRestriction(newPosition);

			if (TryChnageCurrentSeason())
			{
				Vector3 currentPos = seasonViews[NextViewIndex].transform.position;
				newPosition = seasonViews[currentViewIndex].transform.position + (newPosition - currentPos);

				if (IsFollowing)
				{
					float followDistance = newPosition.y - followWorldTarget.y;
					followWorldTarget = newPosition;
					followWorldTarget.y += followDistance;
				}
			}

			TryLoadUpcomingSeasonView(newPosition);

			TryPayWallNotification(newPosition);

			return newPosition;
		}

		private Vector3 SeasonAreaRestriction(Vector3 newPosition)
		{
			float top;
			float bottom;

			int nextSeasonViewIndex = NextViewIndex;

			Vector3 GetNextSeasonCenterWorldPosition() =>
				seasonViews[nextSeasonViewIndex].transform.position + (newPosition - seasonViews[currentViewIndex].transform.position);

			if (seasonViews[currentViewIndex].transform.position.y > seasonViews[nextSeasonViewIndex].transform.position.y)
			{
				top = seasonViews[currentViewIndex].GetTopWorldPosition(newPosition).y;

				if (otherSeason.PayWall.AllComplete())
					bottom = seasonViews[nextSeasonViewIndex].GetBottomWorldPosition(GetNextSeasonCenterWorldPosition()).y;
				else
					bottom = seasonViews[currentViewIndex].GetBottomWorldPosition(newPosition).y;
			}
			else
			{
				if (ViewController.CurrentSeason.PayWall.AllComplete())
					top = seasonViews[nextSeasonViewIndex].GetTopWorldPosition(GetNextSeasonCenterWorldPosition()).y;
				else
					top = seasonViews[currentViewIndex].GetTopWorldPosition(newPosition).y;

				bottom = seasonViews[currentViewIndex].GetBottomWorldPosition(newPosition).y;
			}

			// y = 0 (center of screen) to allow Paywalls come down in view
			if (top > 0 && bottom < ViewController.ScreenBottom.y)
				return newPosition;

			followWorldTarget = seasonViews[currentViewIndex].transform.position;
			return followWorldTarget;
		}

		private bool TryChnageCurrentSeason()
		{
			int nextSeasonViewIndex = NextViewIndex;
			bool isPrev;

			if (seasonViews[currentViewIndex].transform.position.y > seasonViews[nextSeasonViewIndex].transform.position.y &&
				seasonViews[nextSeasonViewIndex].GetTopWorldPosition().y > ViewController.Area.HalfHeight)
			{
				isPrev = true;
			}

			else if (seasonViews[currentViewIndex].transform.position.y < seasonViews[nextSeasonViewIndex].transform.position.y &&
				seasonViews[nextSeasonViewIndex].GetBottomWorldPosition().y < 0)
			{
				isPrev = false;
			}
			else
				return false;

			otherSeason = ViewController.CurrentSeason;
			followWorldTarget.y = seasonViews[nextSeasonViewIndex].transform.position.y + (seasonViews[currentViewIndex].transform.position.y - followWorldTarget.y);

			if (isPrev)
				ViewController.PrevSeason();
			else
				ViewController.NextSeason();

			currentViewIndex = nextSeasonViewIndex;

			return true;
		}

		private void TryLoadUpcomingSeasonView(Vector3 newPosition)
		{
			int nextSeasonIndex = NextViewIndex;
			Season upcomingSeason = null;
			Vector3 basePosition;
			float screenLoadingHeight = ViewController.Area.Height + ViewController.Area.HalfHeight;

			if (seasonViews[currentViewIndex].GetTopWorldPosition(newPosition).y - screenLoadingHeight < ViewController.Area.HalfHeight)
			{
				upcomingSeason = ViewController.PeekNextSeason();
				basePosition = seasonViews[currentViewIndex].GetTopWorldPosition(newPosition);
			}
			else if (ViewController.CurrentSeason.Type != Constants.SeasonAreaType.One &&
				seasonViews[currentViewIndex].GetBottomWorldPosition(newPosition).y + screenLoadingHeight > -ViewController.Area.HalfHeight)
			{
				upcomingSeason = ViewController.PeekPrevSeason();
				basePosition = seasonViews[currentViewIndex].GetBottomWorldPosition(newPosition);
				basePosition.y -= upcomingSeason.GetVisualSize().y;
			}
			else
				basePosition = default;

			if (upcomingSeason != null && !upcomingSeason.CompareView(seasonViews[nextSeasonIndex]))
			{
				otherSeason.ReleaseViews();
				otherSeason = upcomingSeason;
				upcomingSeason.AssignView(seasonViews[nextSeasonIndex], basePosition);
			}
		}

		private void TryPayWallNotification(Vector3 newPosition) 
		{
			if (MathUtils.IsInRectArea(PayWallNotificationArea, seasonViews[currentViewIndex].GetTopWorldPosition(newPosition)))
			{
				ViewController.CurrentSeason.PayWall.TryNotify();
			}
		}

		public float AutoScrollToPayWall(float duration = 2f, Action<float> onComplete = null)
		{
			Vector3 currentPos = seasonViews[currentViewIndex].transform.position;
			float targetPos = currentPos.y - seasonViews[currentViewIndex].GetTopWorldPosition().y;

			float initYPos = currentPos.y;
			float seasonBottomY = seasonViews[currentViewIndex].GetBottomWorldPosition().y;
			if (seasonBottomY > ViewController.ScreenBottom.y)
				initYPos -= seasonBottomY - ViewController.ScreenBottom.y;

			AutoScrollToPosition(targetPos, duration, OnComplete);
			void OnComplete() => onComplete?.Invoke(initYPos);

			return initYPos;
		}

		public void AutoScrollToPosition(float target, float duration = 2f, Action onComplete = null)
		{
			FollowStart();

			Vector3 currentPos = seasonViews[currentViewIndex].transform.position;

			DOTween.To(() => currentPos.y, (x) =>
			{
				currentPos.y = x;
				MoveUpdate(CheckPosition(currentPos));

			}, target, duration).SetEase(Ease.InOutQuad).
			onComplete = OnComplete;

			void OnComplete()
			{
				FollowStop();
				onComplete?.Invoke();
			}
		}
	}
}
