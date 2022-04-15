using System;
using UnityEngine;

namespace Bezier.Points
{
    [Serializable]
    public class BezierMainPoint : BezierPoint
    {
        public BezierMainPoint(BezierControlPoint point) : base(point) { }
        public override Color Color =>  Color.yellow;
        public override float Size => 2f;
    }
}