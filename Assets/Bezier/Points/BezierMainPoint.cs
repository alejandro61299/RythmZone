using System;
using UnityEngine;

namespace Bezier.Points
{
    public class BezierMainPoint : BezierPoint
    {
        public BezierMainPoint(BezierControlPoints points) : base(points)
        {
            _color = Color.yellow;
        }
    }
}