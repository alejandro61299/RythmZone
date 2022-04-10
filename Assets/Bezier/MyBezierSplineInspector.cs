#if UNITY_EDITOR

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Bezier
{
    [CustomEditor(typeof(MyBezierSpline))]
    
    public class MyBezierSplineInspector : Editor
    {
        private const int DISPLAY_STEPS = 10;
        private const float DIRECTION_MAGNITUDE = 0.5f;
        private const float HANDLE_SIZE = 0.1f;
        private const float PICK_SIZE = 0.12f;


        private MyBezierSpline _bezier;
        private Transform _handleTransform;
        private Quaternion _handleRotation;
        private BezierPoint _selectedPoint;
        
        public override void OnInspectorGUI () 
        {
            _bezier = (MyBezierSpline)target;
            EditorGUI.BeginChangeCheck();
            bool loop = EditorGUILayout.Toggle("Loop", _bezier.Loop);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(_bezier, "Toggle Loop");
                EditorUtility.SetDirty(_bezier);
                _bezier.SetLoop(loop);
            }
            if (_selectedPoint != null) 
            {
                DrawSelectedPointInspector();
            }
            
            if (GUILayout.Button("Add Curve")) {
                Undo.RecordObject(_bezier, "Add Curve");
                _bezier.AddCurve();
                EditorUtility.SetDirty(_bezier);
            }
        }

        private void DrawSelectedPointInspector() {
            GUILayout.Label("Selected Point");
            EditorGUI.BeginChangeCheck();
            Vector3 point = EditorGUILayout.Vector3Field("Position", _selectedPoint.Position);
            
            if (EditorGUI.EndChangeCheck()) 
            {
                Undo.RecordObject(_bezier, "Move Point");
                EditorUtility.SetDirty(_bezier);
                _selectedPoint.Position = point;
            }
            
            EditorGUI.BeginChangeCheck();
            BezierPointMode mode = (BezierPointMode)EditorGUILayout.EnumPopup("Mode", _selectedPoint.ControlPoints.Mode);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(_bezier, "Change Point Mode");
                _selectedPoint.ControlPoints.SetMode(mode);
                EditorUtility.SetDirty(_bezier);
            }
        }
        
        private void OnSceneGUI ()
        {
            _bezier = (MyBezierSpline)target;
            UpdateHandle();
            BezierSplineGUI();
        }

        private void UpdateHandle()
        {
            _handleTransform = _bezier.transform;
            _handleRotation =  UnityEditor.Tools.pivotRotation == PivotRotation.Local 
                ? _handleTransform.rotation 
                : Quaternion.identity;
        }

        private void BezierSplineGUI()
        {
            BezierSplineDraw();
            BezierSplineLines();
        }


        private void BezierSplineLines()
        {
            BezierControlPoints point0 = _bezier.ControlPoints.First();
            
            for (int i = 1 ; i < _bezier.ControlPoints.Count ; ++i)
            {
                BezierControlPoints point1 = _bezier.ControlPoints[i];
                BezierCurveDraw(point0, point1);
                point0 = point1;
            }

            if (_bezier.Loop)
            {
                BezierCurveDraw(point0, _bezier.ControlPoints.First());
            }
            
            _bezier.ControlPoints.ForEach(BezierTangentsDraw);
            
            BezierDirectionsDraw();
        }


        private void BezierTangentsDraw(BezierControlPoints points)
        {
            Vector3 position = _handleTransform.TransformPoint(points.Position);
            Vector3 tangent0 = _handleTransform.TransformPoint(points.Tangent0);
            Vector3 tangent1 = _handleTransform.TransformPoint(points.Tangent1); 
                
            if (!points.IsFirst && !_bezier.Loop)
            {
                Handles.DrawLine(tangent0, position);
            }

            if (!points.IsLast && !_bezier.Loop)
            {
                Handles.DrawLine(tangent1, position);
            }
        }
        
        private void BezierCurveDraw(BezierControlPoints startPoints, BezierControlPoints endPoints)
        {
            Vector3 startPosition = _handleTransform.TransformPoint(startPoints.Position);
            Vector3 endPosition = _handleTransform.TransformPoint(endPoints.Position);
            Vector3 startTangent = _handleTransform.TransformPoint(startPoints.Tangent1); 
            Vector3 endTangent = _handleTransform.TransformPoint(endPoints.Tangent0);
            
            Handles.color = Color.gray;
            Handles.DrawBezier(startPosition, endPosition, startTangent, endTangent, Color.white, null, 2f);
        }

        private void BezierDirectionsDraw () 
        {
            Handles.color = Color.green;
            Vector3 point = _bezier.GetPosition(0f);
            Handles.DrawLine(point, point + _bezier.GetDirection(0f) * DIRECTION_MAGNITUDE);
            int steps = DISPLAY_STEPS * _bezier.PointsCount();
                
            for (int i = 1; i <= steps; i++) 
            {
                point = _bezier.GetPosition(i / (float)steps);
                Handles.DrawLine(point, point + _bezier.GetDirection(i / (float)steps) * DIRECTION_MAGNITUDE);
            }
        }
        
        private void BezierSplineDraw ()
        {
            _bezier.ControlPoints.ForEach(BezierControlPointsDraw);
        }

        private void BezierControlPointsDraw(BezierControlPoints bezierPoints)
        {
            BezierPointGizmoDraw(bezierPoints.PositionPoint);
            
            if (!bezierPoints.IsFirst && !_bezier.Loop)
            {
                BezierPointGizmoDraw(bezierPoints.Tangent0Point);
            }

            if (!bezierPoints.IsLast && !_bezier.Loop)
            {
                BezierPointGizmoDraw(bezierPoints.Tangent1Point);
            }
        }

        private void BezierPointGizmoDraw(BezierPoint point)
        {
            Vector3 gizmoPosition = _handleTransform.TransformPoint(point.Position);
            float size = HandleUtility.GetHandleSize(gizmoPosition);
            Handles.color = point.Color;
            
            if (Handles.Button(gizmoPosition, _handleRotation, size * HANDLE_SIZE, size * PICK_SIZE, Handles.DotHandleCap))
            {
                _selectedPoint = point;
                Repaint();
            }

            if (_selectedPoint != point)
            {
                return;
            }
            
            EditorGUI.BeginChangeCheck();
            gizmoPosition = Handles.DoPositionHandle(gizmoPosition, _handleRotation);

            if (!EditorGUI.EndChangeCheck())
            {
                return;
            }
            
            Undo.RecordObject(_bezier, "Move Point");
            EditorUtility.SetDirty(_bezier);
            point.Position = _handleTransform.InverseTransformPoint(gizmoPosition);
        }
    }
}

#endif
