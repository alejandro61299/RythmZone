using System;
using UnityEngine;

namespace Bezier.Points
{
    [Serializable]
    public abstract class BezierPoint
    {
        private readonly BezierControlPoint _controlPoint;
        [SerializeField] public Vector3 Position;
        protected Color _color;
        protected float _size;
        
        public Color Color => _color;
        public BezierControlPoint ControlPoint => _controlPoint;
        public float Size => _size;
        public event Action<Vector3> OnPreChangePosition;

        protected BezierPoint(BezierControlPoint controlPoint)
        {
            _controlPoint = controlPoint;
        } 
 
        public void SetPoint(Vector3 position)
        {
            OnPreChangePosition?.Invoke(position);
            Position = position;
        }
    }
}