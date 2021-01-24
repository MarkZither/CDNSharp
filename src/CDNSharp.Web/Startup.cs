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
            services.AddControllers();
            services.Configure<LiteDbOptions>(Configuration.GetSection("LiteDbOptions"));
            services.AddSingleton<ICDNService, CDNService>();
            services.AddSingleton<ILiteDbContext, LiteDbContext>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CDBSharp Service", Version = "v1" });
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
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //GraphQL
            app.UseGraphQL<FileSchema>();

            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());
        }
    }
}
