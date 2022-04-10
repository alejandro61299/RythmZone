using System;
using UnityEngine;

namespace Bezier.Points
{
    public class BezierTangent0Point : BezierPoint
    {
        public BezierTangent0Point(BezierControlPoints points) : base(points)
        {
            _color = Color.yellow;
        }
    }
}