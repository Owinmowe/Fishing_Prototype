using System;

namespace FishingPrototype.MVP.Presenter
{
    public abstract class Presenter<V> : IDisposable
    {
        protected readonly V view;
            
        protected Presenter (V view)
        {
            this.view = view;
            AddViewListeners();
        }

        public void Dispose ()
        {
            RemoveViewListeners();
        }

        protected abstract void AddViewListeners ();
        protected abstract void RemoveViewListeners ();
    }
}

