#if UNITY_EDITOR

using System;
using System.Linq;
using Bezier.Points;
using UnityEditor;
using UnityEngine;

namespace Bezier
{
    [CustomEditor(typeof(MyBezierSpline))]
    
    public class MyBezierSplineInspector : Editor
    {
        private const int DISPLAY_STEPS = 5;
        private const float DIRECTION_MAGNITUDE = 0.5f;
        private const float HANDLE_SIZE = 0.1f;
        private const float PICK_SIZE = 0.12f;
        
        private MyBezierSpline _bezier;
        private Transform _bezierTransform;
        private Quaternion _bezierRotation;
        private BezierPoint _selectedPoint;


        private void OnEnable()
        {
            _bezier = (MyBezierSpline)target;
        }

        public override void OnInspectorGUI () 
        {
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
                _selectedPoint.SetPoint(point);
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
            UpdateTransform();
            BezierSplineGUI();
        }

        private void UpdateTransform()
        {
            _bezierTransform = _bezier.Transform;
            _bezierRotation =  UnityEditor.Tools.pivotRotation == PivotRotation.Local 
                ? _bezierTransform.rotation 
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
        
        private void BezierTangentsDraw(BezierControlPoints bezierPoints)
        {
            Vector3 position = _bezierTransform.TransformPoint(bezierPoints.Main.Position);
            Vector3 tangent0 = _bezierTransform.TransformPoint(bezierPoints.Tangent0.Position);
            Vector3 tangent1 = _bezierTransform.TransformPoint(bezierPoints.Tangent1.Position);
            
            if (_bezier.Loop || !_bezier.Loop && !bezierPoints.IsFirst)
            {
                Handles.DrawLine(tangent0, position);
            }
            if (_bezier.Loop || !_bezier.Loop && !bezierPoints.IsLast)
            {
                Handles.DrawLine(tangent1, position);
            }
        }
        
        private void BezierCurveDraw(BezierControlPoints startPoints, BezierControlPoints endPoints)
        {
            Vector3 startPosition = _bezierTransform.TransformPoint(startPoints.Main.Position);
            Vector3 endPosition = _bezierTransform.TransformPoint(endPoints.Main.Position);
            Vector3 startTangent = _bezierTransform.TransformPoint(startPoints.Tangent1.Position); 
            Vector3 endTangent = _bezierTransform.TransformPoint(endPoints.Tangent0.Position);
             
            Handles.color = Color.gray;
            Handles.DrawBezier(startPosition, endPosition, startTangent, endTangent, Color.white, null, 2f);
        }

        private void BezierDirectionsDraw () 
        {
            Handles.color = Color.green;

            int steps = DISPLAY_STEPS * _bezier.PointsCount();
                
            for (int i = 1; i <= steps; i++) 
            {
                Vector3 point = _bezier.GetPosition(i / (float)steps);
                Handles.DrawLine(point, point + _bezier.GetDirection(i / (float)steps) * DIRECTION_MAGNITUDE);
            }
        }
        
        private void BezierSplineDraw ()
        {
            _bezier.ControlPoints.ForEach(BezierControlPointsDraw);
        }

        private void BezierControlPointsDraw(BezierControlPoints bezierPoints)
        {
            BezierPointGizmoDraw(bezierPoints.Main);
            
            if (_bezier.Loop || !_bezier.Loop && !bezierPoints.IsFirst)
            {
                BezierPointGizmoDraw(bezierPoints.Tangent0);
            }
            if (_bezier.Loop || !_bezier.Loop && !bezierPoints.IsLast)
            {
                BezierPointGizmoDraw(bezierPoints.Tangent1);
            }
        }

        private void BezierPointGizmoDraw(BezierPoint point)
        {
            Vector3 gizmoPosition = _bezierTransform.TransformPoint(point.Position);
            float size = HandleUtility.GetHandleSize(gizmoPosition);
            Handles.color = point.Color;
            
            if (Handles.Button(gizmoPosition, _bezierRotation, size * HANDLE_SIZE, size * PICK_SIZE, Handles.DotHandleCap))
            {
                _selectedPoint = point;
                Repaint();
            }

            if (_selectedPoint != point)
            {
                return;
            }
            
            EditorGUI.BeginChangeCheck();
            gizmoPosition = Handles.DoPositionHandle(gizmoPosition, _bezierRotation);

            if (!EditorGUI.EndChangeCheck())
            {
                return;
            }
            
            Undo.RecordObject(_bezier, "Move Point");
            EditorUtility.SetDirty(_bezier);
            point.SetPoint(_bezierTransform.InverseTransformPoint(gizmoPosition));
        }
    }
}

#endif
