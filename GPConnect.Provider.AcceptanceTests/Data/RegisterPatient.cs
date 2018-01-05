namespace GPConnect.Provider.AcceptanceTests.Data
{
    public class RegisterPatient
    {
        public string SPINE_NHS_NUMBER { get; set; }
        public string NAME_FAMILY { get; set; }
        public string NAME_GIVEN { get; set; }
        public string GENDER { get; set; }
        public string DOB { get; set; }
        public bool IsRegistered { get; set; }
    }
}
