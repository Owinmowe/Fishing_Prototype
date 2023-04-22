using FishingPrototype.Boat.Data;
using FishingPrototype.UI;
using UnityEngine;

namespace FishingPrototype.MVP.Control
{
    public class LobbyScreenControl : MonoBehaviour, IScreenControl
    {
        [SerializeField] private ConnectedPanelUI[] connectedPanelUis;
        public void OpenScreen() => gameObject.SetActive(true);
        public void CloseScreen() => gameObject.SetActive(false);

        public void SetConnectedPlayersPanel(PlayerData[] connectedPlayersData)
        {
            if (!gameObject.activeSelf) return;
            
            for (int i = 0; i < connectedPanelUis.Length; i++)
            {
                bool panelActive = i < connectedPlayersData.Length;
                connectedPanelUis[i].SetPanelState(panelActive);
                if(panelActive)
                    connectedPanelUis[i].SetPanelData(connectedPlayersData[i].nickname, connectedPlayersData[i].iImage);
            }
        }
    }
}