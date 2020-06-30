namespace Shared
{
    public class AddressDto
    {
        public string AddressLine1 { get; set; }

        public int SuburbID { get; set; }

        public AddressDto(string addressLine1, int suburbId)
        {
            AddressLine1 = addressLine1;
            SuburbID = suburbId;
        }
    }
}
