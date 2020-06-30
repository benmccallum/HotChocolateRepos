using HotChocolate.Types;

namespace WebApplication1.ContentSchema
{
    public class ArticleType : ObjectType<ArticleDto>
    {

    }

    public class ArticleDto
    {
        public string Author { get; set; } = "some author";
    }
}
