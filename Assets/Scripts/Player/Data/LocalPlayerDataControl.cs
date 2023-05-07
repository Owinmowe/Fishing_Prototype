using System;
using FishingPrototype.Player;
using FishingPrototype.Player.Data;
using UnityEngine;

namespace FishingPrototype.Gameplay.Boat
{
    public class LocalPlayerDataControl : MonoBehaviour, IPlayerDataControl
    {
        public Action<PlayerData> OnNewPlayerDataSet { get; set; }

        public void SetPlayerData(PlayerData data)
        {
            OnNewPlayerDataSet?.Invoke(data);
        }
    }
}
