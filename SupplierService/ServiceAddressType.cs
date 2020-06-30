using Shared;

namespace SupplierService
{
    public class ServiceAddressType : BaseAddressType<ServiceAddressDto>
    {
    }

    public class ServiceAddressDto : AddressDto
    {
        public int ServiceRadius { get; set; } = 10;

        public ServiceAddressDto(string addressLine1, int suburbId) 
            : base(addressLine1, suburbId) 
        { }
    }
}
