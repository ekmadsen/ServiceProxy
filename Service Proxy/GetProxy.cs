using System;
using System.Net.Http;
using JetBrains.Annotations;
using Refit;


namespace ErikTheCoder.ServiceProxy
{
    [UsedImplicitly]
    public static class GetProxy
    {
        [UsedImplicitly]
        public static Proxy<T> For<T>(string ServiceUrlRoot) => For<T>(ServiceUrlRoot, () => Guid.Empty);


        [UsedImplicitly]
        public static Proxy<T> For<T>(string ServiceUrlRoot, Func<Guid> GetCorrelationId) => For<T>(ServiceUrlRoot, () => null, () => null, GetCorrelationId);


        [UsedImplicitly]
        public static Proxy<T> For<T>(string ServiceUrlRoot, Func<string> GetAdminAuthToken, Func<string> GetUserAuthToken, Func<Guid> GetCorrelationId)
        {
            // Create admin HTTP client.
            var adminProxyMessageHandler = new ProxyMessageHandler(GetAdminAuthToken, GetCorrelationId);
            var adminHttpClient = new HttpClient(adminProxyMessageHandler) { BaseAddress = new Uri(ServiceUrlRoot) };
            // Create user HTTP client.
            var userProxyMessageHandler = new ProxyMessageHandler(GetUserAuthToken, GetCorrelationId);
            var userHttpClient = new HttpClient(userProxyMessageHandler) { BaseAddress = new Uri(ServiceUrlRoot) };
            return new Proxy<T>(RestService.For<T>(adminHttpClient), RestService.For<T>(userHttpClient));
        }
    }
}
