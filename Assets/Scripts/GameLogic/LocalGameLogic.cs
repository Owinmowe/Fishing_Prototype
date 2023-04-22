using System;
using UnityEngine;

namespace FishingPrototype.Gameplay.Logic
{
    public class LocalGameLogic : MonoBehaviour, IGameLogic
    {
        public Action OnGameStarted { get; set; }
        public Action OnGameEnded { get; set; }
        
        private void Start()
        {
            IGameLogic.OnGameLogicSet?.Invoke(this);
        }

        public void StartGame()
        {
            OnGameStarted?.Invoke();
        }

    }
}