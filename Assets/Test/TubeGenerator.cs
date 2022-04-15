using System.Linq;
using Bezier;
using Bezier.Utils;
using NaughtyAttributes;
using UnityEngine;
using Utils.MathR;

namespace Test
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshRenderer))] 
    [RequireComponent(typeof(MeshFilter))] 
    [RequireComponent(typeof(BezierSpline))] 
    public class TubeGenerator : MonoBehaviour
    {
        [SerializeField] private int _sides;
        [SerializeField] private int _segments;
        [SerializeField] private float _radius;
        
        private Mesh _mesh;
        private MeshFilter _meshFilter;
        private BezierSpline _bezier;
        private Vector3[] _circleVertices;
        private BezierFrame[] _bezierFrames; 
        private Vector3[] _vertices;
        private int[] _indices;
        private Vector2[] _uv;

        private void Awake()
        {
            _meshFilter = gameObject.GetComponent<MeshFilter>();
            _bezier = gameObject.GetComponent<BezierSpline>();
        }
        
        private void OnValidate()
        {
            _sides = Mathf.Max(3, _sides);
            _segments = Mathf.Max(1, _segments);
        }
        

        [Button("GenerateMesh")] 
        private void GenerateMesh()
        {
            GenerateCircleVertices();
            CalculateBezierFrames();
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

        private void CalculateBezierFrames()
        {
            float[] uValues =  GetSegmentValues();
            Vector3[] positions = uValues.Select(_bezier.GetPosition).ToArray(); 
            Vector3[] directions = uValues.Select(_bezier.GetDirection).ToArray();

            Vector3 origin = positions.First();
            Vector3 crvTan = directions.First();
            Vector3 crvNormal = crvTan.PerpendicularTo();
            Vector3 yAxis = Vector3.Cross(crvTan, crvNormal);
            Vector3 xAxis = Vector3.Cross(yAxis, crvTan);
            
            _bezierFrames = new BezierFrame[positions.Length];
            _bezierFrames[0] = new BezierFrame(origin, xAxis, yAxis);
            
            for (int i = 0; i < positions.Length - 1; i++)
            {
                Vector3 v1 = positions[i + 1] - positions[i];
                float c1 = v1.VectorScalar(v1);
                Vector3 rLi = _bezierFrames[i].Right - (2f / c1) *  v1.VectorScalar(_bezierFrames[i].Right) * v1;
                Vector3 tLi = directions[i] - (2f / c1) * v1.VectorScalar(directions[i]) * v1; 
                Vector3 v2 = directions[i + 1] - tLi;
                float c2 = v2.VectorScalar(v2);
                Vector3 rNext = rLi - (2f / c2) * v2.VectorScalar(rLi) * v2; 
                var sNext = Vector3.Cross(directions[i + 1], rNext);
                BezierFrame bezierFrameNext = new BezierFrame( positions[i + 1], rNext, sNext);
                _bezierFrames[i + 1] = bezierFrameNext;
            }
        }


        private float[] GetSegmentValues()
        {
            float percentStep = 1f / _segments;
            float[] values = new float[_segments];
            
            for (int i = 0; i < _segments; i++)
            {
                values[i] = percentStep * i;
            }

            return values;
        }

        private void CalculateVertices()
        {
            int currentVertIndex = 0;
            _vertices = new Vector3[_sides * _segments];

            foreach (BezierFrame axisOrigin in _bezierFrames)
            {
                Vector3[] circleVertices = TransformCircleVertices(axisOrigin.Position, Quaternion.LookRotation(axisOrigin.Forward, axisOrigin.Up));
                
                foreach (Vector3 vertex in circleVertices)
                {
                    _vertices[currentVertIndex++] = vertex;
                }
            }
        }
        
        private void CalculateIndices()
        {
            _indices = new int[_segments * _sides * 2 * 3];

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
                _circleVertices[i] = -Vector3.up *x* _radius + Vector3.right *y* _radius;
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
            if (_bezierFrames == null)
            {
                return;
            }
            
            foreach (BezierFrame frame in _bezierFrames)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(frame.Position, frame.Position + frame.Up * 2f);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(frame.Position, frame.Position + frame.Right * 2f);
            }
        }
    }
}