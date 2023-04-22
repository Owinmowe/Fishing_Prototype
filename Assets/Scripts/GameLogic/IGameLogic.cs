namespace FishingPrototype.Gameplay.Logic
{
    public interface IGameLogic
    {
        public static System.Action<IGameLogic> OnGameLogicSet;
        public System.Action OnGameStarted { get; set; }
        public System.Action OnGameEnded { get; set; }
        public void StartGame();
    }
}
