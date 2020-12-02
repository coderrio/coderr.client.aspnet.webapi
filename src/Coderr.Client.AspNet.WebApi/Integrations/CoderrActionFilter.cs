using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Coderr.Client.AspNet.WebApi.Integrations
{
    /// <summary>
    ///     Used to find invalid models.
    /// </summary>
    public class CoderrActionFilter : IActionFilter
    {
        /// <inheritdoc />
        public bool AllowMultiple { get; } = false;

        /// <inheritdoc />
        public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext,
            CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            if (actionContext.ModelState.IsValid)
                return continuation();

            var ex = CoderrWebApiException.Generate(
                $"Invalid model: {actionContext.ActionDescriptor.ControllerDescriptor.ControllerName}.{actionContext.ActionDescriptor.ActionName}");
            var ctx = actionContext.CreateCoderrContext(this, ex);
            Err.Report(ctx);

            return continuation();
        }
    }
}