using Coderr.Client.ContextCollections;
using Coderr.Client.Contracts;
using Coderr.Client.Reporters;

namespace Coderr.Client.AspNet.WebApi.ContextProviders
{
    /// <summary>
    ///     Collection for the currently logged in user (<c>IPrincipal</c>).
    /// </summary>
    public class UserPrincipalProvider : IContextCollectionProvider
    {
        /// <inheritdoc />
        public ContextCollectionDTO Collect(IErrorReporterContext context)
        {
            var ctx = context as WebApiContext;
            if (ctx?.User == null)
                return null;

            var collection = CollectionBuilder.CreateForCredentials(ctx.User);
            collection.Name = Name;
            return collection;
        }

        /// <summary>
        ///     "UserPrincipal"
        /// </summary>
        public string Name { get; } = "UserPrincipal";
    }
}