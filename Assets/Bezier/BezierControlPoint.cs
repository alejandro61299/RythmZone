using System;
using Bezier.ModeDriver;
using Bezier.Points;
using UnityEngine;

namespace Bezier
{
    [Serializable]
    public class BezierControlPoint
    {
        [SerializeField] private BezierPointMode _mode;
        [SerializeField] private BezierMainPoint _main;
        [SerializeField] private BezierTangent0Point _tangent0;
        [SerializeField] private BezierTangent1Point _tangent1; 
        private BezierControlPointModeDriver _modeDriver;
        
        public BezierPointMode Mode => _mode;
        public BezierMainPoint Main => _main;
        public BezierTangent0Point Tangent0 => _tangent0;
        public BezierTangent1Point Tangent1 => _tangent1;

        public BezierControlPoint()  
        {
            _main = new BezierMainPoint(this);
            _tangent0 =new BezierTangent0Point(this);
            _tangent1 = new BezierTangent1Point(this);
        }

        public void Initialize()
        {
            SetMode(Mode);
        }

        public void SetMode(BezierPointMode mode)
        {
            _mode = mode;
            _modeDriver = BezierControlPointModeDriver.InstantiateDriver(this, _modeDriver);
            _modeDriver.ForceDrive(); 
        }
    }
}