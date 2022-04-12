using System;
using UnityEngine;

namespace Bezier.Points
{
    [Serializable]
    public class BezierTangent1Point : BezierPoint
    {
        public BezierTangent1Point(BezierControlPoint point) : base(point)
        {
            _color = Color.yellow;
        }
    }
}