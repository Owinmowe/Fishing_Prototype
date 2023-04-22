using System;
using Mirror;

namespace FishingPrototype.Gameplay.Logic
{
    public class NetworkGameLogic : NetworkBehaviour, IGameLogic
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
            RpcStartGame();
        }

        [ClientRpc]
        private void RpcStartGame()
        {
            OnGameStarted?.Invoke();
        }

    }
}