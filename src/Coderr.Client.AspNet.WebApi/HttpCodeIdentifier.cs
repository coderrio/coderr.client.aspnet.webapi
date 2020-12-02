using System;
using System.Net;
using System.Security;
using System.Web;

namespace Coderr.Client.AspNet.WebApi
{
    /// <summary>
    ///     Tries to Identity the correct HTTP code
    /// </summary>
    public class HttpCodeIdentifier
    {
        /// <summary>
        ///     Creates a new instance of <see cref="HttpCodeIdentifier" />
        /// </summary>
        /// <param name="application">application running</param>
        /// <param name="exception">thrown exception</param>
        public HttpCodeIdentifier(HttpApplication application, Exception exception)
        {
            HttpCode = 500;
            HttpCodeName = "InternalServerError";

            if (application != null)
            {
                HttpCode = application.Response.StatusCode;
                HttpCodeName = ((HttpStatusCode)application.Response.StatusCode).ToString().Replace(" ", "");
            }

            if (exception != null)
                IdentifyFromException(exception);
        }

        /// <summary>
        ///     Http code
        /// </summary>
        public int HttpCode { get; set; }

        /// <summary>
        ///     Code name (typically HttpStatusCode.ToString())
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Used to know which error page to display. i.e. the name must be PascalCase like "InternalServerError" instead
        ///         of "Internal server error".
        ///     </para>
        /// </remarks>
        public string HttpCodeName { get; set; }

        private void IdentifyFromException(Exception exception)
        {
            if (exception.GetType() == typeof(HttpException))
            {
                HttpCode = ((HttpException)exception).GetHttpCode();
                if (Enum.IsDefined(typeof(HttpStatusCode), HttpCode))
                    HttpCodeName = ((HttpStatusCode)HttpCode).ToString();
            }
            else if (exception.GetType() == typeof(WebException))
            {
                var x = (WebException)exception;
                try
                {
                    if (x.Response is HttpWebResponse response)
                    {
                        HttpCode = (int)response.StatusCode;
                        HttpCodeName = response.StatusCode.ToString();
                    }
                }
                //yes, IT EAT!
                catch
                {
                }
            }
            else if (exception is SecurityException)
            {
                HttpCode = 403;
                HttpCodeName = "Forbidden";
            }
            else if (exception is UnauthorizedAccessException)
            {
                HttpCode = 401;
                HttpCodeName = "Unauthorized";
            }
            else if (exception.GetType().Name == "ObjectNotFoundException")
            {
                HttpCode = 404;
                HttpCodeName = "NotFound";
            }
        }
    }
}