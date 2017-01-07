using LicenseLibrary;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using ServiceAppAPI.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ServiceAppAPI.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        //LoginDetails details = null;
        LoginDetails ld = null;
        string AuthenticationType = string.Empty;
        public ApplicationOAuthProvider() { }
        public ApplicationOAuthProvider(string AuthType) { AuthenticationType = AuthType; }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            LicenseClass lc = new LicenseLibrary.LicenseClass();
            ld = lc.authenticate(context.UserName, context.Password);

            if (ld.isAuthenticated)
            {
                ClaimsIdentity oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);

                oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                oAuthIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "user"));

                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties());

                DateTime currentUtc = DateTime.UtcNow;
                ticket.Properties.IssuedUtc = currentUtc;
                ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromDays(1));

                string accessToken = context.Options.AccessTokenFormat.Protect(ticket);
                //context.Request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);



                // Create the response building a JSON object that mimics exactly the one issued by the default /Token endpoint
                JObject token = new JObject(
                    //new JProperty("userName", context.UserName),                    
                    new JProperty("access_token", accessToken),
                    new JProperty("token_type", "bearer")//,
                    //new JProperty("expires_in", TimeSpan.FromDays(1).TotalSeconds.ToString()),
                    //new JProperty("issued", currentUtc.ToString("ddd, dd MMM yyyy HH':'mm':'ss 'GMT'")),
                    //new JProperty("expires", currentUtc.Add(TimeSpan.FromDays(1)).ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'"))
                );



                try
                {
                    await AuditLogs.insertCustomerAuthCall(context.UserName, context.Request.Host.Value, ld.Email, ld.Phone, accessToken, context.Request.RemoteIpAddress,ld.UID,ld.Role);
                }
                catch (Exception ex)
                { }

                
                //var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                //identity.AddClaim(new Claim("sub", context.UserName));
                //identity.AddClaim(new Claim("role", "user"));
                
                //Dictionary<string, string> properties = ld.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(ld, null).ToString());

                var props = new AuthenticationProperties(token.ToObject<Dictionary<string, string>>());

                var ticket1 = new AuthenticationTicket(oAuthIdentity, props);
                context.Validated(ticket1);
            }
            else
                context.Rejected();


        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {

            context.AdditionalResponseParameters.Add("access_token", context.Properties.Dictionary["access_token"]);
            context.AdditionalResponseParameters.Add("email", ld.Email);
            context.AdditionalResponseParameters.Add("first_name", ld.FirstName);
            context.AdditionalResponseParameters.Add("last_name", ld.LastName);
            context.AdditionalResponseParameters.Add("is_authenticated", ld.isAuthenticated);
            context.AdditionalResponseParameters.Add("phone", ld.Phone);
            context.AdditionalResponseParameters.Add("user_role", ld.Role);
            
            return Task.FromResult<object>(null);

        }
    }
}