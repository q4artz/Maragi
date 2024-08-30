using Maragi_Framework.Models.Listeners;
using System.Collections.Generic;
using System.Linq;

namespace Maragi_Framework.Services
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

        public IEnumerable<Listener> GetListeners()
        {
            return _listeners;
        }

        public Listener GetListener(string name)
        {
            return GetListeners().FirstOrDefault(l => l.Name.Equals(name));
        }

        public void RemoveListener(Listener listener)
        {
            _listeners.Remove(listener);
        }
    }
}