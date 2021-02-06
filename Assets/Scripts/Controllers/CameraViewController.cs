using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class CameraViewController : MonoBehaviour
    {
        // The minumum navigation position.
        private float _minPosition = 0f;

        // The maximum navigation position.
        private float _maxPosition = 0f;

        void Awake()
        {
            PlayerControlInfo controlInfo = FindObjectOfType<PlayerControlInfo>();
            controlInfo.onDrag.AddListener(OnDrag);

            SetMax(100);
        }

        /// <summary>
        /// Binds the drag position.
        /// </summary>
        void BindPosition()
        {
            Vector3 pos = transform.position;
            pos.y = Mathf.Clamp(transform.position.y, _minPosition, _maxPosition);
            transform.position = pos;
        }

        /// <summary>
        /// The drag event.
        /// </summary>
        /// <param name="delta">The delta position.</param>
        void OnDrag(Vector3 delta)
        {
            delta.x = 0;
            transform.Translate(delta);
            BindPosition();
        }

        /// <summary>
        /// Sets the navigation limits.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        public void SetLimits(float min, float max)
        {
            SetMin(min);
            SetMax(max);
        }

        /// <summary>
        /// Sets the maximum position.
        /// </summary>
        /// <param name="value">The new value.</param>
        public void SetMax(float value)
        {
            _maxPosition = value;
            BindPosition();
        }

        /// <summary>
        /// Sets the minimum position.
        /// </summary>
        /// <param name="value">The new value.</param>
        public void SetMin(float value)
        {
            _minPosition = value;
            BindPosition();
        }
    }
}
