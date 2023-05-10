namespace FishingPrototype.Gameplay.GameMode
{
    public class PveGameMode : GameModeBase
    {
        protected override void StartGameModeInternal()
        {
            CallChangeMap();
        }
        
        protected override void RemovePlayerInternal()
        {
            
        }
    }
}
