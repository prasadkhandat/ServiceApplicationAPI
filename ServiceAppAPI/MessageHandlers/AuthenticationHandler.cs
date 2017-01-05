using ServiceAppAPI.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ServiceAppAPI.MessageHandlers
{
    public class AuthenticationHandler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            DateTime dtProcessStartTime = DateTime.Now;
            string AuthToken = string.Empty;
            string UserEmail = string.Empty;

            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);

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

            return response;
        }
    }
}