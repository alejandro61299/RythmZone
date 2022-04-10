using System;
using UnityEngine;

namespace Bezier.Points
{
    public abstract class BezierPoint
    {
        private readonly BezierControlPoints _controlPoints;
        protected Color _color;
        protected float _size;
        
        public Color Color => _color;
        public Vector3 Position { get; set;}
        public BezierControlPoints ControlPoints => _controlPoints;
        public float Size => _size;
        public event Action<Vector3> OnPreChangePosition;

        protected BezierPoint(BezierControlPoints controlPoints)
        {
            _controlPoints = controlPoints;
            Position = Vector3.zero;
        }

        public void SetPoint(Vector3 position)
        {
            OnPreChangePosition?.Invoke(position);
            Position = position;
        }
    }
}