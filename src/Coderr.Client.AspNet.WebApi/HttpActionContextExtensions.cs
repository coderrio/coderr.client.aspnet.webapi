using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using Coderr.Client.AspNet.WebApi.Integrations;

namespace Coderr.Client.AspNet.WebApi
{
    /// <summary>
    ///     Extensions used to generate Coderr context.
    /// </summary>
    internal static class HttpActionContextExtensions
    {
        public static WebApiContext CreateCoderrContext(this HttpRequestMessage request, HttpResponseMessage response,
            object source, Exception exception)
        {
            var requestContext = request.GetRequestContext();
            return new WebApiContext(source, request, response, exception)
            {
                Request = request,
                Response = response,
                LogEntries = CoderrTracer.Instance.GetEntries(),
                ActionDescriptor = request.GetActionDescriptor(),
                RouteData = request.GetRouteData(),
                CorrelationId = request.GetCorrelationId(),
                User = requestContext.Principal,
                Query = request.GetQueryNameValuePairs().ToList()
            };
        }

        public static WebApiContext CreateCoderrContext(this HttpActionContext actionContext, object source,
            Exception exception)
        {
            return new WebApiContext(source, actionContext.Request, actionContext.Response, exception)
            {
                ActionArguments = actionContext.ActionArguments,
                ActionDescriptor = actionContext.ActionDescriptor,
                CorrelationId = actionContext.Request.GetCorrelationId(),
                ControllerContext = actionContext.ControllerContext,
                LogEntries = CoderrTracer.Instance.GetEntries(),
                ModelState = actionContext.ModelState,
                Query = actionContext.Request.GetQueryNameValuePairs().ToList(),
                Request = actionContext.Request,
                Response = actionContext.Response,
                RouteData = actionContext.RequestContext.RouteData,
                User = actionContext.RequestContext.Principal
            };
        }
    }
}