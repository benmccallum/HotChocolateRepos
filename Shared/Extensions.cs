using HotChocolate;
using HotChocolate.Configuration;
using HotChocolate.Execution;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Relay;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Shared
{
    public static class Extensions
    {
        private static readonly List<Type> _commonTypes = new List<Type>
        {
            typeof(AddressType)
        };

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

            return services;
        }

        public static IServiceCollection AddOurCommonGraphQLServices(this IServiceCollection services)
            => services
                .AddDataLoaderRegistry()
                .AddSingleton<INamingConventions, NamingConventions>()
                .AddSingleton<IIdSerializer, IdSerializer>();

        public static ISchemaBuilder AddOurCommonGraphQLTypes(this ISchemaBuilder schemaBuilder)
        {
            _commonTypes.ForEach(t => schemaBuilder.AddType(t));

            // Ensure string still defaults to StringType
            schemaBuilder.BindClrType<string, StringType>();

            // Set a maximum page size
            schemaBuilder
                .AddType(new PaginationAmountType(50))
                .BindClrType<int, IntType>();

            return schemaBuilder;
        }

        public static ISchemaConfiguration AddOurCommonGraphQLTypes(this ISchemaConfiguration schemaConfig)
        {
            _commonTypes.ForEach(t => schemaConfig.RegisterType(t));
            return schemaConfig;
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
