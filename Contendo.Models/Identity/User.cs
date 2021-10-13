using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Contendo.Models.Enums;
using Contendo.Models.Identity.Dto;

namespace Contendo.Models.Identity
{
    public class User: BaseModel
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
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public UserStatus Status { get; set; }
        
        public Address Address { get; set; }

        public string Height { get; set; }
        
        public Gender Gender { get; set; }
        
        public string Weight { get; set; }
        

        public string Photo { get; set; }


        public AuthType AuthType { get; set; }
        
        public List<UserContact> ContactUsers { get; set; }

        public List<UserContact> UserContacts { get; set; }
    }
}