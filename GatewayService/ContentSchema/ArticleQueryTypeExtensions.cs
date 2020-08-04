using HotChocolate.Resolvers;
using HotChocolate.Types;
using System.Linq;
using System.Threading.Tasks;

namespace GatewayService.ContentSchema
{
    [ExtendObjectType(Name = "Query")]
    public class ArticleQueryTypeExtensions
    {
        public async Task<ArticleDto> GetArticleAsync(
            IResolverContext ctx,
            int dbId)
        {
            var articleType = ctx.Schema.Types.Single(t => t.Name.Value == "Article");
            var fields = ctx.CollectFields(articleType as ObjectType);

            return await Task.FromResult(new ArticleDto(dbId));
        }
    }
}
