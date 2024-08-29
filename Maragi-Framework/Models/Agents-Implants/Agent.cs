using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Maragi_Framework.Models.Agent_Implants
{
    public class Agent
    {
            
        // AgentMetadata from AgentMetadata.cs
        public AgentMetadata Metadata { get;}
        public DateTime LastSeen { get; private set; }

        // Queue but thread safe
        // EXP -- Agent checkin and Tasking at the same time won't break things -- using Quaue might break
        private readonly ConcurrentQueue<AgentTask> _pendingTasks = new();

        private readonly List<AgentTaskResult> _taskResults = new();

        // Constructing Agent / Implant
        public Agent(AgentMetadata metadata) 
        {     
            Metadata = metadata;
        }

        public void CheckIn() {
            LastSeen = DateTime.UtcNow;
        }

        public void QueueTask(AgentTask task) 
        {
            _pendingTasks.Enqueue(task);
        }

        // blank IEnumeratble
        public IEnumerable<AgentTask> GetPendingtask()
        {
            List<AgentTask> tasks = new();

            // Keep dequeueing the Queue -- Add tasks to the AgentTask list and return the tasks
            while (_pendingTasks.TryDequeue(out var task))
            {
                tasks.Add(task);
            }
            return tasks;
        }

        public AgentTaskResult GetTaskResult(string taskId) 
        {
            return GetTaskResults().FirstOrDefault(r => r.Id.Equals(taskId));
        }

        public IEnumerable<AgentTaskResult> GetTaskResults()
        {
            return _taskResults;
        }
    }
}
