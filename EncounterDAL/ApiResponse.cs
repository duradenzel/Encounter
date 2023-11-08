using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterDAL
{
    public class ApiResponse<T>
    {
        public List<T> Results { get; set; }
    }
}
