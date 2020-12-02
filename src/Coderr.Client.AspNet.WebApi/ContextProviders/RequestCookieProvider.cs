using System.Collections.Generic;
using System.Net.Http;
using Coderr.Client.ContextCollections;
using Coderr.Client.Contracts;
using Coderr.Client.Reporters;

namespace Coderr.Client.AspNet.WebApi.ContextProviders
{
    /// <summary>
    /// Collects cookies from the request.
    /// </summary>
    public class RequestCookieProvider : IContextCollectionProvider
    {
        /// <inheritdoc />
        public ContextCollectionDTO Collect(IErrorReporterContext context)
        {
            var ctx = context as WebApiContext;
            if (ctx?.Request == null) return null;

            var d = new Dictionary<string, string>();

            var cookies = ctx.Request.Headers.GetCookies();
            foreach (var cookie in cookies)
            {
                foreach (var state in cookie.Cookies)
                {
                    d[state.Name] = state.Value;
                }
            }

            return new ContextCollectionDTO(Name, d);
        }

        /// <summary>
        /// WebApi.RequestCookies
        /// </summary>
        public string Name { get; } = "WebApi.RequestCookies";
    }
}
