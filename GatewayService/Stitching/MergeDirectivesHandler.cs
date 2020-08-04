using HotChocolate.Stitching.Merge;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GatewayService.Stitching
{
    public class MergeDirectivesHandler : IDirectiveMergeHandler
    {
        private readonly MergeDirectiveRuleDelegate _next;

        public MergeDirectivesHandler(MergeDirectiveRuleDelegate next) => _next = next;

        public void Merge(ISchemaMergeContext context, IReadOnlyList<IDirectiveTypeInfo> directives)
        {
            if (!directives.First().Definition.Name.Value.Equals("authorize", StringComparison.InvariantCultureIgnoreCase))
            {
                _next(context, directives);
            }
        }
    }
}
