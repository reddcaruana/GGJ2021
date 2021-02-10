using UnityEngine;

namespace Assets.Scripts.Framework.Tools
{
	public interface IProgressBarView
	{
		void ProgressUpdate(float value);
	}

	public class RDProgressBar
	{
		public float Max { get; private set; }
		public float Value { get; private set; }
		public float Progress => Value / Max;

		IProgressBarView view;

		public RDProgressBar()
		{
		}

		public RDProgressBar(float max, float currentValue) : this(max)
		{
			Value = currentValue;
		}

		public RDProgressBar(float max)
		{
			Max = max;
		}

		public void SetMax(float max) => Max = max;

		public void AssignView(IProgressBarView view) => this.view = view;

		public void Add(float value) => Set(Value + value);
		public void Remove(float value) => Set(Value - value);

		public void Set(float value)
		{
			Value = Mathf.Min(value, Max);
			Value = Mathf.Max(Value, 0);
			ViewUpdate();
		}


		private void ViewUpdate() => view?.ProgressUpdate(Progress);
	}
}
