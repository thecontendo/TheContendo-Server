namespace Contendo.Models.Identity
{
    public class Address: BaseModel
    {
        public string City { get; set; }
        
        public string State { get; set; }
        
        public string Country { get; set; }
    }
}