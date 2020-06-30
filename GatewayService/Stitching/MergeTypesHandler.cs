using HotChocolate.Stitching.Merge;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Stitching
{
    public class MergeTypesHandler : ITypeMergeHandler
    {
        private readonly MergeTypeRuleDelegate _next;

        public MergeTypesHandler(MergeTypeRuleDelegate next) => _next = next;

        /// <summary>
        /// Type names that are used in all our schemas that can be safely merged.
        /// </summary>
        private static readonly HashSet<string> _typesThatAreExactlyTheSame = new HashSet<string>
        {
            "Address"
        };

        public void Merge(ISchemaMergeContext context, IReadOnlyList<ITypeInfo> types)
        {
            if (!_typesThatAreExactlyTheSame.Contains(types.First().Definition.Name.Value))
            {
                _next(context, types);
            }
        }
    }
}
