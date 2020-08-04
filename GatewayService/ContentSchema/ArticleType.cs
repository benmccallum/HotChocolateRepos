using HotChocolate.Types;
using HotChocolate.Types.Relay;
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
                    var fields = ctx.CollectFields(this);

                    return await Task.FromResult(new ArticleDto(id));
                });
        }
    }

    public class ArticleDto
    {
        public int Id { get; set; }

        public int[] TaskIds { get; set; } = { 1, 5, 101, 105 };

        public string Author { get; set; } = "some author";

        public ArticleDto(int id)
        {
            Id = id;
        }
    }
}
