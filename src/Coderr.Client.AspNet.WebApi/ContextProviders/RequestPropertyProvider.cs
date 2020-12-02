using System.Collections.Generic;
using Coderr.Client.ContextCollections;
using Coderr.Client.Contracts;
using Coderr.Client.Reporters;

namespace Coderr.Client.AspNet.WebApi.ContextProviders
{
    /// <summary>
    ///     "WebApi.RequestProperties"
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Will not include properties which starts with "MS_"
    ///     </para>
    /// </remarks>
    public class RequestPropertyProvider : IContextCollectionProvider
    {
        /// <inheritdoc />
        public ContextCollectionDTO Collect(IErrorReporterContext context)
        {
            var ctx = context as WebApiContext;
            var request = ctx?.Request;
            if (request?.Headers == null)
                return null;

            var properties = new Dictionary<string, string>();
            foreach (var kvp in request.Properties)
            {
                if (kvp.Key.StartsWith("MS_"))
                    continue;
                if (kvp.Key.StartsWith("Err_"))
                    continue;

                properties.Add(kvp.Key, kvp.Value?.ToString());
            }

            if (properties.Count == 0)
                return null;

            return new ContextCollectionDTO(Name, properties);
        }

        /// <summary>
        ///     "WebApi.RequestProperties"
        /// </summary>
        public string Name => "WebApi.RequestProperties";
    }
}