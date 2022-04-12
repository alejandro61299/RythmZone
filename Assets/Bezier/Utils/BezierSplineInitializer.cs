using System.Collections.Generic;
using UnityEngine;

namespace Bezier.Utils
{
    public static class BezierSplineInitializer
    {
        public static void DefaultInitialize(BezierSpline bezierSpline)
        {
            BezierControlPoint startPoint = bezierSpline.AddControlPoint();
            BezierControlPoint endPoint = bezierSpline.AddControlPoint();
            startPoint.SetMode(BezierPointMode.Mirrored);
            startPoint.Main.SetPoint(Vector3.left * 2f);
            startPoint.Tangent1.SetPoint(startPoint.Main.Position + Vector3.up);
            endPoint.SetMode(BezierPointMode.Mirrored);
            endPoint.Main.SetPoint(Vector3.right * 2f);
            endPoint.Tangent1.SetPoint(endPoint.Main.Position  +Vector3.down);
        }
    } 
}