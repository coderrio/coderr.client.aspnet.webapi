using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.Routing;
using Coderr.Client.Reporters;

namespace Coderr.Client.AspNet.WebApi
{
    /// <summary>
    ///     WebApi context (provides information regarding different aspects in WebApi).
    /// </summary>
    public class WebApiContext : ErrorReporterContext
    {
        /// <summary>
        ///     Creates a new instance of the <see cref="WebApiContext" /> class.
        /// </summary>
        /// <param name="source">Object that detected the exception.</param>
        /// <param name="request">HTTP request.</param>
        /// <param name="response">HTTP response.</param>
        /// <param name="exception">Exception itself</param>
        public WebApiContext(object source, HttpRequestMessage request, HttpResponseMessage response,
            Exception exception)
            : base(source, exception)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
            Response = response;
        }

        /// <summary>
        ///     Arguments defined for a controller action.
        /// </summary>
        public Dictionary<string, object> ActionArguments { get; set; }

        /// <summary>
        ///     Information about the invoked action.
        /// </summary>
        public HttpActionDescriptor ActionDescriptor { get; set; }

        /// <summary>
        ///     Information regarding execution of the controller.
        /// </summary>
        public HttpControllerContext ControllerContext { get; set; }

        /// <summary>
        ///     Correlation ID to be able to reference related errors.
        /// </summary>
        public Guid CorrelationId { get; set; }

        /// <summary>
        ///     ModelState
        /// </summary>
        public ModelStateDictionary ModelState { get; set; }

        /// <summary>
        ///     Query string parameters.
        /// </summary>
        public IReadOnlyList<KeyValuePair<string, string>> Query { get; set; }

        /// <summary>
        ///     HTTP request
        /// </summary>
        public HttpRequestMessage Request { get; set; }

        /// <summary>
        ///     HTTP response (might be null)
        /// </summary>
        public HttpResponseMessage Response { get; set; }

        /// <summary>
        ///     Current route.
        /// </summary>
        public IHttpRouteData RouteData { get; set; }

        /// <summary>
        ///     User if authenticated.
        /// </summary>
        public IPrincipal User { get; set; }
    }
}