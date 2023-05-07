namespace FishingPrototype.Gameplay.GameMode
{
    public class PveGameMode : GameModeBase
    {
        protected override void StartGameModeInternal()
        {
            CallSpawnAllFishingSpot();
        }
        
        protected override void RemovePlayerInternal()
        {
            
        }
    }
}
