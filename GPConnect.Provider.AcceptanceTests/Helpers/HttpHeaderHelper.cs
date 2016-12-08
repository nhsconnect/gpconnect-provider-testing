using System;
using System.Collections.Generic;

namespace GPConnect.Provider.AcceptanceTests.Helpers
{
    public class HttpHeaderHelper
    {
        private static HttpHeaderHelper httpHeaderHelper;

        private readonly Dictionary<string, string> _requestHeaders;

        private HttpHeaderHelper()
        {
            _requestHeaders = new Dictionary<string, string>();
        }

        public static HttpHeaderHelper Instance => httpHeaderHelper ?? (httpHeaderHelper = new HttpHeaderHelper());

        public void AddHeader(string key, string value)
        {
            _requestHeaders.Add(key, value);
            Console.WriteLine("Added Key='{0}' Value='{1}'", key, value);
        }

        public void ReplaceHeader(string key, string value)
        {
            RemoveHeader(key);
            AddHeader(key, value);
            Console.WriteLine("Replaced Key='{0}' With Value='{1}'", key, value);
        }

        public void RemoveHeader(string key)
        {
            _requestHeaders.Remove(key);
            Console.WriteLine("Removed Key='{0}'", key);
        }

        public Dictionary<string, string> GetRequestHeaders()
        {
            Console.WriteLine("GetRequestHeaders Count='{0}'", _requestHeaders.Count);
            return _requestHeaders;
        }

        public string GetHeaderValue(string key)
        {
            string value;
            _requestHeaders.TryGetValue(key, out value);
            Console.WriteLine("Header Key='{0}' Value='{1}'", key, value);
            return value;
        }

        public void Clear()
        {
            _requestHeaders.Clear();
            Console.WriteLine("All Header(s) Cleared");
        }

    }
}
