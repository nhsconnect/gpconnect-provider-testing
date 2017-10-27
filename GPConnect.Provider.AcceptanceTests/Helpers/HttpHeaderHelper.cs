using System.Collections.Generic;
using GPConnect.Provider.AcceptanceTests.Logger;

// ReSharper disable ClassNeverInstantiated.Global

namespace GPConnect.Provider.AcceptanceTests.Helpers
{
    public class HttpHeaderHelper
    {
        private readonly Dictionary<string, string> _requestHeaders;

        public HttpHeaderHelper()
        {
            Log.WriteLine("HttpHeaderHelper() Constructor");
            _requestHeaders = new Dictionary<string, string>();
        }

        public void AddHeader(string key, string value)
        {
            _requestHeaders.Add(key, value);
            Log.WriteLine("Added Key='{0}' Value='{1}'", key, value);
        }

        public void ReplaceHeader(string key, string value)
        {
            RemoveHeader(key);
            AddHeader(key, value);
            Log.WriteLine("Replaced Key='{0}' With Value='{1}'", key, value);
        }

        public void RemoveHeader(string key)
        {
            _requestHeaders.Remove(key);
            Log.WriteLine("Removed Key='{0}'", key);
        }

        public Dictionary<string, string> GetRequestHeaders()
        {
            Log.WriteLine("GetRequestHeaders Count='{0}'", _requestHeaders.Count);
            return _requestHeaders;
        }

        public void SetRequestHeaders(Dictionary<string, string> headers)
        {
            _requestHeaders.Clear();
            foreach (var header in headers) {
                _requestHeaders.Add(header.Key, header.Value);
            }
        }

        public string GetHeaderValue(string key)
        {
            string value;
            _requestHeaders.TryGetValue(key, out value);
            Log.WriteLine("Header Key='{0}' Value='{1}'", key, value);
            return value;
        }

        public void Clear()
        {
            _requestHeaders.Clear();
            Log.WriteLine("All Header(s) Cleared");
        }

    }
}
