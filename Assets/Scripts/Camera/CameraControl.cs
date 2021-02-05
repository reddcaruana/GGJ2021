using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Camera
{
    public class CameraControl : MonoBehaviour
    {
        float modifier = 0.5f;

        bool isHolding = false;

        void OnPress()
        {
            isHolding = true;
        }

        void OnRelease()
        {
            isHolding = false;
        }

        void OnMove(InputValue value)
        {
            if (isHolding)
            {
                Vector2 delta = value.Get<Vector2>();
                transform.Translate(Vector3.up * delta.y * modifier);
            }
        }
    }
}
