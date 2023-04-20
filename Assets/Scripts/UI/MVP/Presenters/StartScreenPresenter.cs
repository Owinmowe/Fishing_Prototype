using FishingPrototype.MVP.View;
using FishingPrototype.MVP.Data;

namespace FishingPrototype.MVP.Presenter
{
    public class StartScreenPresenter : Presenter<GameView>
    {
        private StartScreenData _data;
        
        public StartScreenPresenter(GameView view, StartScreenData scriptableData) : base(view)
        {
            _data = scriptableData;
        }

        protected override void AddViewListeners()
        {
            view.OnHostLobbyButtonEvent += OnHostLobby;
            view.OnJoinLobbyButtonEvent += OnJoinLobby;
            view.OnExitButtonEvent += OnExit;
        }

        protected override void RemoveViewListeners()
        {
            view.OnHostLobbyButtonEvent -= OnHostLobby;
            view.OnJoinLobbyButtonEvent -= OnJoinLobby;
            view.OnExitButtonEvent -= OnExit;
        }

        private void OnHostLobby()
        {
            view.CloseStartScreen();
            view.OpenLobbyScreen();
            view.HostLobby();
        }
        
        private void OnJoinLobby()
        {
            view.CloseStartScreen();
            view.OpenFindLobbiesScreen();;
        }
        
        private void OnExit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); //TODO agregar boton de preguntar si estas seguro
#endif
        }
    }
}
