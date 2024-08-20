namespace TeamServer.Modules
{
    public abstract class Listener
    {
        public abstract string Name { get; }

        // Starting Listener
        public abstract Task Start();

        // Stopping Listener
        public abstract void Stop();
    }
}
