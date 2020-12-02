using System;
using System.Net.Http;

namespace Coderr.Client.AspNet.WebApi
{
    internal static class RequestExtensions
    {
        private const string IsReportedName = "Err_IsReported";

        /// <summary>
        ///     Check if the error has been reported for the current request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="exception">Exception to check</param>
        /// <returns></returns>
        public static bool IsReported(this HttpRequestMessage request, Exception exception)
        {
            if (request?.Properties == null)
                return false;

            return request.Properties.TryGetValue(IsReportedName, out var value) && value == exception;
        }

        /// <summary>
        ///     Mark error as been reported.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="exception"></param>
        public static void SetIsReported(this HttpRequestMessage request, Exception exception)
        {
            if (request?.Properties != null)
            {
                request.Properties[IsReportedName] = exception;
            }
        }
    }
}