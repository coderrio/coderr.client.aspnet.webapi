using System.Collections.Generic;
using Coderr.Client.ContextCollections;
using Coderr.Client.Contracts;
using Coderr.Client.Reporters;

namespace Coderr.Client.AspNet.WebApi.ContextProviders
{
    /// <summary>
    ///     Generates a context collection named "WebApi.ModelState"
    /// </summary>
    public class ModelStateProvider : IContextCollectionProvider
    {
        /// <summary>
        ///     To be used internally
        /// </summary>
        internal const string NAME = "WebApi.ModelState";

        /// <summary>
        ///     "WebApi.ModelState"
        /// </summary>
        public string Name => NAME;

        /// <inheritdoc />
        public ContextCollectionDTO Collect(IErrorReporterContext context)
        {
            var ctx = context as WebApiContext;
            if (ctx?.ModelState == null || ctx.ModelState.IsValid || ctx.ModelState.Count == 0)
                return null;

            var dict = new Dictionary<string, string>();
            foreach (var item in ctx.ModelState)
            {
                if (item.Value == null)
                {
                    dict[$"{item.Key}"] = "null";
                    continue;
                }

                if (item.Value?.Value?.RawValue != null)
                    dict[$"{item.Key}.RawValue"] = item.Value.Value.RawValue.ToString();
                if (item.Value?.Value?.AttemptedValue != null)
                    dict[$"{item.Key}.AttemptedValue"] = item.Value.Value.AttemptedValue;
                if (item.Value?.Value?.Culture != null)
                    dict[$"{item.Key}.Culture"] = item.Value.Value.Culture.ToString();

                foreach (var error in item.Value.Errors)
                    dict[$"{item.Key}.Error"] = error.ErrorMessage;
            }

            return new ContextCollectionDTO(Name, dict);
        }
    }
}