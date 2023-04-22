using FishingPrototype.Boat.Data;
using FishingPrototype.UI;
using UnityEngine;
using UnityEngine.UI;

namespace FishingPrototype.MVP.Control
{
    public class LobbyScreenControl : MonoBehaviour, IScreenControl
    {
        public event System.Action OnStartButtonPressed;
        
        [SerializeField] private ConnectedPanelUI[] connectedPanelUis;
        [SerializeField] private Button startGameButton;
        public void OpenScreen() => gameObject.SetActive(true);
        public void CloseScreen() => gameObject.SetActive(false);

        private void Start()
        {
            startGameButton.onClick.AddListener(delegate { OnStartButtonPressed?.Invoke(); });
        }

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