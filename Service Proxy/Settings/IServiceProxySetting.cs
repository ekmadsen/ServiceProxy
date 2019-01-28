using JetBrains.Annotations;


namespace ErikTheCoder.ServiceProxy.Settings
{
    public interface IServiceProxySetting
    {
        [UsedImplicitly] string Url { get; set; }
        [UsedImplicitly] string Username { get; set; }
        [UsedImplicitly] string Password { get; set; }
        [UsedImplicitly] string Token { get; set; }
    }
}
