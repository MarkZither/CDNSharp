using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CDNSharp.Web.Configuration;
using CDNSharp.Web.DataAccess;
using CDNSharp.Web.Services;
using Microsoft.OpenApi.Models;
using GraphQL.Server;
using CDNSharp.Web.GraphQL.Schemas;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.OData.ModelBuilder;
using Microsoft.OData.Edm;
using CDNSharp.Web.Models;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.OData.Formatter;
using CDNSharp.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.IO;

namespace CDNSharp.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyDataContext>(opt => opt.UseLazyLoadingProxies().UseInMemoryDatabase("MyDataContextList"));

            //services.AddOData(opt => opt.AddModel("odata", GetEdmModel()));
            services.AddOData(opt => opt.Count().Filter().Expand().Select().OrderBy().SetMaxTop(5)
                .AddModel(GetEdmModel())
                //.AddModel("v1", model1)
                //.AddModel("v2{data}", model2, builder => builder
                //.AddService<ODataBatchHandler, DefaultODataBatchHandler>(Microsoft.OData.ServiceLifetime.Singleton))
                );

            //https://stackoverflow.com/questions/57236413/how-enable-swagger-for-asp-net-core-odata-api
            //https://stackoverflow.com/questions/50940593/integrate-swashbuckle-swagger-with-odata-in-asp-net-core/51599466#51599466
            services.AddControllers(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });



            services.Configure<LiteDbOptions>(Configuration.GetSection("LiteDbOptions"));
            services.AddSingleton<ICDNService, CDNService>();
            services.AddSingleton<ILiteDbContext, LiteDbContext>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "CDNSharp Service", Version = "v1",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Mark Burton",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/mark__burton"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                //https://github.com/OData/AspNetCoreOData/issues/41
                c.DocInclusionPredicate((name, api) => api.HttpMethod != null);

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });


            

            //GraphQL            
            services.AddSingleton<FileSchema>()
                    .AddGraphQL((options, provider) =>
                    {
                        options.EnableMetrics = false;
                        var logger = provider.GetRequiredService<ILogger<Startup>>();
                        options.UnhandledExceptionDelegate = ctx => logger.LogError("{Error} occured", ctx.OriginalException.Message);
                    })
                    .AddSystemTextJson()
                    .AddGraphTypes(typeof(FileSchema));
        }

        private IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<CDNFile>("CDNFile").EntityType.HasKey(c => c.Id);
            return builder.GetEdmModel();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "CDNSharp Service API V1");
                c.DisplayOperationId();
                c.DisplayRequestDuration();
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // A odata debuger route is only for debugger view of the all OData endpoint routing.
                endpoints.MapGet("/$odata", ODataRouteHandler.HandleOData);
                endpoints.MapControllers();
            });

            //GraphQL
            app.UseGraphQL<FileSchema>();

            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());
        }
    }
}
