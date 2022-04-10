using UnityEngine;
using Utils.MathR;

namespace Bezier.ModeDriver
{
    public class BezierControlPointAlignedModeDriver : BezierControlPointModeDriver
    {
        public BezierControlPointAlignedModeDriver(BezierControlPoints controlPoints) : base(controlPoints) {}

        protected override void DriveTangent0(Vector3 newTarget1Position)
        {
            float distance = Vector3.Distance(_controlPoints.Main.Position, _controlPoints.Tangent0.Position);
            Vector3 mirrorPosition = (newTarget1Position - _controlPoints.Main.Position).normalized;
            _controlPoints.Tangent0.Position = _controlPoints.Main.Position - mirrorPosition * distance;
        }

        protected override void DriveTangent1(Vector3 newTarget0Position)
        {
            float distance = Vector3.Distance(_controlPoints.Main.Position, _controlPoints.Tangent1.Position);
            Vector3 mirrorPosition = (newTarget0Position - _controlPoints.Main.Position).normalized;
            _controlPoints.Tangent1.Position = _controlPoints.Main.Position -mirrorPosition * distance;
        }
    }
}