using HotChocolate.Types;

namespace GatewayService.TaskSchema
{
    public class RepairType : ObjectType<RepairDto>
    {

    }

    public class RepairDto : TaskDto
    {
        public RepairDto(int dbId) : base(dbId)
        {
        }
    }
}
