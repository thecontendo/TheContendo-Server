using System.ComponentModel.DataAnnotations;
using Contendo.Models.Enums;
using Contendo.Models.Identity;

namespace Contendo.Models.Identity.Dto
{
    public class UserDto: BaseModel
    {
        public string Title { get; set; }
        
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        [Required] 
        public string LastName { get; set; }
        
        public string ValidFrom { get; set; }
        
        public string ValidTo { get; set; }
        
        public int Status { get; set; }
        
        public int AuthType { get; set; }
        
        public string Photo { get; set; }
        public string Height { get; set; }
        
        public int Gender { get; set; }
        
        public string Weight { get; set; }
        
        public Address Address { get; set; }
    }
}