using Microsoft.AspNetCore.Mvc;

namespace TeamServer.Modules
{
    [Controller]
    // Whenever Controller is called it automatically do "<Protocol-Listener> + Controller"
    public class HttpListenerController : ControllerBase
    {
        public IActionResult HandleImplant()
        {
            return Ok("Listener Works");
        }
    }
}
