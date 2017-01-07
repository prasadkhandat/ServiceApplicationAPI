using ServiceAppAPI.MessageHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "PublicAPI",
            //    routeTemplate: "{controller}/{id}",
            //    defaults: new { controller = "Home", id = RouteParameter.Optional }
            //);

            //config.Routes.MapHttpRoute(
            //   name: "MapByAction",
            //   routeTemplate: "{controller}/{action}/{id}", defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "PublicAPI",
                routeTemplate: "{controller}/{id}",
                defaults: new { controller = "Values", id = RouteParameter.Optional }
            );


        }
    }
}
