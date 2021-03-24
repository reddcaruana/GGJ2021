using System;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Views.Boat
{
	public class BoatView : MonoBehaviour
	{
		private SpriteRenderer boatSpriteRenderer;
		private float initLocalPosY;
		private float hideLocalPosY;

		private void Awake()
		{
			boatSpriteRenderer = transform.Find("ImageBoat").GetComponent<SpriteRenderer>();
			initLocalPosY = transform.localPosition.y;
			hideLocalPosY = initLocalPosY - 6.5f;
		}

		public void Show(float duration, Action onComplpete = null)
		{
			transform.DOLocalMoveY(initLocalPosY, duration).onComplete = OnComplete;
			void OnComplete() => onComplpete?.Invoke();
		}

		public void Hide(float duration, Action onComplpete = null)
		{
			transform.DOLocalMoveY(hideLocalPosY, duration).onComplete = OnComplete;
			void OnComplete() => onComplpete?.Invoke();
		}
	}
}
