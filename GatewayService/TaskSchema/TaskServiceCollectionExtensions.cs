using HotChocolate;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace GatewayService.TaskSchema
{
    public static class TaskServiceCollectionExtensions
    {
        public static ISchemaBuilder AddTaskServicesAndGetSchemaBuilder(this IServiceCollection services)
        {
            var schemaBuilder = SchemaBuilder.New()
                .AddQueryType(d => d.Name("Query"))
                //.AddAuthorizeDirectiveType()
                //.EnableRelaySupport()
                // Add types
                .AddOurCommonGraphQLTypes()
                .AddType<TaskUnionType>()
                .AddType<RepairType>()
                .AddType<InspectionType>()
                .AddType<TaskQueryTypeExtension>()
                ;

            return schemaBuilder;
        }
    }
}
