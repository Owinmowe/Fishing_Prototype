using FishingPrototype.Player.Data;

namespace FishingPrototype.Player
{
    public interface IPlayerDataControl
    {
        System.Action<PlayerData> OnNewPlayerDataSet { get; set; }
        void SetPlayerData(PlayerData data);
    }
}