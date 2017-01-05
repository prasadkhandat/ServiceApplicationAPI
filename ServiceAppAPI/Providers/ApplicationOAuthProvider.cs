using LicenseLibrary;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
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
            LoginDetails ld = lc.authenticate(context.UserName, context.Password);

            if (ld.isAuthenticated)
            {
                try
                {
                    await AuditLogs.insertCustomerAuthCall(context.UserName, context.Request.Host.Value, ld.Email, ld.Phone, ld.AuthToken, context.Request.RemoteIpAddress);
                }
                catch (Exception ex)
                { }

                
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("sub", context.UserName));
                identity.AddClaim(new Claim("role", "user"));
                
                Dictionary<string, string> properties = ld.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(ld, null).ToString());

                var props = new AuthenticationProperties(properties);

                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);
            }
            else
                context.Rejected();


        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}