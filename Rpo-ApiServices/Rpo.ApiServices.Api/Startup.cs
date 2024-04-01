// ***********************************************************************
// Assembly         : Rpo.ApiServices.Api
// Author           : Prajesh Baria
// Created          : 12-14-2017
//
// Last Modified By : Prajesh Baria
// Last Modified On : 05-24-2018
// ***********************************************************************
// <copyright file="Startup.cs" company="CREDENCYS">
//     Copyright ©  2017
// </copyright>
// <summary>Class Required Header Filter.</summary>
// ***********************************************************************


/// <summary>
/// The Api namespace.
/// </summary>
[assembly: Microsoft.Owin.OwinStartup("RpoStartup", typeof(Rpo.ApiServices.Api.StartUp))]
namespace Rpo.ApiServices.Api
{
    using Microsoft.Owin.Cors;
    using Owin;
    using Rpo.ApiServices.Api.Filters;
    using Rpo.ApiServices.Model;
    using Swashbuckle.Application;
    using Swashbuckle.Swagger;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Web.Http;
    using System.Web.Http.Description;
    using System.Web.Http.ExceptionHandling;
    using WebDemoAPI.CustomHandler;    /// <summary>
                                       /// Class Required Header Filter.
                                       /// </summary>
                                       /// <seealso cref="Swashbuckle.Swagger.IOperationFilter" />
    public class RequiredHeaderFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="schemaRegistry">The schema registry.</param>
        /// <param name="apiDescription">The API description.</param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();

            operation.parameters.Add(new Parameter
            {
                name = "Authorization",
                @in = "header",
                type = "string",
                required = true
            });
            operation.parameters.Add(new Parameter
            {
                name = "isSaveAndExit",
                @in = "header",
                type = "string",
                required = true,
                @default = "true"
            });
            operation.parameters.Add(new Parameter
            {
                name = "currentTimeZone",
                @in = "header",
                type = "string",
                required = true,
                @default = "India Standard Time",
            });
        }
    }

    /// <summary>
    /// Class StartUp.
    /// </summary>
    /// <seealso cref="Rpo.Identity.Core.Infrastructure.RpoIdentityStartup" />
    public class StartUp : Rpo.Identity.Core.Infrastructure.RpoIdentityStartup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartUp"/> class.
        /// </summary>
        public StartUp()
            : base(RpoContext.CreateRpoContext)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<RpoContext, Rpo.ApiServices.Model.Migrations.Configuration>());
        }

        /// <summary>
        /// Customizes the application.
        /// </summary>
        /// <param name="app">The application.</param>
        public override void CustomizeApp(IAppBuilder app)
        {
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);

                map.RunSignalR();
            });

            Jobs.Register.All();
        }

        /// <summary>
        /// Gets the XML comments path.
        /// </summary>
        /// <returns>System.String.</returns>
        protected static string GetXmlCommentsPath()
        {
            return System.String.Format(@"{0}bin\Rpo.ApiServices.Api.xml", System.AppDomain.CurrentDomain.BaseDirectory);
        }

        /// <summary>
        /// Customizes the configuration.
        /// </summary>
        /// <param name="httpConfiguration">The HTTP configuration.</param>
        public override void CustomizeConfig(HttpConfiguration httpConfiguration)
        {
            // Web API routes
          //  httpConfiguration.MapHttpAttributeRoutes();
            //Registering GlobalExceptionHandler
            //httpConfiguration.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            //Registering UnhandledExceptionLogger
           httpConfiguration.Services.Replace(typeof(IExceptionLogger), new UnhandledExceptionLogger());

            //Registering RequestResponseHandler
          httpConfiguration.MessageHandlers.Add(new RequestResponseHandler());

            httpConfiguration.Filters.Add(new RpoAuthorizeActionFilter());
            httpConfiguration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Rpo.ApiServices.Api");
                    c.OperationFilter<RequiredHeaderFilter>();
                    c.IncludeXmlComments(GetXmlCommentsPath());
                    c.SchemaId(x => x.FullName);
                   // c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                })
                .EnableSwaggerUi();
        }
    }
}