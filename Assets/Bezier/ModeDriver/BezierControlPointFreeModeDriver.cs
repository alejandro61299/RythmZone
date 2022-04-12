using UnityEngine;

namespace Bezier.ModeDriver
{
    public class BezierControlPointFreeModeDriver : BezierControlPointModeDriver
    {
        public BezierControlPointFreeModeDriver(BezierControlPoint controlPoint) : base(controlPoint) {}

        protected override void DriveTangent0(Vector3 newPosition) {}

        protected override void DriveTangent1(Vector3 newPosition) {}
    }
}