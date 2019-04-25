using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ClientAbstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RequestHandlers;
using RequestHandlers.Behaviors;
using Swashbuckle.AspNetCore.Swagger;
using ViewModels.Common;
using ViewModels.Features.Startup;
using ProblemDetails = ViewModels.Common.ProblemDetails;

namespace DevTalks1
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddHttpClient<IJokeClient, DadJokeClient.DadJokeClient>();
            services.AddSingleton<IJokeClient, MockJokeClient.MockJokeClient>();

            services.AddScoped(typeof(IPipelineBehavior<WrappedRequest<MOTD.Request, MOTD.Response>, WrappedResponse<MOTD.Response>>), typeof(RequestLoggingBehavior));

            services.AddScoped(typeof(IPipelineBehavior<WrappedRequest<MOTD.Request, MOTD.Response>, WrappedResponse<MOTD.Response>>), typeof(MoreCowbellMOTDBehavior));
            services.AddMediatR(typeof(HandlerMarker).Assembly);

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                c.CustomSchemaIds(t=>t.FullName);
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "DevTalks1.xml");
                c.IncludeXmlComments(filePath,true);                
            });
        }

        ///Configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            //app.UseDeveloperExceptionPage();
            app.UseExceptionHandler(new ExceptionHandlerOptions()
            {
                ExceptionHandler = ExceptionToJSON
            });

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Startup}/{action=Index}/{id?}");
            });
        }

        public static async Task ExceptionToJSON(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (ex == null) return;
            var message = ex.Message;
            var detail = ex.StackTrace;

            var error = new ProblemDetails(detail, message, 500);
            context.Response.ContentType = "application/json";

            using (var writer = new StreamWriter(context.Response.Body))
            {
                new JsonSerializer().Serialize(writer, error);
                await writer.FlushAsync().ConfigureAwait(false);
            }
        }





    }
}
