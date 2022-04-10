using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Test
{
    [ExecuteInEditMode]
    public class RendererPoint : MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        public Vector3 GetPosition()
        {
            return _transform.position;
        }

#if UNITY_EDITOR
        
        private static readonly float RADIUS = 1.2f;
        private static readonly Color GIZMO_COLOR = new Color(0f, 1f, 0f, 0.2f);
        private void OnDrawGizmos()
        {
            Gizmos.color = GIZMO_COLOR;
            GUIStyle style = GUIStyle.none;
            style.fontSize = 36;
            style.normal.textColor = Color.white;
            var position = _transform.position;
            Handles.Label(position + Vector3.up * RADIUS * 0.5F, _transform.GetSiblingIndex().ToString(), style);
            Gizmos.DrawSphere(position, RADIUS);
            
        }
        
#endif

    }
    
    
}