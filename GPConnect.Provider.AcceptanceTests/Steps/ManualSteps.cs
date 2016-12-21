using TechTalk.SpecFlow;

// https://decoupledlogic.wordpress.com/2013/12/10/specflow-manual-testing/
namespace GPConnect.Provider.AcceptanceTests.Steps
{
    [Binding, Scope(Tag = "Manual")]
    public class ManualSteps
    {
        [Given(".*"), When(".*"), Then(".*")]
        public void EmptyStep()
        {
        }

        [Given(".*"), When(".*"), Then(".*")]
        public void EmptyStep(string multiLineStringParam)
        {
        }

        [Given(".*"), When(".*"), Then(".*")]
        public void EmptyStep(Table tableParam)
        {
        }
    }
}
