using Maragi_Framework.Services;
using Microsoft.AspNetCore.Mvc;

namespace Maragi_Framework.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgentsController : ControllerBase
    {
        public readonly IAgentService _agents;

        public AgentsController(IAgentService agents) 
        {
            _agents = agents;
        }

        [HttpGet]
        public IActionResult GetAgents()
        {
            var agents = _agents.GetAgents();
            return Ok(agents);
        }


        [HttpGet("{name}")]
        public IActionResult GetAgent(string name)
        {
            var agent = _agents.GetAgent(name);
            if (agent is null) return NotFound();
            return Ok(agent);
        }
    }
}
