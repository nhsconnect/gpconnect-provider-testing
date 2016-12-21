using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding]
    public sealed class AccessRecordSteps : TechTalk.SpecFlow.Steps
    {
        private Dictionary<string, string> _patientDictionary;

        [Given(@"I have the following patient records")]
        public void GivenIHaveTheFollowingPatientRecords(Table table)
        {
            _patientDictionary = new Dictionary<string, string>();
            foreach (var row in table.Rows)
                _patientDictionary.Add(row["Id"], row["NHSNumber"]);
        }

        [Given(@"I am requesting the record for config patient ""([^""]*)""")]
        public void GivenIAmRequestingTheRecordForConfigPatient(string patient)
        {
            Given($@"I am requesting the record for patient with NHS Number ""{_patientDictionary[patient]}""");
        }
    }
}
