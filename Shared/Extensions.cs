using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Relay;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Shared
{
    public static class Extensions
    {
        public static IServiceCollection AddGraphQLCustom(this IServiceCollection services,
            Action<ISchemaBuilder>? configureSchema = null,
            Action<IQueryExecutionBuilder>? configureExecutor = null)
        {
            services.AddOurCommonGraphQLServices();

            var schemaBuilder = SchemaBuilder.New()
                .AddOurCommonGraphQLTypes()
                .AddQueryType(d => d.Name("Query"))
                //.AddAuthorizeDirectiveType()
                //.EnableRelaySupport()
                .ModifyOptions(o =>
                {
                    o.UseXmlDocumentation = true;
                    o.RemoveUnreachableTypes = true;
                });

            configureSchema?.Invoke(schemaBuilder);

            if (configureExecutor != null)
            {
                services.AddGraphQL(sp => schemaBuilder.AddServices(sp).Create(), configureExecutor);
            }
            else
            {
                services.AddGraphQL(schemaBuilder);
            }

            services.RemoveAll<IIdSerializer>();
            services.AddSingleton<IIdSerializer>(new IdSerializer(includeSchemaName: true));

            return services;
        }

        public static IServiceCollection AddOurCommonGraphQLServices(this IServiceCollection services)
            => services
                .AddDataLoaderRegistry()
                .AddSingleton<INamingConventions, NamingConventions>()
                .AddSingleton<IIdSerializer, IdSerializer>();

        public static ISchemaBuilder AddOurCommonGraphQLTypes(this ISchemaBuilder schemaBuilder)
        {
            // Ensure string still defaults to StringType
            schemaBuilder.BindClrType<string, StringType>();

            // Set a maximum page size
            schemaBuilder
                .AddType(new PaginationAmountType(50))
                .BindClrType<int, IntType>();

            return schemaBuilder;
        }

        public static IServiceCollection AddGraphQLClient(this IServiceCollection services, string name, string apiUrl)
        {
            services.AddHttpContextAccessor();

            services
                .AddHttpClient(name, client =>
                {
                    client.BaseAddress = new Uri(apiUrl);//new Uri($"{apiUrl}graphql");
                    client.Timeout = TimeSpan.FromSeconds(10);
                })
                ;

            return services;
        }
    }
}
