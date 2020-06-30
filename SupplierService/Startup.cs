using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared;

namespace SupplierService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGraphQLCustom(schemaBuilder =>
            {
                schemaBuilder
                    .AddType<SupplierType>()
                    .AddType<SupplierTypeExtension>()
                    .AddType<ServiceAddressType>()
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

        [ExtendObjectType(Name = "Query")]
        public class QueryTypeExtension
        {
            public SupplierDto GetSupplier(int dbId)
                => new SupplierDto() { Name = "Supplier Name " + dbId };
        }
    }
}
