using System;

namespace GPConnect.Provider.AcceptanceTests.Helpers
{
    public interface IGlobalContextHelper
    {
        Guid TestRunId { get; }
        T GetValue<T>();
        T GetValue<T>(string key);
        void SaveValue<T>(T value);
        void SaveValue<T>(string key, T value);
    }
}