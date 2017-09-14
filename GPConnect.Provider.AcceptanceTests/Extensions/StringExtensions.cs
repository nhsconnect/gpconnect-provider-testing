using System.Text.RegularExpressions;

namespace GPConnect.Provider.AcceptanceTests.Extensions
{
    public static class StringExtensions
    {
        public static bool IsLanguageFormat(this string str)
        {
            //This is a loose check and does not guarantee a valid lanaguage tag
            //just that it is formatted well
            return !string.IsNullOrEmpty(str) && Regex.IsMatch(str, @"^[a-zA-Z]{1,8}(?:-[a-zA-Z0-9]{1,8})*$");
        }
    }
}
