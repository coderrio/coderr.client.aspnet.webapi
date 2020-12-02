using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Coderr.Client.ContextCollections;

namespace Coderr.Client.AspNet.WebApi.Integrations
{
    /// <summary>
    ///     Used to measure the performance of HTTP requests.
    /// </summary>
    internal class CoderrMessageHandler : DelegatingHandler
    {
        /// <inheritdoc />
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var requestContext = request.GetRequestContext();
            var sw = Stopwatch.StartNew();
            var response = await base.SendAsync(request, cancellationToken);
            sw.Stop();


            if (response.StatusCode == HttpStatusCode.Forbidden && ConfigExtensions.Report403Error)
            {
                var ex2 = new CoderrWebApiException("403 " + request.RequestUri);
                ReportError(request, response, ex2, "authentication");
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized && ConfigExtensions.Report401Error)
            {
                var ex2 = new CoderrWebApiException("401 " + request.RequestUri);
                ReportError(request, response, ex2, "authentication");
            }


            if (ConfigExtensions.PerformanceFilter == null)
                return response;


            var context = new PerformanceContext(request, response, sw.Elapsed);
            ConfigExtensions.PerformanceFilter(context);
            if (context.ReportTheRequest == false)
                return response;

            var action = request.GetActionDescriptor();
            var ex = action != null
                ? CoderrWebApiException.Generate(
                    $"Slow Request '{action.ControllerDescriptor.ControllerName}.{action.ActionName}' took {sw.ElapsedMilliseconds}ms")
                : CoderrWebApiException.Generate(
                    $"Slow Request '{request.RequestUri.PathAndQuery}' took {sw.ElapsedMilliseconds}ms");

            ReportError(request, response, ex, "performance");

            return response;
        }

        private void ReportError(HttpRequestMessage request, HttpResponseMessage response, Exception exception, string tags = null)
        {
            var ctx = new WebApiContext(this, request, response, exception)
            {
                Request = request,
                Response = response,
                LogEntries = CoderrTracer.Instance.GetEntries(),
                ActionDescriptor = request.GetActionDescriptor(),
                RouteData = request.GetRouteData(),
                CorrelationId = request.GetCorrelationId(),
                User = request.GetRequestContext().Principal,
                Query = request.GetQueryNameValuePairs().ToList()
            };

            if (tags != null)
            {
                var collection = ctx.GetCoderrCollection();
                collection.Properties["ErrTags"] = tags;
            }

            Err.Report(ctx);
        }
    }
}