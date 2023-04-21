using FishingPrototype.Network;
using UnityEngine;

namespace FishingPrototype.MVP.Control
{
    public class LobbyScreenControl : MonoBehaviour, IScreenControl
    {
        [SerializeField] private CustomNetworkManager customNetworkManager;
        
        public void OpenScreen() => gameObject.SetActive(true);
        public void CloseScreen() => gameObject.SetActive(false);
    }
}