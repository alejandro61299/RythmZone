using System;
using UnityEngine;

namespace Bezier.Utils
{
    public static class BezierSplineControlPointsUtility
    {
        public static BezierControlPoint FirstControlPoint(this BezierSpline bezier)
        {
            return bezier.ControlPoints[0] ;
        }
        
        public static BezierControlPoint LastControlPoint(this BezierSpline bezier)
        {
            return bezier.ControlPoints[^1];
        }

        public static int ControlPointsCount(this BezierSpline bezier)
        {
            return bezier.Loop ? bezier.ControlPoints.Count : bezier.ControlPoints.Count - 1;
        }
        
        public static BezierControlPoint GetNextControlPoints(this BezierSpline bezier,int index)
        {
            if (++index >= bezier.ControlPoints.Count)
            {
                index = 0;
            }
            
            return bezier.ControlPoints[index];
        }

        public static BezierControlPoint GetPreviousControlPoints(this BezierSpline bezier, int index)
        {
            if (--index < 0)
            {
                index = bezier.ControlPoints.Count - 1;
            }
            
            return bezier.ControlPoints[index];
        }

        public static int GetFloorControlPointIndex(this BezierSpline bezier, float percent, out float subPercent)
        {
            int pointIndex;
            int pointsCount = ControlPointsCount(bezier);
            percent = Mathf.Clamp01(percent);
            
            if (percent >= 1f) 
            {
                subPercent = 1f;
                pointIndex = pointsCount - 1;
            }
            else 
            {
                subPercent = percent * pointsCount;
                pointIndex = (int)subPercent;
                subPercent -= pointIndex;
            }

            return pointIndex;
        }
    }
}