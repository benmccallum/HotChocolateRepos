using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared;

namespace LocationService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGraphQLCustom(schemaBuilder =>
            {
                schemaBuilder
                    .AddType<SuburbType>()
                    .AddType<QueryTypeExtension>()
                    .ModifyOptions(o =>
                    {
                        // TODO: Remove once https://github.com/ChilliCream/hotchocolate/issues/1669 is fixed
                        o.RemoveUnreachableTypes = false;
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGraphQL();
            app.UsePlayground();
        }

        public class SuburbType : ObjectType<SuburbDto> { }
        public class SuburbDto
        {
            public string Name { get; set; } = "Suburb Name";
        }

        [ExtendObjectType(Name = "Query")]
        public class QueryTypeExtension
        {
            public SuburbDto GetSuburbByDbId(int dbId)
                => new SuburbDto() { Name = "Suburb Name " + dbId };
        }
    }
}
