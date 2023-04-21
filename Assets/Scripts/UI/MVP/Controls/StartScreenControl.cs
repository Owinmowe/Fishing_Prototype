using FishingPrototype.Network;
using UnityEngine;
using UnityEngine.UI;

namespace FishingPrototype.MVP.Control
{
    public class StartScreenControl : MonoBehaviour, IScreenControl
    {

        public event System.Action OnExitButtonPressed;
        public event System.Action OnHostLobbyButtonPressed;
        public event System.Action OnJoinLobbyButtonPressed;
        
        [Header("Buttons")]
        [SerializeField] private Button HostLobbyButton;
        [SerializeField] private Button JoinLobbyButton;
        [SerializeField] private Button ExitButtonButton;
        [Header("References")]
        [SerializeField] private CustomNetworkManager customNetworkManager;
        
        private void Start()
        {
            HostLobbyButton.onClick.AddListener(delegate { OnHostLobbyButtonPressed?.Invoke(); });   
            JoinLobbyButton.onClick.AddListener(delegate { OnJoinLobbyButtonPressed?.Invoke(); });   
            ExitButtonButton.onClick.AddListener(delegate { OnExitButtonPressed?.Invoke(); });   
        }


        public void OpenScreen() => gameObject.SetActive(true);

        public void CloseScreen() => gameObject.SetActive(false);
        
        public void HostLobby() => customNetworkManager.RequestCreateLobby(publicLobby: true);
    }
}