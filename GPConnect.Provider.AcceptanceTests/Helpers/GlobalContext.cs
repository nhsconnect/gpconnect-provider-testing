using System;
using System.Collections.Generic;
// ReSharper disable ClassNeverInstantiated.Global

// http://www.hackered.co.uk/articles/my-specflow-global-test-context-service

namespace GPConnect.Provider.AcceptanceTests.Helpers
{

    public class GlobalContext
    {
        private static readonly Dictionary<string, object> GlobalContextItems = new Dictionary<string, object>();

        private static readonly Guid TestRunIdentifier;

        static GlobalContext()
        {
            TestRunIdentifier = Guid.NewGuid();
        }

        public static Guid TestRunId
        {
            get
            {
                return TestRunIdentifier;
            }
        }

        public static void SaveValue<T>(T value)
        {
            if (value.Equals(default(T)))
            {
                throw new Exception("Value cannot be default value");
            }

            var key = typeof(T).FullName;
            SaveValue(key, value);
        }

        public static void SaveValue<T>(string key, T value)
        {
            if (GlobalContextItems.ContainsKey(key))
            {
                GlobalContextItems[key] = value;
            }
            else
            {
                GlobalContextItems.Add(key, value);
            }
        }

        public static T GetValue<T>()
        {
            var key = typeof(T).FullName;

            return GetValue<T>(key);
        }

        public static T GetValue<T>(string key)
        {
            if (!GlobalContextItems.ContainsKey(key))
            {
                return default(T);
            }

            return (T)GlobalContextItems[key];
        }
    }
}
