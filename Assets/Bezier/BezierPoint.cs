using System;
using UnityEngine;

namespace Bezier
{
    [Serializable]
    public abstract class BezierPoint
    {
        [SerializeField] protected BezierControlPoints _controlPoints;
        [SerializeField] protected Color _color;
        [SerializeField] protected float _size;
        public Color Color => _color;
        public Vector3 Position { get; set;}
        public BezierControlPoints ControlPoints => _controlPoints;
        public float Size => _size;

        protected BezierPoint(BezierControlPoints controlPoints)
        {
            _controlPoints = controlPoints;
            Position = Vector3.zero;
        }
    }

    public class BezierPositionPoint : BezierPoint
    {
        public BezierPositionPoint(BezierControlPoints points) : base(points)
        {
            _color = Color.yellow;
        }
    }
    
    public class BezierTangent0Point : BezierPoint
    {
        public BezierTangent0Point(BezierControlPoints points) : base(points)
        {
            _color = Color.yellow;
        }
    }
    
    public class BezierTangent1Point : BezierPoint
    {
        public BezierTangent1Point(BezierControlPoints points) : base(points)
        {
            _color = Color.yellow;
        }
    }
}