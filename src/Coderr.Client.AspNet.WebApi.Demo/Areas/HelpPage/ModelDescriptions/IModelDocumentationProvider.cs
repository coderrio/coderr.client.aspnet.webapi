using System;
using System.Reflection;

namespace Coderr.Client.AspNet.WebApi.Demo.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}