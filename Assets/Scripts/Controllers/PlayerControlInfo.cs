using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Controllers
{
    public class MouseScreenToWorldEvent : UnityEvent<Vector3>
    { }

    public class PlayerControlInfo : MonoBehaviour
    {
        public static PlayerControlInfo main { get; private set; }

        // The drag event - submits delta values.
        public MouseScreenToWorldEvent onDrag = new MouseScreenToWorldEvent();

        // The tap event.
        public MouseScreenToWorldEvent onTap = new MouseScreenToWorldEvent();

        // User tapping the screen.
        public bool isTapping { get; private set; } = false;

        // User holding their cursor/finger on the screen.
        public bool isHolding { get; private set; } = false;

        // The click origin.
        private Vector2 _origin;

        // The current position.
        private Vector2 _mousePosition;

        void Awake()
        {
            if (main == null)
            {
                main = this;
            }
        }

        void OnDestroy()
        {
            if (main == this)
            {
                main = null;
            }
        }

        /// <summary>
        /// The move event.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        void OnMove(InputValue inputValue)
        {
            Vector2 value = inputValue.Get<Vector2>();
            _mousePosition = value;

            if (isHolding)
            {
                Vector3 origin = Camera.main.ScreenToWorldPoint(_origin);
                Vector3 position = Camera.main.ScreenToWorldPoint(_mousePosition);
                
                onDrag.Invoke(origin - position);
                _origin = _mousePosition;
            }
        }

        /// <summary>
        /// The press event.
        /// </summary>
        void OnPress()
        {
            isHolding = true;
            _origin = _mousePosition;
        }

        /// <summary>
        /// The release event.
        /// </summary>
        void OnRelease()
        {
            isHolding = false;
        }

        /// <summary>
        /// The tap event.
        /// </summary>
        void OnTap()
        {
            isHolding = false;
            onTap.Invoke(_mousePosition);
        }
    }
}
