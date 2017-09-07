using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPConnect.Provider.AcceptanceTests.Models
{
    public class GpcCode
    {
        public GpcCode(string code, string display)
        {
            Code = code;
            Display = display;
        }
        public string Code;
        public string Display;


    }


}
