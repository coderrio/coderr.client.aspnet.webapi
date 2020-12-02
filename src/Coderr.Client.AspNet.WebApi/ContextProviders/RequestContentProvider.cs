using System;
using System.Collections.Generic;
using Coderr.Client.ContextCollections;
using Coderr.Client.Contracts;
using Coderr.Client.Reporters;

namespace Coderr.Client.AspNet.WebApi.ContextProviders
{
    /// <summary>
    ///     WebApi.RequestContent
    /// </summary>
    public class RequestContentProvider : IContextCollectionProvider
    {
        /// <inheritdoc />
        public ContextCollectionDTO Collect(IErrorReporterContext context)
        {
            var ctx = context as WebApiContext;
            if (ctx?.Request?.Content == null)
                return null;


            var properties = new Dictionary<string, string>();
            foreach (var header in ctx.Request.Content.Headers)
            {
                if (header.Value != null)
                    properties.Add(header.Key, header.Value.ToString());
            }

            var type = ctx.Request.Content.Headers?.ContentType?.MediaType;
            if (type == null)
                return new ContextCollectionDTO(Name, properties);

            if (ctx.Request.Content.Headers.ContentLength > 1000000)
                properties.Add("Body_error", "Body is too large to include in error report.");
            else if (type.StartsWith("text") || type.Contains("xml") || type.Contains("json"))
                try
                {
                    var content = ctx.Request.Content.ReadAsStringAsync().Result;
                    properties.Add("Body", content);
                }
                catch (Exception ex)
                {
                    properties.Add("Body_error", ex.Message);
                }

            return new ContextCollectionDTO(Name, properties);
        }

        /// <summary>
        ///     "WebApi.RequestContent"
        /// </summary>
        public string Name => "WebApi.RequestContent";
    }
}