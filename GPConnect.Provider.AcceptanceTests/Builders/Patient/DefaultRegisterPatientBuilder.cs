namespace GPConnect.Provider.AcceptanceTests.Builders.Patient
{
    using System.Collections.Generic;
    using Constants;
    using Data;
    using Helpers;
    using Hl7.Fhir.Model;

    public class DefaultRegisterPatientBuilder
    {
        private readonly RegisterPatient _registerPatient;

        public DefaultRegisterPatientBuilder(RegisterPatient registerPatient)
        {
            _registerPatient = registerPatient;
        }

        public Patient BuildPatient()
        {
            var patientIdentifier = new Identifier(FhirConst.IdentifierSystems.kNHSNumber, _registerPatient.SPINE_NHS_NUMBER);

            patientIdentifier.Extension.Add(new Extension
            {
                Url = FhirConst.StructureDefinitionSystems.kExtCcGpcNhsNumVerification,
                Value = new CodeableConcept(FhirConst.CodeSystems.kCcNhsNumVerification, "01", "Number present and verified")
            });

            var patientToRegister = new Patient
            {
                BirthDateElement = new Date(_registerPatient.DOB),
                Name = new List<HumanName>
                {
                    NameHelper.CreateOfficialName(_registerPatient.NAME_GIVEN, _registerPatient.NAME_FAMILY)
                },
                Identifier = new List<Identifier>
                {
                    patientIdentifier
                }
            };

			//SJD 31/07/2019 #269 meta.profile added to request payload
			var patientMeta = new Meta();
			{
				IEnumerable<string> MetaProfile = new string[] { FhirConst.StructureDefinitionSystems.kPatient };
				patientMeta.Profile = MetaProfile;
			}

			patientToRegister.Meta = patientMeta;

			switch (_registerPatient.GENDER)
            {
                case "MALE":
                    patientToRegister.Gender = AdministrativeGender.Male;
                    break;
                case "FEMALE":
                    patientToRegister.Gender = AdministrativeGender.Female;
                    break;
                case "OTHER":
                    patientToRegister.Gender = AdministrativeGender.Other;
                    break;
                case "UNKNOWN":
                    patientToRegister.Gender = AdministrativeGender.Unknown;
                    break;
            }

            return patientToRegister;
        }
    }
}
