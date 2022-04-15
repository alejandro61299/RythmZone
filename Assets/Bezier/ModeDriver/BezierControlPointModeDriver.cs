using System;
using UnityEngine;

namespace Bezier.ModeDriver
{
    public abstract class BezierControlPointModeDriver
    {
        protected readonly BezierControlPoint controlPoint;

        protected BezierControlPointModeDriver(BezierControlPoint controlPoint)
        {
            this.controlPoint = controlPoint;
        }

        public void ForceDrive()
        {
            DriveTangent0(controlPoint.Tangent1.Position);
        }
        
        private void Initialize()
        {
            controlPoint.Main.OnPreChangePosition += DriveTangents;
            controlPoint.Tangent0.OnPreChangePosition += DriveTangent1;
            controlPoint.Tangent1.OnPreChangePosition += DriveTangent0;
        }

        private void Terminate()
        {
            controlPoint.Main.OnPreChangePosition -= DriveTangents;
            controlPoint.Tangent0.OnPreChangePosition -= DriveTangent1;
            controlPoint.Tangent1.OnPreChangePosition -= DriveTangent0;
        }
         
        private void DriveTangents(Vector3 newMainPosition)
        {
            Vector3 delta = newMainPosition - controlPoint.Main.Position;
            controlPoint.Tangent0.Position += delta;
            controlPoint.Tangent1.Position += delta;
        }
        
        protected abstract void DriveTangent0(Vector3 newPosition);
        
        protected abstract void DriveTangent1(Vector3 newPosition);
        
        public static BezierControlPointModeDriver  InstantiateDriver(BezierControlPoint controlPoint, BezierControlPointModeDriver driver = null)
        {
            driver?.Terminate();
            
            driver = controlPoint.Mode switch
            {
                BezierPointMode.Free => new BezierControlPointFreeModeDriver(controlPoint),
                BezierPointMode.Aligned => new BezierControlPointAlignedModeDriver(controlPoint),
                BezierPointMode.Mirrored => new BezierControlPointMirroredModeDriver(controlPoint),
                _ => throw new ArgumentOutOfRangeException()
            }; 
            
            driver.Initialize();
            return driver; 
        }
    }
}