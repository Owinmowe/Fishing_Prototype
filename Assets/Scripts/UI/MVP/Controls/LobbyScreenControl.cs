using FishingPrototype.Network;
using UnityEngine;

namespace FishingPrototype.MVP.Control
{
    public class LobbyScreenControl : MonoBehaviour, IScreenControl
    {
        public void OpenScreen() => gameObject.SetActive(true);
        public void CloseScreen() => gameObject.SetActive(false);
    }
}