using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPConnect.Provider.AcceptanceTests.Models
{
    public class GpcCode
    {
        public GpcCode(string code, string display, string system = null)
        {
            Code = code;
            Display = display;
            System = system;
        }
        public string Code;
        public string Display;
        public string System;


    }


}
