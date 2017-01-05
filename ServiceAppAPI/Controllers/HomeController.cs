using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace ServiceAppAPI.Controllers
{
    public class HomeController : Controller
    {
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            string viewPath = Server.MapPath(@"~/Views/Home/Index.cshtml");
            var template = System.IO.File.ReadAllText(viewPath);
            response.Content = new StringContent(template);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
            return response;
        }

        public HttpResponseMessage Index()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            string viewPath = Server.MapPath(@"~/Views/Home/Index.cshtml");
            var template = System.IO.File.ReadAllText(viewPath);
            response.Content = new StringContent(template);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
