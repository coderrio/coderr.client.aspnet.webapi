using System.Collections.Generic;
using Coderr.Client.ContextCollections;
using Coderr.Client.Contracts;
using Coderr.Client.Reporters;

namespace Coderr.Client.AspNet.WebApi.ContextProviders
{
    /// <summary>
    ///     WebApi.Controller
    /// </summary>
    public class ControllerProvider : IContextCollectionProvider
    {
        /// <inheritdoc />
        public ContextCollectionDTO Collect(IErrorReporterContext context)
        {
            var ctx = context as WebApiContext;
            var controllerInfo = ctx?.ControllerContext;
            if (controllerInfo == null)
                return null;

            var properties = new Dictionary<string, string>
            {
                {"InstanceType", controllerInfo.Controller.GetType().FullName},
                {"ControllerDescriptor.Type", controllerInfo.ControllerDescriptor.ControllerType.ToString()},
                {"ControllerDescriptor.Name", controllerInfo.ControllerDescriptor.ControllerName}
            };

            if (controllerInfo.ControllerDescriptor.Properties != null)
                foreach (var item in controllerInfo.ControllerDescriptor.Properties)
                    properties.Add($"Property[\"{item.Key}\"]", item.Value.ToString());

            return new ContextCollectionDTO(Name, properties);
        }

        /// <summary>
        ///     "WebApi.Controller"
        /// </summary>
        public string Name => "WebApi.Controller";
    }
}