using System;
using UnityEngine.UI;

namespace Assets.Scripts.Framework.Ui
{
	public class UiTransitionButtonAttachemnt<T> : UiTransitionAttachment
		where T : Button
	{
		public T button;

		protected override void OnStartTransition() => button.enabled = false;
		protected override void OnDoneTransition(State newState, Action onComplete)
		{
			button.enabled = true;
			base.OnDoneTransition(newState, onComplete);
		}
	}
}
