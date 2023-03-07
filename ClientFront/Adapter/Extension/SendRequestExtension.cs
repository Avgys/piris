using RestSharp;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Adapter.Extension
{
    /// <summary>
    /// Class uses restSharp client for sending requests to analytic receiver.
    /// Content is session info in json format that injected in request's body.
    /// There 2 pairs of methods: sendStatistic and closeSession.
    /// The difference is how installationId injected in request, in cookies or content or queryString.
    /// </summary>
    public class RequestService
    {
        public readonly RestClient _restClient;

        public RequestService()
        {
            _restClient = new RestClient(AppSettingsProvider.HostName);
        }
        
        public async Task<RestResponse> Get(string url, CancellationToken cancellationToken)
        {
            var adapterRequest = new RestRequest(url, Method.Get)
                .AddHeader("content-type", "application/json");
        
            return await _restClient.ExecuteAsync(adapterRequest, cancellationToken);
        }

        public async Task<RestResponse> Post(string url, string body, CancellationToken cancellationToken)
        {
            var adapterRequest = new RestRequest(url, Method.Post)
                .AddHeader("content-type", "application/json")
                .AddStringBody(body, DataFormat.Json);

            return await _restClient.ExecuteAsync(adapterRequest, cancellationToken);
        }

        public async Task<RestResponse> Post(string url, CancellationToken cancellationToken)
        {
            var adapterRequest = new RestRequest(url, Method.Post);

            return await _restClient.ExecuteAsync(adapterRequest, cancellationToken);
        }

        public async Task<RestResponse> Put(string url, string body, CancellationToken cancellationToken)
        {
            var adapterRequest = new RestRequest(url, Method.Put)
                .AddHeader("content-type", "application/json")
                .AddStringBody(body, DataFormat.Json);
            
            return await _restClient.ExecuteAsync(adapterRequest, cancellationToken);
        }
        
        // public async Task<RestResponse> Send(string url, string content, CancellationToken cancellationTokenm, Method method)
        // {
        //     var adapterRequest = new RestRequest(url, method)
        //         .AddHeader("content-type", "application/json")
        //         .AddStringBody(content, DataFormat.Json);
        //
        //     var receiverResponse = await _restClient.ExecuteAsync(adapterRequest, cancellationToken);
        //
        //     return receiverResponse;
        // }

        // public async Task<RestResponse> SendStatisticAsync(string content, CancellationToken cancellationToken)
        // {
        //     Console.WriteLine("api/statistics");
        //     var adapterRequest = new RestRequest("api/statistics", Method.Post)
        //         .AddHeader("content-type", "application/json")
        //         .AddStringBody(content, DataFormat.Json);
        //
        //     var receiverResponse = await _restClient.ExecuteAsync(adapterRequest, cancellationToken);
        //
        //     return receiverResponse;
        // }
        //
        // public async Task<RestResponse> SendStatisticAsync(Guid cookieValue, string content, CancellationToken cancellationToken)
        // {
        //     var adapterRequest = new RestRequest("api/statistics", Method.Post)
        //         .AddHeader("content-type", "application/json")
        //         .AddStringBody(content, DataFormat.Json);
        //
        //     var cookie = new Cookie(AppSettingsProvider.AnalyticCookieUniqueidentifierName, cookieValue.ToString(), "/", "localhost");
        //     _restClient.CookieContainer.Add(cookie);
        //
        //     var receiverResponse = await _restClient.ExecuteAsync(adapterRequest, cancellationToken);
        //
        //     return receiverResponse;
        // }
        //
        // public async Task<RestResponse> SendCloseSessionByQueryStringAsync(Guid guid, CancellationToken cancellationToken)
        // {
        //     var adapterRequest = new RestRequest("api/statistics/closeLatestSession", Method.Put)
        //         .AddParameter("installationId", guid, ParameterType.QueryString);
        //
        //     var cookie = new Cookie(AppSettingsProvider.AnalyticCookieUniqueidentifierName, guid.ToString(), "/", "localhost");
        //     _restClient.CookieContainer.Add(cookie);
        //     var receiverResponse = await _restClient.ExecuteAsync(adapterRequest, cancellationToken);
        //
        //     return receiverResponse;
        // }
        //
        // public async Task<RestResponse> SendCloseSessionByCookieAsync(Guid? guid, CancellationToken cancellationToken)
        // {
        //     var adapterRequest = new RestRequest("api/statistics/closeLatestSession", Method.Put);
        //     var cookie = new Cookie(AppSettingsProvider.AnalyticCookieUniqueidentifierName, guid.ToString(), "/", "localhost");
        //
        //     _restClient.CookieContainer.Add(cookie);
        //
        //     var receiverResponse = await _restClient.ExecuteAsync(adapterRequest, cancellationToken);
        //
        //     return receiverResponse;
        // }
    }
}
