using HotChocolate;
using HotChocolate.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GatewayService.TaskSchema
{
    [ExtendObjectType(Name = "Query")]
    public class TaskQueryTypeExtension
    {
        [GraphQLType(typeof(NonNullType<ListType<NonNullType<TaskUnionType>>>))]
        public async Task<IReadOnlyList<TaskDto>> GetTasksByDbIdsAsync(
            int[] dbIds)
        {
            var tasks = new List<TaskDto>();
            foreach (var dbId in dbIds)
            {
                tasks.Add(dbId < 100 ? (TaskDto)new RepairDto(dbId) : new InspectionDto(dbId));
            }
            return await Task.FromResult(tasks);
        }
    }
}
