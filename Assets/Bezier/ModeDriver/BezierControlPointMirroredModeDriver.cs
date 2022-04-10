using UnityEngine;
using Utils.MathR;

namespace Bezier.ModeDriver
{
    public class BezierControlPointMirroredModeDriver : BezierControlPointModeDriver
    {
        public BezierControlPointMirroredModeDriver(BezierControlPoints controlPoints) : base(controlPoints) {}

        protected override void DriveTangent0(Vector3 newTarget1Position)
        {
            _controlPoints.Tangent0.Position = newTarget1Position.Mirror(_controlPoints.Main.Position);
        }

        protected override void DriveTangent1(Vector3 newTarget0Position)
        {
            _controlPoints.Tangent1.Position =newTarget0Position.Mirror(_controlPoints.Main.Position);
        }
    }
}