using HotChocolate;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace GatewayService.ContentSchema
{
    public static class ContentServiceCollectionExtensions
    {
        public static ISchemaBuilder AddContentServicesAndGetSchemaBuilder(this IServiceCollection services)
        {
            var schemaBuilder = SchemaBuilder.New()
                .AddQueryType(d => d.Name("Query"))
                //.AddAuthorizeDirectiveType()
                .EnableRelaySupport()
                // Add types
                .AddOurCommonGraphQLTypes()
                .AddType<ArticleType>()
                .AddType<ArticleQueryTypeExtensions>()
                ;

            return schemaBuilder;
        }
    }
}
