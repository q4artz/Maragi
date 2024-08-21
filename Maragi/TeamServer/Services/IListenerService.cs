using TeamServer.Modules;
using Microsoft.AspNetCore.Mvc;


namespace TeamServer.Services
{
    public interface IListenerService
    {
        void AddListener(Listener listener);

        // can return a list / array / any type
        IEnumerable<Listener> GetListeners();

        Listener GetListener(string name);

        void RemoveListener(Listener listener);
    }

    public class ListenerService : IListenerService
    {
        private readonly List<Listener> _listeners = new();
        public void AddListener(Listener listener)
        {
            _listeners.Add(listener);
        }

        public Listener GetListener(string name)
        {
            return GetListeners().FirstOrDefault(l => l.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Listener> GetListeners()
        {
            return _listeners;
        }

        public void RemoveListener(Listener listener)
        {
            _listeners.Remove(listener);
        }
    }
}
