using System;
using UnityEngine;

namespace Bezier.Points
{
    [Serializable]
    public class BezierTangent1Point : BezierPoint
    {
        public BezierTangent1Point(BezierControlPoint point) : base(point) { }
        public override Color Color =>  Color.blue;
        public override float Size => 2f;
    }
}