using System.Collections.Generic;
using FishingPrototype.MVP.Data;
using FishingPrototype.MVP.View;
using FishingPrototype.Network.Data;

namespace FishingPrototype.MVP.Presenter
{
    public class FindLobbiesPresenter: Presenter<GameView>
    {
        public FindLobbiesPresenter(GameView view, FindLobbiesData scriptableData) : base(view)
        {
            //view.RefreshLobbiesList();
        }

        protected override void AddViewListeners()
        {
            view.OnFindLobbiesEvent += OnLobbiesGetEvent;
            view.OnJoinLobbyEvent += OnJoinLobby;
        }

        protected override void RemoveViewListeners()
        {
            view.OnFindLobbiesEvent -= OnLobbiesGetEvent;
            view.OnJoinLobbyEvent -= OnJoinLobby;
        }

        private void OnLobbiesGetEvent(List<SteamLobbyData> lobbiesData)
        {
            
        }

        private void OnJoinLobby()
        {
            view.CloseFindLobbiesScreen();
            view.OpenLobbyScreen();
        }
    }
}