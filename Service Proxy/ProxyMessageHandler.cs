using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ErikTheCoder.ServiceContract;


namespace ErikTheCoder.ServiceProxy
{
    public class ProxyMessageHandler : DelegatingHandler
    {
        private readonly Func<Guid> _getCorrelationId;


        public ProxyMessageHandler(Func<Guid> GetCorrelationId)
        {
            _getCorrelationId = GetCorrelationId;
            InnerHandler = new HttpClientHandler();
        }


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage Request, CancellationToken CancellationToken)
        {
            if (!Request.Headers.Contains(CustomHttpHeader.CorrelationId))
            {
                Request.Headers.Add(CustomHttpHeader.CorrelationId, _getCorrelationId().ToString());
            }
            return await base.SendAsync(Request, CancellationToken);
        }
    }
}
