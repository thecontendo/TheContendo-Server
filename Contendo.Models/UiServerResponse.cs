using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistex.Cloud.Services.Models
{
    public class UiServerResponse<T>: ServerResponse<T>
    {
        public UiServerResponse()
        {

        }

        public UiServerResponse(T data): base(data)
        {
        }

        public Dictionary<string, object> Dictionaries { get; set; }
    }
}
