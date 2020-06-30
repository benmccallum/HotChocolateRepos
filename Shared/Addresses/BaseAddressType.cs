using HotChocolate.Types;

namespace Shared
{
    public abstract class BaseAddressType<TAddressDto> : ObjectType<TAddressDto>
        where TAddressDto : AddressDto
    {
        protected override void Configure(IObjectTypeDescriptor<TAddressDto> descriptor)
        {
            descriptor.Field(a => a.SuburbID).Name("suburbDbId");
        }
    }
}
