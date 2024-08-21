using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TeamServer.Services;

namespace TeamServer.Controllers
{
    [ApiController]
    public class ListenersController : ControllerBase
    {
        // write API to interact with Listener Service

        private readonly IListenerService _listeners;

        public  ListenersController(IListenerService listeners)
        {
            _listeners = listeners;
        }

        [HttpGet]
        public IActionResult GetListeners()
        {
            var listeners = _listeners.GetListeners;
            return Ok(listeners);
        }

        // will be populated with swagger -- swagger will pick this up
        // if name match it will know GetListener's string "name" is HttpGet "name"
        [HttpGet("(name)")]
        public IActionResult GetListener(string name)
        {
            // in IListenerService we set FirstOrDefault == if listener don't exist it will return null
            var listener = _listeners.GetListener(name);

            if (listener is null) return NotFound();
            
            return Ok(listener);
        }
    }
}
