using UnityEngine;
using Assets.Scripts.Rods;
using UnityEngine.InputSystem;
using Assets.Scripts.Framework;
using Assets.Scripts.Framework.Tools;

namespace Assets.Scripts.Controllers
{
	public class GameController : MonobehaviourSingleton<GameController>
	{
		Vector2 mousePos;

		public RodBase Rod { get; private set; }

		//[SerializeField] Transform obj;
		//AccelerationModule a;
		//float angle = 0;

		protected override void Awake()
		{
			base.Awake();
			GameFactory.Start();

			ViewController.Init();

			//void TestSet(Vector3 pos) => obj.transform.position = pos;
			//Vector3 TestGet() => obj.transform.position;
			//a = new AccelerationModule(TestSet, TestGet);

		}

		private void Start()
		{
			Rod = new BasicRod();
			Rod.CreateView(ViewController.MainParent);
			Rod.RegisterToOnCastComnplete(ViewController.CurrentSeason.OnCast);
		}

		private void OnPosition(InputValue inputValue)
		{
			Vector2 value = inputValue.Get<Vector2>();
			mousePos = value;
		}

		private void OnClick()
		{
			Rod.ReelIn();
			Rod.TryCast(mousePos);
		}

		private void OnUp()
		{
			//a.SetSpeed(0.05f);
		}

		private void OnDown()
		{
			//a.SetSpeed(-0.05f);
		}

		private void OnRight()
		{
			//angle++;
			//obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
			//a.SetDirection(angle);
		}

		private void OnLeft()
		{
			//angle--;
			//obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
			//a.SetDirection(angle);
		}
	}
}
