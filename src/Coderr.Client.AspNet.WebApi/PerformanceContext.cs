using System;
using System.Net.Http;

namespace Coderr.Client.AspNet.WebApi
{
    /// <summary>
    ///     Context used to specify which types of requests that we should measure.
    /// </summary>
    public class PerformanceContext
    {
        /// <summary>
        ///     Creates a new instance of the <see cref="PerformanceContext" /> class.
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <param name="response">HTTP response</param>
        /// <param name="elapsed">Amount of time that it took to process the request.</param>
        public PerformanceContext(HttpRequestMessage request, HttpResponseMessage response, TimeSpan elapsed)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
            Response = response ?? throw new ArgumentNullException(nameof(response));
            ExecutionTime = elapsed;
        }

        /// <summary>
        ///     Amount of time that it took to process the request.
        /// </summary>
        public TimeSpan ExecutionTime { get; set; }

        /// <summary>
        ///     HTTP request.
        /// </summary>
        public HttpRequestMessage Request { get; set; }

        /// <summary>
        ///     HTTP response.
        /// </summary>
        public HttpResponseMessage Response { get; set; }

        internal bool ReportTheRequest { get; set; }

        /// <summary>
        ///     Tell Coderr to report the request (exceeded the maximum amount of time that request processing may take).
        /// </summary>
        public void ReportRequest()
        {
            ReportTheRequest = true;
        }
    }
}