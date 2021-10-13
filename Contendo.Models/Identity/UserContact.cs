using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contendo.Models.Identity
{
    public class UserContact
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public Guid ContactId { get; set; }
        public virtual User Contact { get; set; }
    }
}
