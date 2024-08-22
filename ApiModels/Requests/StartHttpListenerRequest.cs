namespace ApiModels.Requests
{
    public class StartHttpListenerRequest
    {
        // creating seperate project so other libraries can reference it -- client can reference this ApiModel
        public string Name { get; set; }
        public int BindPort { get; set; }
    }
}
