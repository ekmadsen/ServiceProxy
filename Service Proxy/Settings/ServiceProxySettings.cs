using System.Collections.Generic;
using JetBrains.Annotations;


namespace ErikTheCoder.ServiceProxy.Settings
{
    [UsedImplicitly]
    public class ServiceProxySettings : Dictionary<string, ServiceProxySetting>, IServiceProxySettings
    {
    }
}
