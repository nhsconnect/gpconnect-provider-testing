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

        public string Code { get; set; }
        public string Display { get; set; }
        public string System { get; set; }
    }
}
