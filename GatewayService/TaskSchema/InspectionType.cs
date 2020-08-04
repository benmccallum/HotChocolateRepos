using HotChocolate.Types;

namespace GatewayService.TaskSchema
{
    public class InspectionType : ObjectType<InspectionDto>
    {

    }

    public class InspectionDto : TaskDto
    {
        public InspectionDto(int dbId) : base(dbId)
        {
        }
    }
}
