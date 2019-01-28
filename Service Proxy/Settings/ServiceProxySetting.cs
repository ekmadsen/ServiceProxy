using JetBrains.Annotations;


namespace ErikTheCoder.ServiceProxy.Settings
{
    [UsedImplicitly]
    public class ServiceProxySetting : IServiceProxySetting
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
