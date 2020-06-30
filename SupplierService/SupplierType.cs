using HotChocolate.Types;

namespace SupplierService
{
    public class SupplierType : ObjectType<SupplierDto> { }

    public class SupplierDto
    {
        public string Name { get; set; } = "Suppier Name";
    }
}
