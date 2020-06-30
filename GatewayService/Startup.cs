using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Subscriptions;
using HotChocolate.Execution;
using HotChocolate.Stitching;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared;
using System;
using System.IO;
using WebApplication1.ContentSchema;
using WebApplication1.Stitching;

namespace WebApplication1
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // GraphQL
            AddGraphQL(services);
        }

        private void AddGraphQL(IServiceCollection services)
        {
            services.AddOurCommonGraphQLServices();

            var contentSchemaBuilder = services.AddContentServicesAndGetSchemaBuilder();

            services
                .AddGraphQLSubscriptions()
                .AddStitchedSchema(stitchingBuilder => stitchingBuilder
                    .AddSchemaFromHttp("Supplier", _env, _config, services)
                    .AddSchemaFromHttp("Location", _env, _config, services)
                    .AddDirectiveMergeHandler<MergeDirectivesHandler>()
                    .AddTypeMergeHandler<MergeTypesHandler>()
                    .AddExtensionsFromFileForService("Shared", _env)
                    .AddSchemaConfiguration(config =>
                    {
                        config.Options.UseXmlDocumentation = true;
                        config.Options.RemoveUnreachableTypes = true;

                        config.RegisterExtendedScalarTypes();
                        //config.RegisterAuthorizeDirectiveType();
                        //config.RegisterDirective<DeprecatedDirectiveType>();

                        config.AddOurCommonGraphQLTypes();
                    })
                    .AddExecutionConfiguration(executionBuilder =>
                    {
                        executionBuilder.AddErrorFilter(_errorFilterFunc);
                    }));
        }

        private static readonly Func<IError, IError> _errorFilterFunc = error =>
        {
            if (error.Extensions.TryGetValue("remote", out var o)
              && o is IError originalError)
            {
                return error.AddExtension(
                  "remote_code",
                  originalError.Code);
            }
            return error;
        };

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGraphQL();
            app.UsePlayground();
        }
    }

    public static class StartupHelperExtensions
    {
        public static IStitchingBuilder AddSchemaFromHttp(this IStitchingBuilder stitchingBuilder,
            string serviceName, IWebHostEnvironment env, IConfiguration config, IServiceCollection services)
        {
            var clientName = $"{serviceName}Client";
            var schemaName = $"AG_{serviceName}";

            // Register named HTTP client with schema name
            services.AddGraphQLClient(schemaName, config[$"{clientName}ApiUrl"]);

            // Add schema by using same schema name
            stitchingBuilder.AddSchemaFromHttp(schemaName);

            // Add extensions (for types that get stitched in from this service)
            stitchingBuilder.AddExtensionsFromFileForService(serviceName, env);

            return stitchingBuilder;
        }

        public static IStitchingBuilder AddSchemaFromLocal(this IStitchingBuilder stitchingBuilder,
            string serviceName, ISchemaBuilder schemaBuilder, IWebHostEnvironment env, IServiceCollection services)
        {
            var schemaName = serviceName.Substring(3);

            stitchingBuilder.AddQueryExecutor(schemaName, sp => schemaBuilder.AddServices(sp).Create().MakeExecutable());

            // Add extensions (for types that get stitched in from this service)
            stitchingBuilder.AddExtensionsFromFileForService(serviceName, env);

            return stitchingBuilder;
        }

        public static IStitchingBuilder AddExtensionsFromFileForService(this IStitchingBuilder stitchingBuilder,
            string serviceName, IWebHostEnvironment env)
        {
            var filePaths = Directory.GetFiles($"{env.ContentRootPath}/Stitching/{serviceName}", "*.graphql");
            foreach (var filePath in filePaths)
            {
                stitchingBuilder.AddExtensionsFromFile(filePath);
            }
            return stitchingBuilder;
        }
    }
}
