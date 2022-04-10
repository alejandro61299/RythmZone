using UnityEngine;

namespace Bezier.ModeDriver
{
    public class BezierControlPointFreeModeDriver : BezierControlPointModeDriver
    {
        public BezierControlPointFreeModeDriver(BezierControlPoints controlPoints) : base(controlPoints) {}

        protected override void DriveTangent0(Vector3 newPosition) {}

        protected override void DriveTangent1(Vector3 newPosition) {}
    }
}