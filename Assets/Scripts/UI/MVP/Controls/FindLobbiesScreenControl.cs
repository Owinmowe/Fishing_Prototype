using UnityEngine;

namespace FishingPrototype.MVP.Control
{
    public class FindLobbiesScreenControl : MonoBehaviour, IScreenControl
    {
        public void OpenScreen() => gameObject.SetActive(true);
        public void CloseScreen() => gameObject.SetActive(false);
    }
}