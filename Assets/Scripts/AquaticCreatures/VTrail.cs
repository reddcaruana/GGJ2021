using UnityEngine;
using Assets.Scripts.Framework;
using Assets.Scripts.Views.Fish;
using Assets.Scripts.Controllers;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.AquaticCreatures
{
	public class VTrail
	{
		public bool IsActive => view.IsActive;

		private Vector3 rotation = new Vector3();
		private VTrailView view;

		public void Init()
		{
			SpriteRenderer spriteRenderer = new GameObject("VTrail").AddComponent<SpriteRenderer>();
			view = spriteRenderer .gameObject.AddComponent<VTrailView>();
			view.transform.SetParent(ViewController.MainParent);
		}

		public void Size(float fishSize) => view.transform.localScale = Statics.VECTOR3_ONE * fishSize;

		public void Show() => view.Show();

		public void Update(Vector3 worldPosition, Vector3 lookAtWorldPos)
		{
			view.transform.position = worldPosition;
			rotation.z = MathUtils.AimAtTarget2D(worldPosition, lookAtWorldPos) + 90;
			view.transform.rotation = Quaternion.Euler(rotation);
		}

		public void Hide() => view.Hide();
	}
}
