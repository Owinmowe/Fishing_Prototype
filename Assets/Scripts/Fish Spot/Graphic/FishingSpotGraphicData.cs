using System;
using UnityEngine;

namespace FishingPrototype.Gameplay.FishingSpot
{
    [CreateAssetMenu(fileName = "Fishing Spot Graphic Data", menuName = "Fishing Spots/Graphic Data", order = 1)]
    public class FishingSpotGraphicData : ScriptableObject
    {
        [HideInInspector] public ParticleSystem[] fishingSpotGraphicPrefabs = new ParticleSystem[Enum.GetValues(typeof(FishingSpotType)).Length];

        public ParticleSystem GetFishingSpotGraphicPrefab(FishingSpotType type)
        {
            return fishingSpotGraphicPrefabs[(int)type];
        }
    }
}
