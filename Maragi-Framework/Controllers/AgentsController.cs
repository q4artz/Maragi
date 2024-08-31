using ApiModels.Requests;
using Maragi_Framework.Models.Agent_Implants;
using Maragi_Framework.Services;
using Microsoft.AspNetCore.Mvc;
using System;

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


        [HttpGet("{agentId}")]
        public IActionResult GetAgent(string agentId)
        {
            var agent = _agents.GetAgent(agentId);
            if (agent is null) return NotFound();
            return Ok(agent);
        }

        // For if only called task
        [HttpGet("{agentId}/tasks")]
        public IActionResult GetTaskResults(string agentId)
        {
            var agent = _agents.GetAgent(agentId);
            if (agent is null) return NotFound("Agent Not Found");

            var results = agent.GetTaskResults();

            return Ok(results);
        }

        // For if called specific TaskID
        [HttpGet("{agentId}/tasks/{taskId}")]
        public IActionResult GetTaskResult(string agentId, string taskId)
        {
            var agent = _agents.GetAgent(agentId);
            if (agent is null) return NotFound("Agent Not Found");

            var result = agent.GetTaskResult(taskId);
            if (result is null) return NotFound("Task Not Found");

            return Ok(result);
        }

        [HttpPost("{agentId}")]
        public IActionResult TaskAgent(string agentId, [FromBody] TaskAgentRequest request)
        {
            var agent = _agents.GetAgent(agentId);
            if (agent is null) return NotFound();

            // can use automappers to map TaskAgentRequest to AgentTask
            var task = new AgentTask()
            {
                Id = Guid.NewGuid().ToString(),
                Command = request.Command,
                Arguements = request.Arguements,
                File = request.File
            };

            agent.QueueTask(task);


            // requires another way to get result of the task
            var root = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}";
            var path = $"{root}/tasks/{task.Id}";

            return Created(path, task);
        }
    }
}
