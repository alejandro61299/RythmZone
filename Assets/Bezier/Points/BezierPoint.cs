using System;
using UnityEngine;

namespace Bezier.Points
{
    [Serializable]
    public abstract class BezierPoint
    {
        [SerializeField] public Vector3 Position;
        private readonly BezierControlPoint _controlPoint;
        public BezierControlPoint ControlPoint => _controlPoint;
        
        public abstract Color Color { get; }
        public abstract float Size { get; }
        
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