using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace Coderr.Client.AspNet.WebApi.Integrations
{
    /// <summary>
    ///     Reports all unhandled exceptions to Coderr.
    /// </summary>
    public class CoderrExceptionHandler : ExceptionHandler
    {
        /// <inheritdoc />
        public override void Handle(ExceptionHandlerContext context)
        {
            if (context.Request.IsReported(context.Exception))
                return;
            context.Request.SetIsReported(context.Exception);


            var coderrContext = context.ExceptionContext.ActionContext.CreateCoderrContext(this, context.Exception);
            Err.Report(coderrContext);

            base.Handle(context);
        }
    }
}