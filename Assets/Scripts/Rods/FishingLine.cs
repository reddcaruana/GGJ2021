using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Rods
{
    public class FishingLine : MonoBehaviour
    {
        // The line renderer component.
        private LineRenderer _lineRenderer;

        // The number of segments.
        private int _nPoints = 30;

        // The point positions.
        private Vector3[] _points;

        // The anchor position.
        private Vector3 _anchorPosition = Vector3.zero;

        // The control position.
        private Vector3 _controlPosition = Vector3.zero;

        // The hook position.
        private Vector3 _hookPosition = Vector3.zero;

        [Header("Testing")]
        [SerializeField]
        private Transform _anchor = null;

        [SerializeField]
        private Transform _hook = null;

        [SerializeField]
        private Transform _control = null;

        [SerializeField]
        private bool _isTesting = false;

        void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        void Update()
        {
            if (_isTesting)
            {
                _anchorPosition = _anchor.position;
                _hookPosition = _hook.position;

                _controlPosition = (_control == null) ? Vector3.Lerp(_anchorPosition, _hookPosition, 0.5f) : _control.position;

                SetPositions(_anchorPosition, _controlPosition, _hookPosition);
            }
        }

        /// <summary>
        /// Links the line to an anchor.
        /// </summary>
        /// <param name="anchor">The anchor transform.</param>
        public void SetAnchor(Transform anchor)
        {
            _anchor = anchor;
            if (anchor != null) _anchorPosition = anchor.position;
        }

        /// <summary>
        /// Anchors the line on a position.
        /// </summary>
        /// <param name="value">The position.</param>
        public void SetAnchor(Vector3 value)
        {
            _anchor = null;
            _anchorPosition = value;
        }

        /// <summary>
        /// Links the control point.
        /// </summary>
        /// <param name="control">The control transform.</param>
        public void SetControl(Transform control)
        {
            _control = control;
            _controlPosition = control.position;
        }

        /// <summary>
        /// Sets the control position.
        /// </summary>
        /// <param name="control">The position.</param>
        public void SetControl(Vector3 value)
        {
            _control = null;
            _controlPosition = value;
        }

        /// <summary>
        /// Links the line to a hook.
        /// </summary>
        /// <param name="hook">The hook transform.</param>
        public void SetHook(Transform hook)
        {
            _hook = hook;
            _hookPosition = hook.position;
        }

        /// <summary>
        /// Hooks the line on a position.
        /// </summary>
        /// <param name="value">The position.</param>
        public void SetHook(Vector3 value)
        {
            _hook = null;
            _hookPosition = value;
        }

        /// <summary>
        /// Positions the line and all its points.
        /// </summary>
        /// <param name="start">The start position.</param>
        /// <param name="control">The control position.</param>
        /// <param name="end">The end position.</param>
        public void SetPositions(Vector3 start, Vector3 control, Vector3 end)
        {
            _points = new Vector3[_nPoints];
            
            int maxPoint = _nPoints - 1;
            for (int i = 0; i < _nPoints; i++)
                _points[i] = GetPosition(start, control, end, (float) i / maxPoint);

            _lineRenderer.positionCount = _nPoints;
            _lineRenderer.SetPositions(_points);
        }

        /// <summary>
        /// Gets the position of a point on a line.
        /// </summary>
        /// <param name="start">The start position.</param>
        /// <param name="control">The control position.</param>
        /// <param name="end">The end position.</param>
        /// <param name="t">The normalized point value.</param>
        Vector3 GetPosition(Vector3 start, Vector3 control, Vector3 end, float t)
            => Vector3.Lerp(Vector3.Lerp(start, control, t), Vector3.Lerp(control, end, t), t);
    }
}