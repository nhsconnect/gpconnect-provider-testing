namespace GPConnect.Provider.AcceptanceTests.Steps
{
    using System.Collections.Generic;
    using System.Linq;
    using Context;
    using Hl7.Fhir.Model;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    internal sealed class DeviceSteps : BaseSteps
    {
        public DeviceSteps(FhirContext fhirContext, HttpSteps httpSteps) : base(fhirContext, httpSteps)
        {
        }

        private List<Device> Devices => _fhirContext.Devices;


        public void TheDeviceShouldBeValid()
        {
            TheDeviceShouldExcludeFields();
            TheDeviceIdentifierShouldBeValid();
            TheDeviceTypeShouldBeValid();
            TheDeviceNoteShouldBeValid();
        }

        [Then(@"the Device should exclude fields")]
        public void TheDeviceShouldExcludeFields()
        {
            Devices.ForEach(device =>
            {
                device.Status.ShouldBeNull();
                device.ManufactureDate.ShouldBeNull();
                device.Expiry.ShouldBeNull();
                device.Udi.ShouldBeNull();
                device.LotNumber.ShouldBeNull();
                device.Patient.ShouldBeNull();
                device.Contact?.Count.ShouldBe(0);
                device.Url.ShouldBeNull();
            });
        }

        [Then(@"the Device Identifier should be valid")]
        public void TheDeviceIdentifierShouldBeValid()
        {
            Devices.ForEach(device =>
            {
                device.Identifier.Count.ShouldBeLessThanOrEqualTo(1);

                device.Identifier.ForEach(identifier =>
                {
                    identifier.Value.ShouldNotBeNullOrEmpty();
                });
            });
        }

        [Then(@"the Device Type should be valid")]
        public void TheDeviceTypeShouldBeValid()
        {
            Devices.ForEach(device =>
            {
                device.Type.ShouldNotBeNull();
                device.Type.Coding.Count.ShouldBe(1);

                var coding = device.Type.Coding.First();

                coding.System.ShouldBe("http://snomed.info/sct");
                coding.Code.ShouldBe("462240000");
                coding.Display.ShouldBe("Patient health record information system (physical object)");
            });
        }

        [Then(@"the Device Note should be valid")]
        public void TheDeviceNoteShouldBeValid()
        {
            Devices.ForEach(device =>
            {
                if (device.Note != null)
                {
                    device.Note.Count.ShouldBeLessThanOrEqualTo(1);
                    device.Note.ForEach(note =>
                    {
                        note.Text.ShouldNotBeNullOrEmpty();
                    });
                }
            });
        }
    }
}
