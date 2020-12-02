using Coderr.Client.ContextCollections;
using Coderr.Client.Contracts;
using Coderr.Client.Reporters;

namespace Coderr.Client.AspNet.WebApi.ContextProviders
{
    /// <summary>
    ///     Collection for the currently logged in user (<c>IPrincipal</c>), but does a Md5 on it so that user cannot be
    ///     identified.
    /// </summary>
    public class UserPrincipalTokenProvider : IContextCollectionProvider
    {
        /// <inheritdoc />
        public ContextCollectionDTO Collect(IErrorReporterContext context)
        {
            var ctx = context as WebApiContext;
            if (ctx?.User == null || !ctx.User.Identity.IsAuthenticated)
                return null;

            return CollectionBuilder.CreateTokenForCredentials(ctx.User.Identity);
        }

        /// <summary>
        ///     "UserPrincipalToken"
        /// </summary>
        public string Name { get; } = "UserCredentials";
    }
}