using FishingPrototype.Boat.Data;

namespace FishingPrototype.Gameplay.Boat
{
    public interface IPlayerDataControl
    {
        System.Action<PlayerData> OnNewPlayerDataSet { get; set; }
        void SetPlayerData(PlayerData data);
    }
}