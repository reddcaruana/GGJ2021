using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Rods
{
    public class FishingLineHealth : MonoBehaviour
    {
        // The line renderer component.
        private LineRenderer _lineRenderer;

        // The gradient blending value.
        private float _blending = 0.025f;

        // The change of color position.
        private float _position = 1f;

        // The current stress.
        [SerializeField]
        private float _stress = 0;

        // The maximum stress.
        private float _maxStress = 10;

        void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        void Update()
        {
            SetStress(_stress);
        }

        /// <summary>
        /// Sets the color of the fishing line.
        /// </summary>
        /// <param name="normal">The normal color.</param>
        /// <param name="stress">The stress color.</param>
        /// <param name="position">The normalized position.</param>
        private void SetColor(Color normal, Color stress, float position)
        {
            Gradient gradient = new Gradient();

            float minBlend = (position > 0 && position < 1) ? Mathf.Clamp(position - _blending, 0f, 1f) : position;
            float maxBlend = (position > 0 && position < 1) ? Mathf.Clamp(position + _blending, 0f, 1f) : position;

            gradient.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(normal, 0f),
                    new GradientColorKey(normal, minBlend),
                    new GradientColorKey(stress, maxBlend),
                    new GradientColorKey(stress, 1f)
                },
                new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) }
            );

            _lineRenderer.colorGradient = gradient;
        }

        /// <summary>
        /// Sets the fishing line stress.
        /// </summary>
        /// <param name="value">The stress value.</param>
        public void SetStress(float value)
        {
            _stress = value;
            SetColor(Color.white, Color.red, (_maxStress - _stress) / _maxStress);
        }
    }
}
