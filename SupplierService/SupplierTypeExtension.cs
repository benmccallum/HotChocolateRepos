#nullable enable

using HotChocolate;
using HotChocolate.Types;
using Shared;

namespace SupplierService
{
    [ExtendObjectType(Name = "Supplier")]
    public class SupplierTypeExtension
    {
        public ServiceAddressDto GetServiceAddress([Parent] SupplierDto s)
            => new ServiceAddressDto("Line 1 Service Address", 9);

        public AddressDto? GetBillingAddress([Parent] SupplierDto s)
            => new AddressDto("Line 1", 1);

        public AddressDto? GetShippingAddress([Parent] SupplierDto s)
            => new AddressDto("Line 1 Shipping", 2);
    }
}
