using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Camera
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField]
        InputSettings inputSettings;

        // Flag to determine if the user is pressing the screen.
        bool isPressing = false;

        // Flag to determine if the user is holding the screen.
        bool isHolding = false;

        // The click origin.
        Vector2 origin;

        /// <summary>
        /// Press Event.
        /// </summary>
        void OnPress()
        {
            isPressing = true;
        }

        void OnHold()
        {
            isHolding = true;
        }

        /// <summary>
        /// Release Event.
        /// </summary>
        void OnRelease()
        {
            isPressing = false;
            isHolding = false;
        }

        /// <summary>
        /// Move Event.
        /// </summary>
        /// <param name="value">The input value.</param>
        void OnMove(InputValue value)
        {
            Vector2 pos = value.Get<Vector2>();
            if (isHolding)
            {
                Vector3 delta = UnityEngine.Camera.main.ScreenToWorldPoint(origin) - UnityEngine.Camera.main.ScreenToWorldPoint(pos);
                transform.Translate(Vector3.up * delta.y);
            }
            origin = pos;
        }
    }
}
