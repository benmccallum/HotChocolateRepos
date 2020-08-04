using HotChocolate.Types;

namespace GatewayService.TaskSchema
{
    public class TaskUnionType : UnionType<TaskDto>
    {
        protected override void Configure(IUnionTypeDescriptor descriptor)
        {
            descriptor.Name("Task");
        }
    }

    public abstract class TaskDto
    {
        public int Id { get; set; }

        public string Name => $"some task {Id}";

        public TaskDto(int dbId)
        {
            Id = dbId;
        }
    }
}
