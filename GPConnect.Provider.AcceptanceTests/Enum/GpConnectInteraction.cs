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

        LocationSearch = 8,
        LocationRead = 9,

        RegisterPatient = 10,

        GpcGetSchedule = 11,

        AppointmentCreate = 12,
        AppointmentSearch = 13,
        AppointmentAmend = 14,
        AppointmentCancel = 15,
        AppointmentRead = 16,

        MetadataRead = 17
    }
}
