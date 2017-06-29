namespace GPConnect.Provider.AcceptanceTests.Enum
{
    public enum GpConnectInteraction
    {
        GpcGetCareRecord = 1,

        OrganizationSearch = 2,
        OrganizationRead = 3,

        PractitionerSearch = 4,
        PractitionerRead = 5,

        LocationSearch = 6,
        LocationRead = 7,

        RegisterPatient = 8,

        GpcGetSchedule = 9,

        AppointmentCreate = 10,
        AppointmentRetrieve = 11,
        AppointmentAmend = 12,
        AppointmentCancel = 13,
        AppointmentRead = 14
    }
}
