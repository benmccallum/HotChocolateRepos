using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GatewayService.ContentSchema
{
    public class ArticleType : ObjectType<ArticleDto>
    {
        protected override void Configure(IObjectTypeDescriptor<ArticleDto> descriptor)
        {
            descriptor
                .AsNode()
                .IdField(a => a.Id)
                .NodeResolver(async (ctx, id) =>
                {
                    // Attempt same way as in ArticleQueryTypeExtensions, fails
                    var articleType = ctx.Schema.Types.Single(t => t.Name.Value == "Article");
                    var fields = ctx.CollectFields(articleType as ObjectType);

                    // Attempt way I'd ideally want to work
                    // (so I can do this from a base class that can resolve its sub types in one way)
                    fields = ctx.CollectFields(this);

                    return await Task.FromResult(new ArticleDto(id, fields));
                });
        }
    }

    public class ArticleDto
    {
        public int Id { get; set; }

        public int[] TaskIds { get; set; } = { 1, 5, 101, 105 };

        public string[] FieldsCollected { get; set; }

        public string Author { get; set; } = "some author";

        public ArticleDto(int id, IReadOnlyList<IFieldSelection> fieldsCollected)
        {
            Id = id;
            FieldsCollected = fieldsCollected.Select(f => f.Field.Name.Value).ToArray();
        }
    }
}
