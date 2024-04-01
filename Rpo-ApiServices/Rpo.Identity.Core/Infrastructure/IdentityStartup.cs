using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using Rpo.Identity.Core.Managers;
using Rpo.Identity.Core.Providers;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Rpo.Identity.Core.Infrastructure
{
    public class HeaderFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null)
                actionExecutedContext.Response.Headers.Add("Cache-Control", "no-cache");
        }
    }
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception.GetType().Name.Equals("RpoUnAuthorizedException"))
            {
                context.Response = context.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, new { Error = context.Exception.Message });
            }

            if (context.Exception.GetType().Name.Equals("RpoBusinessException"))
            {
                context.Response = context.Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, new { Error = context.Exception.Message });
            }

            if (context.Exception is DbEntityValidationException)
            {
                DbEntityValidationException e = (DbEntityValidationException)context.Exception;

                context.Response = context.Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, new { Error = string.Join("<br>", e.EntityValidationErrors.SelectMany(eve => eve.ValidationErrors.Select(ve => ve.ErrorMessage))) });
            }
            else if (context.Exception is System.Data.Entity.Infrastructure.DbUpdateException)
            {
                var uq_rgx = new Regex(@"Cannot insert duplicate key row in object 'dbo\.(?<table>\w+)' with unique index '(?<index>\w+)'. The duplicate key value is ", RegexOptions.IgnoreCase);
                var fk_rgx = new Regex(@"The DELETE statement conflicted with the REFERENCE constraint ", RegexOptions.IgnoreCase);
                //The DELETE statement conflicted with the REFERENCE constraint "FK_dbo.Employees_dbo.Groups_IdGroup". The conflict occurred in database "RPO", table "dbo.Employees", column 'IdGroup'.

                var innerException = context.Exception;
                while (context.Response == null && innerException != null)
                {
                    var match = uq_rgx.Match(innerException.Message);

                    if (match.Success)
                    {
                        var field = ((System.Data.Entity.Infrastructure.DbUpdateException)context.Exception).Entries.First().Entity.GetType()
                            .GetProperties()
                            .FirstOrDefault(p => p.GetCustomAttributes(true).FirstOrDefault(a => a.GetType() == typeof(IndexAttribute) && ((IndexAttribute)a).Name == match.Groups["index"].ToString()) != null);

                        if (field != null)
                            context.Response = context.Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, new { Error = String.Format("Cannot insert duplicate {0} in {1}.", field.Name, match.Groups["table"].ToString()) });

                        break;
                    }
                    else
                    {
                        match = fk_rgx.Match(innerException.Message);

                        if (match.Success)
                        {
                            context.Response = context.Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, new { Error = "Cannot delete record associated with other records." });

                            break;
                        }
                        else
                            innerException = innerException.InnerException;
                    }
                }
            }
        }
    }

    public class RpoIdentityStartup
    {
        public virtual void CustomizeConfig(HttpConfiguration httpConfiguration)
        {

        }

        public virtual void CustomizeApp(IAppBuilder app)
        {

        }

        public RpoIdentityStartup(Func<RpoIdentityDbContext> createCallback)
        {
            _createCallback = createCallback;
        }

        private Func<RpoIdentityDbContext> _createCallback = null;
        public static OAuthAuthorizationServerOptions OAuthServerOptions { get; private set; }
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfiguration = new HttpConfiguration();

            ConfigureOAuthTokenGeneration(app);
            ConfigureOAuthTokenConsumption(app);
            ConfigureWebApi(httpConfiguration);
            ConfigureRoutes(httpConfiguration);

            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseCors(new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(new CorsPolicy
                    {
                        AllowAnyOrigin = true,
                        AllowAnyMethod = true,
                        AllowAnyHeader = true,
                        ExposedHeaders = { "Content-Disposition" }
                    })
                }
            });

            CustomizeConfig(httpConfiguration);

            app.UseWebApi(httpConfiguration);

            CustomizeApp(app);
        }

        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            // Configure the db context, user manager, signin manager and role manager to use a single instance per request
            if (_createCallback != null)
                app.CreatePerOwinContext(_createCallback);
            else
                app.CreatePerOwinContext(RpoIdentityDbContext.Create);

            app.CreatePerOwinContext<RpoUserManager>(RpoUserManager.Create);
            app.CreatePerOwinContext<RpoSignInManager>(RpoSignInManager.Create);
            app.CreatePerOwinContext<RpoRoleManager>(RpoRoleManager.Create);

            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(30),
                Provider = new AuthorizationServerProvider(),
                RefreshTokenProvider = new RefreshTokenProvider(),
                AccessTokenFormat = new RpoJwtFormat(ConfigurationManager.AppSettings["server:IdentityServerUri"])
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {
            string issuer = ConfigurationManager.AppSettings["server:IdentityServerUri"];
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
                });
        }

        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            //config.Formatters.XmlFormatter.SupportedMediaTypes -> application/xml e text/xml
            while (config.Formatters.XmlFormatter.SupportedMediaTypes.Count > 0)
                config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(config.Formatters.XmlFormatter.SupportedMediaTypes[0]);

            JsonMediaTypeFormatter formatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            formatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            config.Filters.Add(new CustomExceptionFilterAttribute());

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Filters.Add(new HeaderFilter());
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
        }

        private void ConfigureRoutes(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", defaults: new { id = RouteParameter.Optional });
        }
    }
}
