using UnityEngine;
using Utils.MathR;

namespace Bezier.ModeDriver
{
    public class BezierControlPointAlignedModeDriver : BezierControlPointModeDriver
    {
        public BezierControlPointAlignedModeDriver(BezierControlPoint controlPoint) : base(controlPoint) {}

        protected override void DriveTangent0(Vector3 newTarget1Position)
        {
            float distance = Vector3.Distance(controlPoint.Main.Position, controlPoint.Tangent0.Position);
            Vector3 mirrorPosition = (newTarget1Position - controlPoint.Main.Position).normalized;
            controlPoint.Tangent0.Position = controlPoint.Main.Position - mirrorPosition * distance;
        }

        protected override void DriveTangent1(Vector3 newTarget0Position)
        {
            float distance = Vector3.Distance(controlPoint.Main.Position, controlPoint.Tangent1.Position);
            Vector3 mirrorPosition = (newTarget0Position - controlPoint.Main.Position).normalized;
            controlPoint.Tangent1.Position = controlPoint.Main.Position -mirrorPosition * distance;
        }
    }
}