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
        private const string _authHeader = "Authorization";
        private readonly Func<string> _getAuthToken;
        private readonly Func<Guid> _getCorrelationId;


        public ProxyMessageHandler(Func<string> GetAuthToken, Func<Guid> GetCorrelationId)
        {
            _getAuthToken = GetAuthToken;
            _getCorrelationId = GetCorrelationId;
            InnerHandler = new HttpClientHandler();
        }


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage Request, CancellationToken CancellationToken)
        {
            string authToken = _getAuthToken();
            if (!string.IsNullOrEmpty(authToken) && !Request.Headers.Contains(_authHeader)) Request.Headers.Add(_authHeader, _getAuthToken());
            Guid correlationId = _getCorrelationId();
            if ((correlationId != Guid.Empty) && !Request.Headers.Contains(CustomHttpHeader.CorrelationId)) Request.Headers.Add(CustomHttpHeader.CorrelationId, _getCorrelationId().ToString());
            return await base.SendAsync(Request, CancellationToken);
        }
    }
}
