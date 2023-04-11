using System;
using UnityEngine;

namespace FishingPrototype.Waves
{
    public class MultiPointFloatComponent : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private Transform[] floatPoints;
        [SerializeField] private Transform transformToAlign;
        [Header("Update Float Logic")]
        [SerializeField] private float smoothTime;
        
        private Transform _thirdHighestFloatPoint;
        private Transform _secondHighestFloatPoint;
        private Transform _highestFloatPoint;
        
        private Plane _alignmentPlane;
        private Vector3 _alignmentNormal;
        private Vector3 _alignmentForward;
        
        private Vector3 _currentPosition;
        private Vector3 _auxiliaryVector;
        
        private Vector3 _smoothVelocity = Vector3.zero;

        private void Start()
        {
            SetStartingFloatPoint();
            SetStartingAlignmentPlane();
        }

        private void Update()
        {
            if (floatPoints.Length > 2)
            {
                SetFloatPointsForAlignmentPlane();
                SetAlignmentPlane();
                AlignTransformWithPlane();
            }

            AlignCenterWithWave();
        }

        private void AlignCenterWithWave()
        {
            _currentPosition = transformToAlign.position;
            _currentPosition.y = WaveManager.Get().GetWaveHeight(_currentPosition.x);
            transformToAlign.position = _currentPosition;
        }

        private void SetStartingFloatPoint()
        {
            if (floatPoints.Length > 2)
            {
                if (_thirdHighestFloatPoint == null)
                    _thirdHighestFloatPoint = floatPoints[0];

                if (_secondHighestFloatPoint == null)
                    _secondHighestFloatPoint = floatPoints[0];
                
                if (_highestFloatPoint == null)
                    _highestFloatPoint = floatPoints[0];
            }
        }

        private void SetStartingAlignmentPlane()
        {
            _alignmentPlane = new Plane(transformToAlign.up, transformToAlign.position);
        }
        
        private void SetFloatPointsForAlignmentPlane()
        {
            SetFloatPointPositions();
            Array.Sort(floatPoints, CompareByY);
            
            _highestFloatPoint = floatPoints[^1];
            _secondHighestFloatPoint = floatPoints[^2];
            _thirdHighestFloatPoint = floatPoints[^3];
        }

        private int CompareByY(Transform t1, Transform t2) {
            if (t1.position.y < t2.position.y)
                return -1;
            if (t1.position.y > t2.position.y)
                return 1;
            return 0;
        }
        
        private void SetFloatPointPositions()
        {
            foreach (var floatPoint in floatPoints)
            {
                _auxiliaryVector = floatPoint.position;
                _auxiliaryVector.y = WaveManager.Get().GetWaveHeight(_auxiliaryVector.x);
                floatPoint.position = _auxiliaryVector;
            }
        }

        private void SetAlignmentPlane()
        {
            _alignmentPlane.Set3Points(_thirdHighestFloatPoint.position, _secondHighestFloatPoint.position, _highestFloatPoint.position);
        }

        private void AlignTransformWithPlane()
        {
            _alignmentNormal = _alignmentPlane.normal;
            if (_alignmentNormal.y < 0)
                _alignmentNormal *= -1;
            
            Vector3 newUp = Vector3.SmoothDamp(transformToAlign.up, _alignmentNormal, ref _smoothVelocity, smoothTime);
            transformToAlign.up = newUp;
            transformToAlign.Rotate(0, -transformToAlign.localEulerAngles.y, 0);

            DebugDrawPlane();
        }
        
        private Vector3 GetCenterPoint()
        {
            Vector3 total = (_highestFloatPoint.position + _secondHighestFloatPoint.position + _thirdHighestFloatPoint.position) / 3;
            return total;
        }
        
        private void DebugDrawPlane()
        {
            Debug.DrawLine(_thirdHighestFloatPoint.position, _secondHighestFloatPoint.position, Color.red);
            Debug.DrawLine(_secondHighestFloatPoint.position, _highestFloatPoint.position, Color.red);
            Debug.DrawLine(_highestFloatPoint.position, _thirdHighestFloatPoint.position, Color.red);
            Debug.DrawLine(GetCenterPoint(), GetCenterPoint() + _alignmentNormal, Color.magenta);
        }
    }
}
