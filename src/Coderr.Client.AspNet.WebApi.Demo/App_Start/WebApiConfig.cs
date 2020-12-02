using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Tracing;

namespace Coderr.Client.AspNet.WebApi.Demo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var url = new Uri("http://localhost:60473/");
            Err.Configuration.Credentials(url,
                "5a617e0773b94284bef33940e4bc8384",
                "3fab63fb846c4dd289f67b0b3340fefc");

            Err.Configuration.CatchWebApiExceptions(config);

            Err.Configuration.Report401();
            Err.Configuration.Report403();
Err.Configuration.TrackPerformance(x =>
{
    if (x.ExecutionTime.TotalMilliseconds < 1000) return;
    
    if (x.Request.Method == HttpMethod.Get)
    {
        x.ReportRequest();
    }

    if (x.Request.Method == HttpMethod.Post && x.ExecutionTime.TotalMilliseconds > 2000)
    {
        x.ReportRequest();
    }
});

Err.Configuration.ConfigureLogging(options =>
{
    options.MaxAge = TimeSpan.FromMinutes(2);
    options.MinLevel = TraceLevel.Info;
    options.MaxEntries = 50;
});


            Err.Configuration.CatchWebApiExceptions(config);

            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
