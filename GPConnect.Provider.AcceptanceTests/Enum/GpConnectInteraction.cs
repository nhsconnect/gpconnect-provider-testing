namespace GPConnect.Provider.AcceptanceTests.Enum
{
    public enum GpConnectInteraction
    {
        GpcGetCareRecord = 1,

        OrganizationSearch = 2,
        OrganizationRead = 3,

        PractitionerSearch = 4,
        PractitionerRead = 5,

        PatientSearch = 6,
        PatientRead = 7,

        LocationRead = 9,

        RegisterPatient = 10,

        SearchForFreeSlots = 11,

        AppointmentCreate = 12,
        AppointmentSearch = 13,
        AppointmentAmend = 14,
        AppointmentCancel = 15,
        AppointmentRead = 16,

        MetadataRead = 17,

        GpcGetStructuredRecord = 18,

		StructuredMetaDataRead = 19,

        DocumentsMetaDataRead = 20,
        DocumentsPatientSearch = 21,
        DocumentsSearch = 22,
        DocumentsRetrieve = 23,
        MigrateStructuredRecordWithoutSensitive = 24,
        MigrateStructuredRecordWithSensitive = 25,
        MigrateDocument = 26


    }
}
