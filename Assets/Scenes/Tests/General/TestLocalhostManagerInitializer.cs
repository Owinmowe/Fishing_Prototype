using System;
using Mirror;
using UnityEngine;

namespace FishingPrototype.Test
{
    public class TestLocalhostManagerInitializer : MonoBehaviour
    {

        [SerializeField] private Initialization initialization;
        
        private NetworkManager _manager;
        
        private void Awake()
        {
            _manager = GetComponent<NetworkManager>();
        }

        private void Start()
        {
            if(initialization == Initialization.Host)
                _manager.StartHost();
            else// if(initialization == Initialization.Client)
                _manager.StartClient();
        }

        [Serializable]
        public enum Initialization
        {
            Host,
            Client
        }
    }
}
