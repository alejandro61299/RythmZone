using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bezier
{
    public class MyBezierSpline : MonoBehaviour
    {
        [SerializeField] private List<BezierControlPoints> _controlPoints = new ();
        [SerializeField] private Transform _transform;
        [SerializeField] private bool _loop;

        public List<BezierControlPoints> ControlPoints => _controlPoints;
        public bool Loop => _loop;
        public Transform Transform => _transform;

        private void OnValidate()
        {
            TryInitialize();
        }

        private void Awake()
        {
            TryInitialize();
        }

        private void Reset()
        {
            TryInitialize();
        }

        private void TryInitialize()
        {
            _transform = transform;
            _controlPoints.Clear();
            BezierControlPoints startPoint = new BezierControlPoints(isFirst: true);
            BezierControlPoints endPoint = new BezierControlPoints(isLast: true);
            startPoint.SetMode(BezierPointMode.Mirrored);
            startPoint.Main.SetPoint(Vector3.left * 2f);
            startPoint.Tangent1.SetPoint(startPoint.Main.Position + Vector3.up);
            endPoint.SetMode(BezierPointMode.Mirrored);
            endPoint.Main.SetPoint(Vector3.right * 2f);
            endPoint.Tangent1.SetPoint(endPoint.Main.Position  +Vector3.down);
            _controlPoints.Add(startPoint);
            _controlPoints.Add(endPoint);
        }

        public void SetLoop(bool loop)
        {
            _loop = loop;
        }

        public void AddCurve()
        {
            BezierControlPoints controlPoints = new BezierControlPoints();
            _controlPoints.Insert(1, controlPoints);
        }
        
        public Vector3 GetPosition(float percent)
        {
            int startIndex = GetFloorControlPointIndex(percent, out float subPercent);
            Vector3 point = BezierUtils.GetPoint(_controlPoints[startIndex], GetNextControlPoints(startIndex), subPercent);
            return _transform.TransformPoint(point);
        }
        
        public Vector3 GetVelocity (float percent) 
        {
            int startIndex = GetFloorControlPointIndex(percent, out float subPercent);
            Vector3 point = BezierUtils.GetFirstDerivative(_controlPoints[startIndex], GetNextControlPoints(startIndex), subPercent);
            return _transform.TransformPoint(point) - _transform.position;
        }
	
        public Vector3 GetDirection (float t) {
            return GetVelocity(t).normalized;
        }
        private int GetFloorControlPointIndex(float percent, out float subPercent)
        {
            percent = Mathf.Clamp01(percent);
            int pointIndex;
            int pointsCount = PointsCount();
            
            if (percent >= 1f) 
            {
                subPercent = 1f;
                pointIndex = pointsCount - 1;
            }
            else 
            {
                subPercent = percent * pointsCount;
                pointIndex = (int)subPercent;
                subPercent -= pointIndex;
            }

            return pointIndex;
        }
        
        private BezierControlPoints GetNextControlPoints(int index)
        {
            if (++index ==  _controlPoints.Count)
            {
                index = 0;
            }
            
            return _controlPoints[index];
        }
        
        public int PointsCount()
        {
            return _loop ? _controlPoints.Count : _controlPoints.Count - 1;
        }
    }
}