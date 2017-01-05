using ServiceAppAPI.MessageHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace ServiceAppAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();


            config.MessageHandlers.Add(new AuditAPI());

            var jsonFormatter = config.Formatters.JsonFormatter;
            config.Formatters.Remove(jsonFormatter);
            config.Formatters.Insert(0, jsonFormatter);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "PublicAPI",
                routeTemplate: "{controller}/{id}",
                defaults: new { controller = "Home", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
               name: "PrivateDefaultApi",
               routeTemplate: "private/{controller}/{id}",
               defaults: new { id = RouteParameter.Optional },
               constraints: null,
               handler: new AuthenticationHandler() { InnerHandler = new HttpControllerDispatcher(config) }
           );

        }
    }
}
