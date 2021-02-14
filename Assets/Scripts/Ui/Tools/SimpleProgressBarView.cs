using System;
using Assets.Scripts.Framework.Ui;

namespace Assets.Scripts.Ui.Tools
{
	public class SimpleProgressBarView : RDProgressBarNoSpriteView
	{
		protected override string GetImagePath() => "ImageProgress";
	}
}
