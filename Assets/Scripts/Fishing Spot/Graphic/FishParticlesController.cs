using System;
using FishingPrototype.Gameplay.FishingSpot.Data;
using UnityEngine;

namespace FishingPrototype.Gameplay.FishingSpot
{
    public class FishParticlesController : MonoBehaviour
    {
        
        [SerializeField] private FishingSpotGraphicData graphicData;

        private IFishingSpot _fishingSpot;
        private ParticleSystem _fishParticleSystem;
        private ParticleSystem _bubblesParticleSystem;
        private ParticleSystem.MainModule _fishParticlesMainModule;

        private void Awake()
        {
            _fishingSpot = GetComponent<IFishingSpot>();
            
            _fishingSpot.OnFishingSpotSet += SetParticleSystem;
        }

        private void Start()
        {
            _fishingSpot.OnFishAmountChanged += UpdateParticleSystemAmount;
        }

        private void OnDestroy()
        {
            _fishingSpot.OnFishingSpotSet -= SetParticleSystem;
            _fishingSpot.OnFishAmountChanged -= UpdateParticleSystemAmount;
        }

        private void SetParticleSystem(FishingSpotData fishingSpotData)
        {
            _fishParticleSystem = Instantiate(graphicData.GetFishingSpotGraphicPrefab(fishingSpotData.type), transform);
            _fishParticlesMainModule = _fishParticleSystem.main;
            UpdateParticleSystemAmount(fishingSpotData.amount);
        }

        private void UpdateParticleSystemAmount(int fishAmount)
        {
            int particlesCount = _fishParticleSystem.particleCount;
            
            if (fishAmount < particlesCount)
            {
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[_fishParticleSystem.particleCount];
                _fishParticleSystem.GetParticles(particles);
                
                for (int i = fishAmount; i < particlesCount; i++)
                {
                    particles[i].startLifetime = 0f;
                    particles[i].remainingLifetime = 0f;
                }
                
                _fishParticleSystem.SetParticles(particles);
            }
            
            _fishParticlesMainModule.maxParticles = fishAmount;
        }
    }
}
