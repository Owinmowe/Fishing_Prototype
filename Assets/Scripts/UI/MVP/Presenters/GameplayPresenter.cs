using FishingPrototype.MVP.Data;
using FishingPrototype.MVP.View;

namespace FishingPrototype.MVP.Presenter
{
    public class GameplayPresenter: Presenter<GameView>
    {
        private GameplayData _scriptableData;
        
        public GameplayPresenter(GameView view, GameplayData scriptableData) : base(view)
        {
            _scriptableData = scriptableData;
        }

        protected override void AddViewListeners()
        {
            
        }

        protected override void RemoveViewListeners()
        {
            
        }
    }
}