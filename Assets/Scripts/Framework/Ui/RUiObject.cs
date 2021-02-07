using UnityEngine;

namespace Assets.Scripts.Framework.Ui
{
	public class RUiObject : MonoBehaviour
	{
        private RectTransform rectTransformRef;

        public RectTransform rectTransform
        {
            get
            {
                if (rectTransformRef == null)
                    rectTransformRef = GetComponent<RectTransform>();
                return rectTransformRef;
            }
        }
    }
}
