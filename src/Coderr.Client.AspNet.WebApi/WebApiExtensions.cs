using System;
using System.Net.Http;
using System.Web.Http;

namespace Coderr.Client.AspNet.WebApi
{
    /// <summary>
    ///     To allow reporting through the error pipeline
    /// </summary>
    public static class WebApiExtensions
    {
        /// <summary>
        ///     Report an exception (allows Coderr to pass it through the WebApi pipeline)
        /// </summary>
        /// <param name="controller">Controller that processed the request.</param>
        /// <param name="exception">What went wrong</param>
        /// <param name="contextData">Additional information</param>
        public static void ReportToCoderr(this ApiController controller, Exception exception, object contextData = null)
        {
            var context = controller.ActionContext.CreateCoderrContext(controller, exception);
            if (contextData != null)
                Err.Report(context, contextData);
            else
                Err.Report(context);
        }

        /// <summary>
        ///     Report an exception (allows Coderr to pass it through the WebApi pipeline)
        /// </summary>
        /// <param name="request">Request that couldn't be processed.</param>
        /// <param name="exception">What went wrong</param>
        /// <param name="contextData">Additional information</param>
        /// <param name="response">Response, if any.</param>
        public static void ReportToCoderr(this HttpRequestMessage request, Exception exception, object contextData = null,
            HttpResponseMessage response = null)
        {
            var context = request.CreateCoderrContext(response, request, exception);
            if (contextData != null)
                Err.Report(context, contextData);
            else
                Err.Report(context);
        }
    }
}