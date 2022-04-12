using UnityEngine;
using Utils.MathR;

namespace Bezier.ModeDriver
{
    public class BezierControlPointMirroredModeDriver : BezierControlPointModeDriver
    {
        public BezierControlPointMirroredModeDriver(BezierControlPoint controlPoint) : base(controlPoint) {}

        protected override void DriveTangent0(Vector3 newTarget1Position)
        {
            controlPoint.Tangent0.Position = newTarget1Position.Mirror(controlPoint.Main.Position);
        }

        protected override void DriveTangent1(Vector3 newTarget0Position)
        {
            controlPoint.Tangent1.Position =newTarget0Position.Mirror(controlPoint.Main.Position);
        }
    }
}