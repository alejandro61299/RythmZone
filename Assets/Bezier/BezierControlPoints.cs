using System;
using Bezier.ModeDriver;
using Bezier.Points;
using UnityEngine;

namespace Bezier
{
    [Serializable]
    public class BezierControlPoints
    { 
        private BezierPointMode _mode;
        [SerializeField] private bool _isFirst;
        [SerializeField] private bool _isLast;
        
        private BezierMainPoint _main;
        private BezierTangent0Point _tangent0;
        private BezierTangent1Point _tangent1;
        private BezierControlPointModeDriver _modeDriver;
        
        public BezierPointMode Mode => _mode;

        public BezierMainPoint Main => _main;
        public BezierTangent0Point Tangent0 => _tangent0;
        public BezierTangent1Point Tangent1 => _tangent1;
        public bool IsFirst => _isFirst;
        public bool IsLast => _isLast;

        public BezierControlPoints(bool isFirst = false , bool isLast = false)
        {

            _isFirst = isFirst;
            _isLast = isLast; 
            _main = new BezierMainPoint(this);
            _tangent0 =new BezierTangent0Point(this);
            _tangent1 = new BezierTangent1Point(this);
            _modeDriver = BezierControlPointModeDriver.InstantiateDriver(this);

        }

        public void SetMode(BezierPointMode mode)
        {
            _mode = mode;
            _modeDriver = BezierControlPointModeDriver.InstantiateDriver(this, _modeDriver);
        }
    }
}