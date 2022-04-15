using System;
using UnityEngine;

namespace Bezier.Points
{
    [Serializable]
    public class BezierTangent0Point : BezierPoint
    {
        public BezierTangent0Point(BezierControlPoint point) : base(point) { }
        public override Color Color =>  Color.cyan;
        public override float Size => 2f;
    }
}