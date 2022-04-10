using System;
using UnityEngine;

namespace Bezier.ModeDriver
{
    public abstract class BezierControlPointModeDriver
    {
        protected readonly BezierControlPoints _controlPoints;

        protected BezierControlPointModeDriver(BezierControlPoints controlPoints)
        {
            _controlPoints = controlPoints;
        }
        
        private void Initialize()
        {
            _controlPoints.Main.OnPreChangePosition += DriveTangents;
            _controlPoints.Tangent0.OnPreChangePosition += DriveTangent1;
            _controlPoints.Tangent1.OnPreChangePosition += DriveTangent0;
        }

        private void Terminate()
        {
            _controlPoints.Main.OnPreChangePosition -= DriveTangents;
            _controlPoints.Tangent0.OnPreChangePosition -= DriveTangent1;
            _controlPoints.Tangent1.OnPreChangePosition -= DriveTangent0;
        }
        
        private void DriveTangents(Vector3 newMainPosition)
        {
            Vector3 delta = newMainPosition - _controlPoints.Main.Position;
            _controlPoints.Tangent0.Position += delta;
            _controlPoints.Tangent1.Position += delta;
        }
        
        protected abstract void DriveTangent0(Vector3 newPosition);
        
        protected abstract void DriveTangent1(Vector3 newPosition);
        
        public static BezierControlPointModeDriver  InstantiateDriver(BezierControlPoints controlPoints, BezierControlPointModeDriver driver = null)
        {
            driver?.Terminate();
            
            driver = controlPoints.Mode switch
            {
                BezierPointMode.Free => new BezierControlPointFreeModeDriver(controlPoints),
                BezierPointMode.Aligned => new BezierControlPointAlignedModeDriver(controlPoints),
                BezierPointMode.Mirrored => new BezierControlPointMirroredModeDriver(controlPoints),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            driver.Initialize();
            return driver;
        }
    }
}