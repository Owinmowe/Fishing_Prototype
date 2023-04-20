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
        [SerializeField] private Button HostLobby;
        [SerializeField] private Button JoinLobby;
        [SerializeField] private Button ExitButton;

        private void Start()
        {
            ExitButton.onClick.AddListener(delegate { OnExitButtonPressed?.Invoke(); });   
            HostLobby.onClick.AddListener(delegate { OnHostLobbyButtonPressed?.Invoke(); });   
            JoinLobby.onClick.AddListener(delegate { OnJoinLobbyButtonPressed?.Invoke(); });   
        }


        public void OpenScreen() => gameObject.SetActive(true);

        public void CloseScreen() => gameObject.SetActive(false);
    }
}