using System;
using UnityEngine;

namespace Bezier.Points
{
    public class BezierTangent1Point : BezierPoint
    {
        public BezierTangent1Point(BezierControlPoints points) : base(points)
        {
            _color = Color.yellow;
        }
    }
}