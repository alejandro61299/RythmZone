using UnityEngine;

namespace Bezier.Utils
{
    public class BezierFrame
    {
        private readonly Vector3 _position;
        private readonly Vector3 _right;
        private readonly Vector3 _up;
        private readonly Vector3 _forward;

        public Vector3 Position => _position;
        public Vector3 Right => _right;
        public Vector3 Up => _up;
        public Vector3 Forward => _forward;

        public BezierFrame(Vector3 position, Vector3 right, Vector3 up)
        {
            _forward = Vector3.Cross(right, up).normalized;
            _position = position;
            _right = right;
            _up = up;
        }
    }
}