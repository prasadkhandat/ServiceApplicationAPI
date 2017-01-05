using ServiceAppAPI.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;

namespace ServiceAppAPI.MessageHandlers
{
    public class AuditAPI : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            DateTime dtProcessStartTime = DateTime.Now;
            string AuthToken = string.Empty;
            string UserEmail = string.Empty;
            string ReqData = string.Empty;
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            try
            {
                ReqData = await request.Content.ReadAsStringAsync();
            }
            catch { }           
            /*
            if (request.RequestUri.LocalPath.ToString().ToLower().Contains("private/"))
            {
                bool Authenticated = false;
                try
                {
                    AuthToken = request.Headers.GetValues("authtoken").First<string>();
                    UserEmail = request.Headers.GetValues("uname").First<string>();
                    DBServerIdentification objData = new DBServerIdentification();
                    objData = await AuditLogs.getIdentificationInfo(UserEmail, AuthToken);
                    if (objData.Status == "Success")
                    {
                        Authenticated = true;
                        request.Headers.Add("MOBJID", objData.ObjectID);
                    }
                    else
                    {
                        response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    }

                }
                catch (Exception ex)
                {
                    response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }

                if (Authenticated)
                {
                    response = await base.SendAsync(request, cancellationToken);
                }
            }
            else
            */
            {
                response = await base.SendAsync(request, cancellationToken);
            }
            if (!(request.RequestUri.LocalPath.ToString().Equals(@"/") || request.RequestUri.LocalPath.ToString().Length == 0))
            {
                DateTime dtProcessEndTime = DateTime.Now;
                long? bodylength = 0;
                long? headerlength = 0;
                if (response.Content != null)
                {
                    await response.Content.LoadIntoBufferAsync();
                    bodylength = response.Content.Headers.ContentLength;
                    headerlength = response.Headers.ToString().Length;
                }
                try
                {
                    string resContaint = string.Empty;
                    try
                    {
                        resContaint = await response.Content.ReadAsStringAsync();
                    }
                    catch { }
                    await AuditLogs.insertRequestCall(AuthToken == null ? "" : AuthToken, request.Headers.Host == null ? "" : request.Headers.Host, ReqData == null ? "" : ReqData, resContaint, response.StatusCode.ToString(), bodylength == null ? "" : bodylength.ToString(), response.Headers == null ? "" : response.Headers.ToString(), dtProcessStartTime.ToString("yyyy-MM-dd HH:mm:ss"), dtProcessEndTime.ToString("yyyy-MM-dd HH:mm:ss"), response.StatusCode.GetHashCode());
                }
                catch { }
            }
            return response;
        }
    }
}