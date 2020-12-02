using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Coderr.Client.AspNet.WebApi.Integrations
{
    /// <summary>
    ///     Reports all unhandled exceptions to Coderr.
    /// </summary>
    public class CoderrErrorFilter : IExceptionFilter
    {
        /// <inheritdoc />
        public bool AllowMultiple { get; } = false;

        /// <inheritdoc />
        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext,
            CancellationToken cancellationToken)
        {
            if (actionExecutedContext.Request.IsReported(actionExecutedContext.Exception))
            {
#if NET451
                return Task.FromResult<object>(null);
#else
                return Task.CompletedTask;
#endif
            }

            actionExecutedContext.Request.SetIsReported(actionExecutedContext.Exception);

            var ctx = actionExecutedContext.ActionContext.CreateCoderrContext(this, actionExecutedContext.Exception);
            Err.Report(ctx);

#if NET451
            return Task.FromResult<object>(null);
#else
            return Task.CompletedTask;
#endif        
        }
    }
}