using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contendo.Models
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
