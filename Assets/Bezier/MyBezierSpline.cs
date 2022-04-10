using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bezier
{
    public class MyBezierSpline : MonoBehaviour
    {
        [SerializeField] private List<BezierControlPoints> _controlPoints = new ();
        [SerializeField] private bool _loop;

        public List<BezierControlPoints> ControlPoints => _controlPoints;
        public bool Loop => _loop;

        
        private void OnValidate()
        {
            AddInitialPoints();
        }

        private void AddInitialPoints()
        {
            _controlPoints.Clear();
            
            if (_controlPoints.Count >= 2)
            {
                return;
            }

            BezierControlPoints startPoint = new BezierControlPoints(isFirst: true);
            BezierControlPoints endPosition = new BezierControlPoints(isLast: true);
            startPoint.SetPoint(Vector3.left * 2f);
            endPosition.SetPoint(Vector3.right * 2f);
            _controlPoints.Add(startPoint);
            _controlPoints.Add(endPosition);
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
            return transform.TransformPoint(point);
        }
        
        public Vector3 GetVelocity (float percent) 
        {
            int startIndex = GetFloorControlPointIndex(percent, out float subPercent);
            Vector3 point = BezierUtils.GetFirstDerivative(_controlPoints[startIndex], GetNextControlPoints(startIndex), subPercent);
            return transform.TransformPoint(point);
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
                pointIndex = pointsCount;
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
            if (++index > PointsCount())
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