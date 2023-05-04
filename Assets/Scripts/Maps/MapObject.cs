using System.Collections.Generic;
using UnityEngine;

namespace FishingPrototype.Gameplay.Maps
{
    public class MapObject : MonoBehaviour
    {
        [Header("Spawn Positions")]
        [SerializeField] private List<Transform> playersSpawnPosition;
        [SerializeField] private List<Transform> easyFishSpawnPositions;
        [SerializeField] private List<Transform> mediumFishSpawnPositions;
        [SerializeField] private List<Transform> hardFishSpawnPositions;
        [SerializeField] private Transform bossSpawnPosition;
        [SerializeField] private float spawnDistanceVariance;
        
        public List<Transform> PlayersSpawnPosition => playersSpawnPosition;
        public List<Transform> EasyFishSpawnPositions => easyFishSpawnPositions;
        public List<Transform> MediumFishSpawnPositions => mediumFishSpawnPositions;
        public List<Transform> HardFishSpawnPositions => hardFishSpawnPositions;
        public Transform BossSpawnPosition => bossSpawnPosition;
        public float SpawnDistanceVariance => spawnDistanceVariance;
    }
}
