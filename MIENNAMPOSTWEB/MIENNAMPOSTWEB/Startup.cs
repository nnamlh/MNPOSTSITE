using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using MIENNAMPOSTWEB.Models;
using MIENNAMPOSTWEB.Providers;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(MIENNAMPOSTWEB.Startup))]
namespace MIENNAMPOSTWEB
{
    public partial class Startup
    {
        static Startup()
        {
            PublicClientId = "self";

            UserManagerFactory = () => new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/MNToken"),
                Provider = new ApplicationOAuthProvider(PublicClientId, UserManagerFactory),
                // AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(5),
                AllowInsecureHttp = true
            };
        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static Func<UserManager<ApplicationUser>> UserManagerFactory { get; set; }

        public static string PublicClientId { get; private set; }
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
