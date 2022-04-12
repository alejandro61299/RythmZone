using System;
using System.Collections.Generic;
using Bezier.Utils;
using UnityEngine;

namespace Bezier
{
    [ExecuteInEditMode]
    public class BezierSpline : MonoBehaviour
    {
        [SerializeField] private List<BezierControlPoint> _controlPoints = new ();
        [SerializeField] private bool _loop;
        private Transform _transform;
        public List<BezierControlPoint> ControlPoints => _controlPoints;
        public bool Loop => _loop;
        public event Action OnAddControlPoint;
        public event Action OnRemoveControlPoint;

        private void Awake()
        {
            _transform = transform; 
        }
        
        private void Reset()
        {
            _controlPoints.Clear();
            BezierSplineInitializer.DefaultInitialize(this);
        }
 
        public void SetLoop(bool loop)
        {
            _loop = loop;
        }

        public BezierControlPoint AddControlPoint()
        {
            BezierControlPoint controlPoint = new BezierControlPoint();
            _controlPoints.Add(controlPoint);
            OnAddControlPoint?.Invoke();
            return controlPoint;
        }

        public void RemoveControlPoint(BezierControlPoint controlPoint)
        {
            if (_controlPoints.Remove(controlPoint))
            {
                OnRemoveControlPoint?.Invoke();
            }
        }
        
        public Vector3 GetPosition(float percent)
        {
            int startIndex = this.GetFloorControlPointIndex(percent, out float subPercent);
            Vector3 point = BezierUtils.GetPoint(_controlPoints[startIndex], this.GetNextControlPoints(startIndex), subPercent);
            return _transform.TransformPoint(point);
        }
        
        public Vector3 GetVelocity (float percent) 
        {
            int startIndex = this.GetFloorControlPointIndex(percent, out float subPercent);
            Vector3 point = BezierUtils.GetFirstDerivative(_controlPoints[startIndex], this.GetNextControlPoints(startIndex), subPercent);
            return _transform.TransformPoint(point) - _transform.position;
        }
        
        public Vector3 GetDirection (float t) 
        {
            return GetVelocity(t).normalized;
        }
    }
}