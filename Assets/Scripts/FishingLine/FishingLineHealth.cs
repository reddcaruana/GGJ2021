using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.FishingLine
{
    public class FishingLineHealth : MonoBehaviour
    {
        // The line renderer component.
        private LineRenderer lineRenderer;

        // The number of points.
        private int nPoints = 30;

        // The point positions.
        private Vector3[] points;

        // The gradient blending value.
        private float blending = 0.025f;

        // The change of color position.
        private float position = 1f;

        void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        void Start()
        {
            SetPositions(Vector3.zero, Vector3.up * 10);
        }

        void Update()
        {
            SetColor(Color.white, Color.red, position);
        }

        Vector3 GetPosition(Vector3 start, Vector3 end, float x)
        {
            return x * Vector3.Lerp(start, end, x);
        }

        public void SetColor(Color start, Color end, float position)
        {
            Gradient gradient = new Gradient();

            float minBlend = (position > 0 && position < 1) ? Mathf.Clamp(position - blending, 0f, 1f) : position;
            float maxBlend = (position > 0 && position < 1) ? Mathf.Clamp(position + blending, 0f, 1f) : position;

            gradient.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(start, 0f),
                    new GradientColorKey(start, minBlend),
                    new GradientColorKey(end, maxBlend),
                    new GradientColorKey(end, 1f)
                },
                new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) }
            );

            lineRenderer.colorGradient = gradient;
        }

        public void SetPositions(Vector3 start, Vector3 end)
        {
            points = new Vector3[nPoints];
            for (int i = 0; i < nPoints; i++)
            {
                points[i] = GetPosition(start, end, (float) i / nPoints);
            }
            lineRenderer.positionCount = nPoints;
            lineRenderer.SetPositions(points);
        }
    }
}
