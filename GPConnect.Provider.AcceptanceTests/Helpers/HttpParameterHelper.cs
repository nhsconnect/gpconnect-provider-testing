using System.Collections.Generic;
using GPConnect.Provider.AcceptanceTests.Logger;

namespace GPConnect.Provider.AcceptanceTests.Helpers
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class HttpParameterHelper
    {
        private readonly Dictionary<string, string> _parameters;

        private HttpParameterHelper()
        {
            Log.WriteLine("HttpHelper() Constructor");
            _parameters = new Dictionary<string, string>();
        }

        public void AddParameter(string key, string value)
        {
            _parameters.Add(key, value);
            Log.WriteLine("Added Key='{0}' Value='{1}'", key, value);
        }
        
        public Dictionary<string, string> GetRequestParameters()
        {
            Log.WriteLine("GetRequestParameters Count='{0}'", _parameters.Count);
            return _parameters;
        }

        public string GetParameterValue(string key)
        {
            string value;
            _parameters.TryGetValue(key, out value);
            Log.WriteLine("Parameter Key='{0}' Value='{1}'", key, value);
            return value;
        }

        public void ClearParameters()
        {
            _parameters.Clear();
            Log.WriteLine("All Parameters Cleared");
        }

    }

}
