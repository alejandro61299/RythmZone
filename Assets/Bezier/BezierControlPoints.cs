using System;
using UnityEngine;

namespace Bezier
{
    [Serializable]
    public class BezierControlPoints
    {
        [SerializeField] private MyBezierSpline _bezier;
        [SerializeField] private BezierPointMode _mode;
        [SerializeField] private BezierPoint _positionPoint;
        [SerializeField] private BezierPoint _tangent0Point;
        [SerializeField] private BezierPoint _tangent1Point;
        [SerializeField] private bool _isFirst;
        [SerializeField] private bool _isLast;
        
        public BezierPointMode Mode => _mode;
        public Vector3 Position => _positionPoint.Position;
        public Vector3 Tangent0 => _tangent0Point.Position;
        public Vector3 Tangent1 => _tangent1Point.Position;
        
        public BezierPoint PositionPoint => _positionPoint;
        public BezierPoint Tangent0Point => _tangent0Point;
        public BezierPoint Tangent1Point => _tangent1Point;
        
        public bool IsFirst => _isFirst;
        public bool IsLast => _isLast;

        public BezierControlPoints(bool isFirst = false , bool isLast = false)
        {
            _isFirst = isFirst;
            _isLast = isLast; 
            _positionPoint = new BezierPositionPoint(this);
            _tangent0Point =new BezierTangent0Point(this);
            _tangent1Point = new BezierTangent1Point(this);
            SetTangent0(Vector3.left);
            SetTangent1(Vector3.right);
        }
        
        public void SetPoint(Vector3 position)
        {
            Vector3 delta = position - _positionPoint.Position;;
            _positionPoint.Position = position;
            MoveTangents(delta);
        }

        private void MoveTangents(Vector3 delta)
        {
            _tangent0Point.Position += delta;
            _tangent1Point.Position += delta;
        }

        public void SetTangent0(Vector3 position)
        {
            _tangent0Point.Position = position;
        }
        
        public void SetTangent1(Vector3 position)
        {
            _tangent1Point.Position = position;
        }

        public void SetMode(BezierPointMode mode)
        {
            _mode = mode;
        }
    }
}