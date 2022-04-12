using System;
using System.Collections.Generic;
using Bezier;
using NaughtyAttributes;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine;

namespace Test
{
    [ExecuteInEditMode]
    public class TubeMeshCurveGenerator : MonoBehaviour
    {
        private static readonly Vector3 UP_VECTOR = Vector3.up;
        private static readonly Vector3 RIGHT_VECTOR = Vector3.right;
        private static readonly Vector3 EPSILON_VECTOR = new (float.Epsilon, float.Epsilon, float.Epsilon);
        
        [SerializeField] private BezierSpline _bezier;
        [SerializeField] private int _sides;
        [SerializeField] private int _segments;
        [SerializeField] private float _radius;
        
        private Mesh _mesh;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Vector3[] _circleVertices;
        private Transform _transform;

        private Vector3[] _positions;
        private Vector3[] _directions;
        private Vector3[] _vertices;
        private int[] _indices;
        private Vector2[] _uv; 

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshFilter ??= gameObject.AddComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshRenderer ??= gameObject.AddComponent<MeshRenderer>();
            _transform = transform;
        }
        
        private void OnValidate()
        {
            _sides = Mathf.Max(3, _sides);
            _segments = Mathf.Max(1, _segments);
            _transform = transform;
        }
        
        private void Update()
        {
            GenerateMesh();
        }
        
        [Button("GenerateMesh")] 
        private void GenerateMesh()
        {
            GenerateCircleVertices();
            CalculatePositionsDirections();
            CalculateVertices();
            CalculateIndices();
            CalculateUVs();
            SetMesh();
        }

        private void SetMesh()
        {
            _mesh ??= new Mesh();
            
            if (_vertices.Length > _mesh.vertexCount)
            {
                _mesh.vertices = _vertices;
                _mesh.triangles = _indices;
            }
            else
            {
                _mesh.triangles = _indices;
                _mesh.vertices = _vertices;
            }
            
            _mesh.uv = _uv;
            _mesh.RecalculateTangents();
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
            _meshFilter.mesh = _mesh;
        }

        private void CalculatePositionsDirections()
        {
            _positions = new Vector3[_segments];
            _directions = new Vector3[_segments];
            
            float percentStep = 1f / _segments;
            
            for (int i = 0; i < _segments; i++)
            {
                float percent = percentStep * i;
                _positions[i] =  _bezier.GetPosition(percent);
                _directions[i] =  _bezier.GetDirection(percent);
            }
        }

        private void CalculateVertices()
        {
            _vertices = new Vector3[_sides * _segments];
            int currentVertIndex = 0;
            
            for (int i = 0; i < _segments; i++)
            {
                Vector3[] circleVertices = TransformCircleVertices(_positions[i],  Quaternion.LookRotation(_directions[i], UP_VECTOR));
                
                foreach (Vector3 vertex in circleVertices)
                {
                    _vertices[currentVertIndex++] = vertex;
                }
            }
        }
        
        private void CalculateIndices()
        {
            _indices = new int[_segments * _sides * 2 * 3]; // Two triangles and 3 vertices

            var currentIndicesIndex = 0;
            for (int segment = 1; segment < _segments; segment++)
            {
                for (int side = 0; side < _sides; side++)
                {
                    var vertIndex = (segment*_sides + side);
                    var prevVertIndex = vertIndex - _sides;
                    _indices[currentIndicesIndex++] = prevVertIndex;
                    _indices[currentIndicesIndex++] = (side == _sides - 1) ? (vertIndex - (_sides - 1)) : (vertIndex + 1);
                    _indices[currentIndicesIndex++] = vertIndex;
                    _indices[currentIndicesIndex++] = (side == _sides - 1) ? (prevVertIndex - (_sides - 1)) : (prevVertIndex + 1);
                    _indices[currentIndicesIndex++] = (side == _sides - 1) ? (vertIndex - (_sides - 1)) : (vertIndex + 1);
                    _indices[currentIndicesIndex++] = prevVertIndex;
                }
            }
        }


        private void CalculateUVs()
        {
            _uv = new Vector2[_segments * _sides];

            for (int segment = 0; segment < _segments; segment++)
            {
                for (int side = 0; side < _sides; side++)
                {
                    var vertIndex = (segment * _sides + side);
                    var u = side / (_sides - 1f);
                    var v = segment / (_segments - 1f);
                    _uv[vertIndex] = new Vector2(u, v);
                }
            }
        }
        
        private void GenerateCircleVertices()
        {
            _circleVertices = new Vector3[_sides];
            var angleStep = 2f * Mathf.PI/_sides;
            var angle = 0f;
            
            for (int i = 0; i < _sides; i++)
            {
                float x = Mathf.Cos(angle);
                float y = Mathf.Sin(angle);
                _circleVertices[i] = -UP_VECTOR *x* _radius + RIGHT_VECTOR *y* _radius;
                angle += angleStep;
            }
        }

        private Vector3[] TransformCircleVertices(Vector3 position, Quaternion rotation)
        {
            Vector3[] circleVertices = new Vector3[_sides];

            for (var i = 0; i < _sides; i++)
            {
                circleVertices[i] = rotation * _circleVertices[i];
                circleVertices[i] += position;
            }

            return circleVertices;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < _positions.Length; i++)
            {
                Gizmos.DrawLine(_positions[i], _positions[i] + _directions[i] * 3f );
            }
        }
    }
}