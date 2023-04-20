using System;
using FishingPrototype.Boat.Data;
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
