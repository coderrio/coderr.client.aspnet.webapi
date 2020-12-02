using System.Collections.Generic;
using System.Net.Http;
using Coderr.Client.ContextCollections;
using Coderr.Client.Contracts;
using Coderr.Client.Reporters;

namespace Coderr.Client.AspNet.WebApi.ContextProviders
{
    /// <summary>
    ///     Generates a collection called "WebApi.Request"
    /// </summary>
    public class RequestProvider : IContextCollectionProvider
    {
        /// <summary>
        ///     WebApi.Request
        /// </summary>
        public string Name => "WebApi.Request";

        /// <inheritdoc />
        public ContextCollectionDTO Collect(IErrorReporterContext context)
        {
            var ctx = context as WebApiContext;
            if (ctx?.Request == null) return null;

            var d = new Dictionary<string, string>();

            foreach (var header in ctx.Request.Headers)
            {
                if (header.Key == "Cookie")
                    continue;

                d[header.Key] = string.Join(",", header.Value);
            }

            d["RequestUri"] = ctx.Request.RequestUri.ToString();
            d["Method"] = ctx.Request.Method.ToString();
            d["Version"] = ctx.Request.Version.ToString();
            d["RequestUri"] = ctx.Request.RequestUri.ToString();

            return new ContextCollectionDTO(Name, d);
        }
    }
}