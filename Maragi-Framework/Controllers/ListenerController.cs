using Maragi_Framework.Services;
using Maragi_Framework.Models;
using Microsoft.AspNetCore.Mvc;
using ApiModels.Requests;

namespace Maragi_Framework.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListenersController : ControllerBase
    {
        // write API to interact with Listener Service
        private readonly IListenerService _listeners;

        public ListenersController(IListenerService listeners)
        {
            _listeners = listeners;
        }

        [HttpGet]
        public IActionResult GetListeners()
        {
            var listeners = _listeners.GetListeners();
            return Ok(listeners);
        }

        // will be populated with swagger -- swagger will pick this up
        // if name match it will know GetListener's string "name" is HttpGet "name"
        [HttpGet("{name}")]
        public IActionResult GetListener(string name)
        {
            // in IListenerService we set FirstOrDefault == if listener don't exist it will return null
            var listener = _listeners.GetListener(name);
            if (listener is null) return NotFound();

            return Ok(listener);
        }

        [HttpPost]
        public IActionResult StartListener([FromBody] StartHttpListenerRequest request)
        {
            // Remove Using System.Net; Add using TeamServer.Modules;
            var listener = new HttpListener(request.Name, request.BindPort);
            listener.Start();

            _listeners.AddListener(listener);

            var root = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}";
            var path = $"{root}/{listener.Name}";

            return Created(path, listener);
        }

        [HttpDelete("{name}")]
        public IActionResult StopListener(string name)
        {
            var listener = _listeners.GetListener(name);
            if (listener is null) return NotFound();

            listener.Stop();
            _listeners.RemoveListener(listener);

            return NoContent();
        }
    }
}