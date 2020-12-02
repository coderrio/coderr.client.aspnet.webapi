using System.Collections.Generic;
using System.Globalization;
using Coderr.Client.ContextCollections;
using Coderr.Client.Contracts;
using Coderr.Client.Reporters;

namespace Coderr.Client.AspNet.WebApi.ContextProviders
{
    /// <summary>
    ///     Shows how the action was bound to the current HTTP request.
    /// </summary>
    public class ActionDescriptorProvider : IContextCollectionProvider
    {
        /// <summary>
        ///     "WebApi.ActionDescriptor"
        /// </summary>
        public string Name => "WebApi.ActionDescriptor";

        /// <inheritdoc />
        public ContextCollectionDTO Collect(IErrorReporterContext context)
        {
            var ctx = context as WebApiContext;
            if (ctx?.ActionDescriptor == null)
                return null;

            var d = new Dictionary<string, string>();
            var name = ctx.ActionDescriptor.ActionName;
            if (name != null)
                d.Add("ActionName", name);

            foreach (var item in ctx.ActionDescriptor.Properties)
                d.Add($"Property[\"{item.Key}\"]", string.Format(CultureInfo.InvariantCulture, "{0}", item.Value));


            var controllerName = ctx.ActionDescriptor?.ControllerDescriptor?.ControllerName;
            if (controllerName != null)
                d.Add("ControllerName", controllerName);

            return new ContextCollectionDTO(Name, d);
        }
    }
}