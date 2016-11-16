using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPConnect.Provider.AcceptanceTests.tools
{
    class HeaderController
    {
        private static HeaderController headerController;
        private static Dictionary<string, string> requestHeaders;

        private HeaderController() {
            requestHeaders = new Dictionary<string, string>();
        }

        public static HeaderController Instance
        {
            get
            {
                if (headerController == null)
                {
                    headerController = new HeaderController();

                }
                return headerController;
            }
        }

        public void addHeader(string key, string value) {
            requestHeaders.Add(key, value);
        }

        public void removeHeader(string key)
        {
            requestHeaders.Remove(key);
        }

        public Dictionary<string, string> getRequestHeaders() {
            return requestHeaders;
        }

        public void headerClearDown() {
            requestHeaders.Clear();
        }

    }
}
