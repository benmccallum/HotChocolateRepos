using HotChocolate;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace WebApplication1.ContentSchema
{
    public static class ContentServiceCollectionExtensions
    {
        public static ISchemaBuilder AddContentServicesAndGetSchemaBuilder(this IServiceCollection services)
        {
            var schemaBuilder = SchemaBuilder.New()
                .AddQueryType(d => d.Name("Query"))
                //.AddAuthorizeDirectiveType()
                //.EnableRelaySupport()
                .ModifyOptions(o =>
                {
                    o.UseXmlDocumentation = true;
                    //o.RemoveUnreachableTypes = true; // TODO: Re-enable once https://github.com/ChilliCream/hotchocolate/issues/1669 is fixed
                })
                // Add types
                .AddOurCommonGraphQLTypes()
                .AddType<ArticleType>()
                //.AddType<ArticleTypeExtension>()
                ;

            return schemaBuilder;
        }
    }
}
