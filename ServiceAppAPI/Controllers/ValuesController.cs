using ServiceAppAPI.HelperClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ServiceAppAPI.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {

        //
        // GET api/values        
        public HttpResponseMessage Get()
        {
            //DBServerIdentification data = await AuditLogs.getIdentificationInfo(Request.Headers.Authorization.Parameter);
            //return Ok(new string[] { "value1", "value2" });
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            string viewPath = HttpContext.Current.Server.MapPath(@"~/Views/Home/Index.cshtml");
            var template = File.ReadAllText(viewPath);
            response.Content = new StringContent(template);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
            return response;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
