using FishingPrototype.Gameplay.Maps.Data;

namespace FishingPrototype.Gameplay.FishingSpot.Data
{
    [System.Serializable]
    public struct FishingSpotData
    {
        public int amount;
        public FishingSpotType type;
        public int spawnIndex;
        public SpawnDifficulty spawnDifficulty;
    }
}