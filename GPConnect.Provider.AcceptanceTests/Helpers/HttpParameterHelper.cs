using System.Collections.Generic;
using GPConnect.Provider.AcceptanceTests.Logger;

namespace GPConnect.Provider.AcceptanceTests.Helpers
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class HttpParameterHelper
    {
        private readonly List<KeyValuePair<string, string>> _parameters;

        public HttpParameterHelper()
        {
            Log.WriteLine("HttpHelper() Constructor");
            _parameters = new List<KeyValuePair<string, string>>();
        }

        public void AddParameter(string key, string value)
        {
            _parameters.Add(new KeyValuePair<string,string>(key, value));
            Log.WriteLine("Added Key='{0}' Value='{1}'", key, value);
        }
        
        public List<KeyValuePair<string, string>> GetRequestParameters()
        {
            Log.WriteLine("GetRequestParameters Count='{0}'", _parameters.Count);
            return _parameters;
        }

        public string GetParameterValue(string key)
        {
            foreach (var parameter in _parameters) {
                if (parameter.Key.Equals(key)) {
                    return parameter.Value;
                }
            }
            return "";
        }

        public void ClearParameters()
        {
            _parameters.Clear();
            Log.WriteLine("All Parameters Cleared");
        }

    }

}
