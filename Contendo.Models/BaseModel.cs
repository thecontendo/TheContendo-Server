using System;
using System.ComponentModel.DataAnnotations;
using Vistex.Cloud.Services.Models.Tenants;

namespace Vistex.Cloud.Services.Models
{
    public class BaseModel
    {
        [Key]
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public string Description { get; set; }
    }
}
