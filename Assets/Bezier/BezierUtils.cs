﻿using UnityEngine;

namespace Bezier
{
	public static class BezierUtils 
	{
		public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
			t = Mathf.Clamp01(t);
			float OneMinusT = 1f - t;
			return
				OneMinusT * OneMinusT * OneMinusT * p0 +
				3f * OneMinusT * OneMinusT * t * p1 +
				3f * OneMinusT * t * t * p2 +
				t * t * t * p3;
		}

		public static Vector3 GetPoint(BezierControlPoint startPoint, BezierControlPoint endPoint, float percent)
		{
			return GetPoint(
				startPoint.Main.Position, 
				startPoint.Tangent1.Position, 
				endPoint.Tangent0.Position, 
				endPoint.Main.Position, 
				percent);
		}

		public static Vector3 GetFirstDerivative (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
			t = Mathf.Clamp01(t);
			float oneMinusT = 1f - t;
			return
				3f * oneMinusT * oneMinusT * (p1 - p0) +
				6f * oneMinusT * t * (p2 - p1) +
				3f * t * t * (p3 - p2);
		}
		
		public static Vector3 GetFirstDerivative(BezierControlPoint startPoint, BezierControlPoint endPoint, float percent)
		{
			return GetFirstDerivative(
				startPoint.Main.Position,
				startPoint.Tangent1.Position,
				endPoint.Tangent0.Position,
				endPoint.Main.Position,
				percent);
		}
	}
}