using System;
using System.Web.Http;
using System.Web.Http.Tracing;
using Coderr.Client.AspNet.WebApi;
using Coderr.Client.AspNet.WebApi.ContextProviders;
using Coderr.Client.AspNet.WebApi.Integrations;
using Coderr.Client.Config;

// Keeps in the root namespace to get IntelliSense

// ReSharper disable once CheckNamespace
namespace Coderr.Client
{
    /// <summary>
    ///     Extensions for the <see cref="Err" /> configuration class.
    /// </summary>
    public static class ConfigExtensions
    {
        internal static Action<PerformanceContext> PerformanceFilter { get; set; }

        internal static bool Report401Error { get; set; }

        internal static bool Report403Error { get; set; }

        /// <summary>
        ///     Activate the ASP.NET error catching library
        /// </summary>
        /// <param name="configurator">instance.</param>
        /// <param name="webApiConfiguration"></param>
        public static void CatchWebApiExceptions(this CoderrConfiguration configurator,
            HttpConfiguration webApiConfiguration)
        {
            webApiConfiguration.Filters.Add(new CoderrErrorFilter());
            webApiConfiguration.MessageHandlers.Add(new CoderrMessageHandler());
            webApiConfiguration.Services.Replace(typeof(ITraceWriter), CoderrTracer.Instance);

            configurator.ContextProviders.Add(new ActionDescriptorProvider());
            configurator.ContextProviders.Add(new ControllerProvider());
            configurator.ContextProviders.Add(new ModelStateProvider());
            configurator.ContextProviders.Add(new QueryStringProvider());
            configurator.ContextProviders.Add(new RequestCookieProvider());
            configurator.ContextProviders.Add(new RequestProvider());
            configurator.ContextProviders.Add(new RequestContentProvider());
            configurator.ContextProviders.Add(new RequestPropertyProvider());
            configurator.ContextProviders.Add(new RouteDataProvider());
        }

        /// <summary>
        ///     Configure how Coderr collects trace logs from WebApi (to be able to include latest log entries to error reports).
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="options"></param>
        public static void ConfigureLogging(this CoderrConfiguration configuration, Action<CoderrTracer> options)
        {
            options(CoderrTracer.Instance);
        }

        /// <summary>
        ///     Report all authentication failures.
        /// </summary>
        /// <param name="configuration"></param>
        public static void Report401(this CoderrConfiguration configuration)
        {
            Report401Error = true;
        }


        /// <summary>
        ///     Report all requests to forbidden resources.
        /// </summary>
        /// <param name="configuration"></param>
        public static void Report403(this CoderrConfiguration configuration)
        {
            Report403Error = true;
        }

        /// <summary>
        ///     Track HTTP performance.
        /// </summary>
        /// <param name="instance">Coderr config</param>
        /// <param name="filter">Should return <c>true</c> if the processing is OK; <c>false</c> if the request should be reported.</param>
        /// <remarks>
        ///     <para>
        ///         Invoke <see cref="PerformanceContext.ReportRequest" /> to send too slow requests to Coderr.
        ///     </para>
        /// </remarks>
        public static void TrackPerformance(this CoderrConfiguration instance, Action<PerformanceContext> filter)
        {
            PerformanceFilter = filter ?? throw new ArgumentNullException(nameof(filter));
        }
    }
}