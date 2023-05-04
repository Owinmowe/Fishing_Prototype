using FishingPrototype.Waves;
using UnityEngine;

namespace FishingPrototype.Gameplay.Maps.Data
{
    [CreateAssetMenu(fileName = "Map Data", menuName = "Gameplay Data/Maps/Map Data", order = 1)]
    public class MapData : ScriptableObject
    {
        [Header("Map General")]
        public string mapName;
        public MapObject mapObject;
        
        [Header("Water")]
        public Material waterMaterial;
        public WaveConfiguration waveConfiguration;
    }
}
