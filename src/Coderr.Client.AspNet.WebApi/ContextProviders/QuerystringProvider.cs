using System.Collections.Generic;
using Coderr.Client.ContextCollections;
using Coderr.Client.Contracts;
using Coderr.Client.Reporters;

namespace Coderr.Client.AspNet.WebApi.ContextProviders
{
    /// <summary>
    ///     Adds a HTTP request query string collection.
    /// </summary>
    /// <remarks>The name of the collection is "WebApi.Query"</remarks>
    public class QueryStringProvider : IContextCollectionProvider
    {
        /// <summary>
        ///     Collect information
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Collection</returns>
        public ContextCollectionDTO Collect(IErrorReporterContext context)
        {
            var ctx = context as WebApiContext;
            if (ctx?.Query == null)
                return null;

            var items = new Dictionary<string, string>();
            foreach (var item in ctx.Query)
                items[item.Key] = string.Join(",", item.Value);

            return items.Count == 0 ? null : new ContextCollectionDTO(Name, items);
        }

        /// <summary>
        ///     "WebApi.Query"
        /// </summary>
        public string Name => "WebApi.Query";
    }
}