using UnityEngine;

namespace Assets.Scripts.Views
{
	public class FishView : MonoBehaviour
	{
		private Sprite sprite;

		private void Awake()
		{
			sprite = transform.Find("SpriteFish").GetComponent<Sprite>();
			Debug.Log("Is null? " + (sprite == null));
		}


	}
}
