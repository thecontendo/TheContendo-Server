using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Contendo.Models
{
    public class BaseModel
    {
        [Key]
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        
        //[JsonIgnore]
        public string Description { get; set; }
    }
}
