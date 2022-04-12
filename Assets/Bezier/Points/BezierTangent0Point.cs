using System;
using UnityEngine;

namespace Bezier.Points
{
    [Serializable]
    public class BezierTangent0Point : BezierPoint
    {
        public BezierTangent0Point(BezierControlPoint point) : base(point)
        {
            _color = Color.yellow;
        }
    }
}