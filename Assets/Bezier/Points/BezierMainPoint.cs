using System;
using UnityEngine;

namespace Bezier.Points
{
    [Serializable]
    public class BezierMainPoint : BezierPoint
    {
        public BezierMainPoint(BezierControlPoint point) : base(point)
        {
            _color = Color.yellow; 
        }
    }
}