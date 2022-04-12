#if UNITY_EDITOR

using System.Linq;
using Bezier.Points;
using Bezier.Utils;
using UnityEditor;
using UnityEngine;

namespace Bezier
{
    [CustomEditor(typeof(BezierSpline))]
    
    public class BezierSplineInspector : Editor
    {
        private const int DISPLAY_STEPS = 5;
        private const float DIRECTION_MAGNITUDE = 0.5f;
        private const float HANDLE_SIZE = 0.1f;
        private const float PICK_SIZE = 0.12f;
        
        private BezierSpline _bezier;
        private Transform _bezierTransform;
        private Quaternion _bezierRotation;
        private BezierPoint _selectedPoint;
        private BezierControlPoint _firstPoint;
        private BezierControlPoint _lastPoint;
        
        private void OnEnable()
        {
            _bezier = (BezierSpline)target;
            _firstPoint = _bezier.FirstControlPoint();
            _lastPoint = _bezier.LastControlPoint();
            UpdateParameters();
        }

        public override void OnInspectorGUI () 
        {
            EditorGUI.BeginChangeCheck();
            bool loop = EditorGUILayout.Toggle("Loop", _bezier.Loop);
            
            if (EditorGUI.EndChangeCheck()) 
            {
                Undo.RecordObject(_bezier, "Toggle Loop");
                EditorUtility.SetDirty(_bezier);
                _bezier.SetLoop(loop);
            }
            if (_selectedPoint != null) 
            {
                DrawSelectedPointInspector();
            }
            
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Add Prev")) 
            {
                Undo.RecordObject(_bezier, "Add Point");
                _bezier.AddControlPoint();
                EditorUtility.SetDirty(_bezier);
            }
            if (GUILayout.Button("Add Next")) 
            {
                Undo.RecordObject(_bezier, "Add Point");
                _bezier.AddControlPoint();
                EditorUtility.SetDirty(_bezier);
            }
            
            GUILayout.EndHorizontal();
            
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
            BezierPointMode mode = (BezierPointMode)EditorGUILayout.EnumPopup("Mode", _selectedPoint.ControlPoint.Mode);
            
            if (EditorGUI.EndChangeCheck()) 
            {
                Undo.RecordObject(_bezier, "Change Point Mode");
                _selectedPoint.ControlPoint.SetMode(mode);
                EditorUtility.SetDirty(_bezier);
            }
        }
        
        private void OnSceneGUI ()
        {
            UpdateParameters();
            BezierSplineGUI();
        }

        private void UpdateParameters()
        {
            _bezierTransform = _bezier.transform;
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
            BezierControlPoint point0 = _bezier.ControlPoints.First();
            
            for (int i = 1 ; i < _bezier.ControlPoints.Count ; ++i)
            {
                BezierControlPoint point1 = _bezier.ControlPoints[i];
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
        
        private void BezierTangentsDraw(BezierControlPoint bezierPoint)
        {
            Vector3 position = _bezierTransform.TransformPoint(bezierPoint.Main.Position);
            Vector3 tangent0 = _bezierTransform.TransformPoint(bezierPoint.Tangent0.Position);
            Vector3 tangent1 = _bezierTransform.TransformPoint(bezierPoint.Tangent1.Position);
            
            if (_bezier.Loop || !_bezier.Loop && _firstPoint != bezierPoint)
            {
                Handles.DrawLine(tangent0, position);
            }
            if (_bezier.Loop || !_bezier.Loop && _lastPoint != bezierPoint)
            {
                Handles.DrawLine(tangent1, position);
            }
        }
        
        private void BezierCurveDraw(BezierControlPoint startPoint, BezierControlPoint endPoint)
        {
            Vector3 startPosition = _bezierTransform.TransformPoint(startPoint.Main.Position);
            Vector3 endPosition = _bezierTransform.TransformPoint(endPoint.Main.Position);
            Vector3 startTangent = _bezierTransform.TransformPoint(startPoint.Tangent1.Position); 
            Vector3 endTangent = _bezierTransform.TransformPoint(endPoint.Tangent0.Position);
             
            Handles.color = Color.gray;
            Handles.DrawBezier(startPosition, endPosition, startTangent, endTangent, Color.white, null, 2f);
        }

        private void BezierDirectionsDraw () 
        {
            Handles.color = Color.green;

            int steps = DISPLAY_STEPS * _bezier.ControlPointsCount();
                
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

        private void BezierControlPointsDraw(BezierControlPoint bezierPoint)
        {
            BezierPointGizmoDraw(bezierPoint.Main);
            BezierPointLabelDraw(bezierPoint.Main, _bezier.ControlPoints.IndexOf(bezierPoint));

            if (_bezier.Loop || !_bezier.Loop && _firstPoint != bezierPoint)
            {
                BezierPointGizmoDraw(bezierPoint.Tangent0);
            }
            if (_bezier.Loop || !_bezier.Loop && _lastPoint != bezierPoint)
            {
                BezierPointGizmoDraw(bezierPoint.Tangent1);
            }
        }

        private void BezierPointLabelDraw(BezierPoint point, int index)
        {
            GUIStyle style = GUIStyle.none;
            style.fontSize = 36;
            style.normal.textColor = Color.white;
            Vector3 labelPosition = _bezierTransform.TransformPoint(point.Position) + Vector3.up * 0.5F;
            Handles.Label(labelPosition, index.ToString(), style);
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
