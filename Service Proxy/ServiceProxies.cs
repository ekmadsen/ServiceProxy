using System;
using System.Net.Http;
using JetBrains.Annotations;
using Refit;


namespace ErikTheCoder.ServiceProxy
{
    [UsedImplicitly]
    public static class Proxy
    {
        [UsedImplicitly]
        public static T For<T>(string ServiceUrlRoot, string Jwt, Func<Guid> GetCorrelationId)
        {
            ProxyMessageHandler proxyMessageHandler = new ProxyMessageHandler(GetCorrelationId);
            HttpClient httpClient = new HttpClient(proxyMessageHandler) { BaseAddress = new Uri(ServiceUrlRoot) };
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Jwt}");
            return RestService.For<T>(httpClient);
        }
    }
}
