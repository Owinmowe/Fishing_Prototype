using System;
using UnityEngine;

namespace FishingPrototype.Waves
{
    public class SinglePointFloatComponent : MonoBehaviour
    {
        private Transform _transform;
        private Vector3 _currentPosition;

        private void Start()
        {
            _transform = transform;
        }

        private void Update()
        {
            float waveHeight = WaveManager.Get().GetWaveHeight(transform.position.x);
            _currentPosition = _transform.position;
            _currentPosition.y = waveHeight;
            _transform.position = _currentPosition;
        }
    }
}

