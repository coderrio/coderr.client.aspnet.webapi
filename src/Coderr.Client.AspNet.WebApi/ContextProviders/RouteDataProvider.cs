using System.Collections.Generic;
using Coderr.Client.ContextCollections;
using Coderr.Client.Contracts;
using Coderr.Client.Reporters;

namespace Coderr.Client.AspNet.WebApi.ContextProviders
{
    /// <summary>
    ///     Creates a collection named "WebApi.RouteData";
    /// </summary>
    public class RouteDataProvider : IContextCollectionProvider
    {
        /// <summary>
        ///     "WebApi.RouteData"
        /// </summary>
        public string Name => "WebApi.RouteData";

        /// <inheritdoc />
        public ContextCollectionDTO Collect(IErrorReporterContext context)
        {
            var mvcContext = context as WebApiContext;
            if (mvcContext?.RouteData == null)
                return null;

            var dict = new Dictionary<string, string>();
            if (mvcContext.RouteData.Route.DataTokens != null)
            {
                foreach (var token in mvcContext.RouteData.Route.DataTokens)
                    dict.Add($"DataToken[\"{token.Key}\"]", token.Value?.ToString() ?? "null");
            }

            foreach (var token in mvcContext.RouteData.Values)
                dict.Add($"Values[\"{token.Key}\"]", token.Value?.ToString() ?? "null");

            return new ContextCollectionDTO(Name, dict);
        }
    }
}