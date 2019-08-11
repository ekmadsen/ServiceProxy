using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ErikTheCoder.ServiceContract;
using JetBrains.Annotations;


namespace ErikTheCoder.ServiceProxy
{
    [UsedImplicitly]
    public class ProxyMessageHandler : DelegatingHandler
    {
        private const string _authenticationHeader = "Authorization";
        private readonly Func<string> _getAuthenticationToken;
        private readonly Func<Guid> _getCorrelationId;


        public ProxyMessageHandler(Func<string> GetAuthenticationToken, Func<Guid> GetCorrelationId)
        {
            _getAuthenticationToken = GetAuthenticationToken;
            _getCorrelationId = GetCorrelationId;
            InnerHandler = new HttpClientHandler();
        }


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage Request, CancellationToken CancellationToken)
        {
            if (!Request.Headers.Contains(_authenticationHeader)) Request.Headers.Add(_authenticationHeader, _getAuthenticationToken());
            if (!Request.Headers.Contains(CustomHttpHeader.CorrelationId)) Request.Headers.Add(CustomHttpHeader.CorrelationId, _getCorrelationId().ToString());
            return await base.SendAsync(Request, CancellationToken);
        }
    }
}
