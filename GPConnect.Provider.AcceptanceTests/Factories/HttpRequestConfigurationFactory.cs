namespace GPConnect.Provider.AcceptanceTests.Factories
{
    using System.Net.Http;
    using Constants;
    using Enum;
    using Http;

    public class HttpRequestConfigurationFactory
    {
        private readonly GpConnectInteraction _gpConnectInteraction;
        private static HttpRequestConfiguration _httpRequestConfiguration;

        public HttpRequestConfigurationFactory(GpConnectInteraction gpConnectInteraction, HttpRequestConfiguration httpRequestConfiguration)
        {
            _gpConnectInteraction = gpConnectInteraction;
            _httpRequestConfiguration = httpRequestConfiguration;
            _httpRequestConfiguration.LoadAppConfig();
            _httpRequestConfiguration.SetDefaultHeaders();
        }

        public HttpRequestConfiguration GetHttpRequestConfiguration()
        {
            switch (_gpConnectInteraction)
            {
                case GpConnectInteraction.GpcGetCareRecord:
                    return GetCareRecordConfiguration();
                case GpConnectInteraction.GpcGetStructuredRecord:
                    return GetStructuredRecordConfiguration();
                case GpConnectInteraction.OrganizationSearch:
                    return OrganizationSearchConfiguration();
                case GpConnectInteraction.OrganizationRead:
                    return OrganizationReadConfiguration();
                case GpConnectInteraction.PractitionerSearch:
                    return PractitionerSearchConfiguration();
                case GpConnectInteraction.PractitionerRead:
                    return PractitionerReadConfiguration();
                case GpConnectInteraction.PatientSearch:
                    return PatientSearchConfiguration();
                case GpConnectInteraction.PatientRead:
                    return PatientReadConfiguration();
                case GpConnectInteraction.LocationRead:
                    return LocationReadConfiguration();
                case GpConnectInteraction.RegisterPatient:
                    return RegisterPatientConfiguration();
                case GpConnectInteraction.SearchForFreeSlots:
                    return SearchForFreeSlotsConfiguration();
                case GpConnectInteraction.AppointmentCreate:
                    return AppointmentCreateConfiguration();
                case GpConnectInteraction.AppointmentSearch:
                    return AppointmentSearchConfiguration();
                case GpConnectInteraction.AppointmentAmend:
                    return AppointmentAmendConfiguration();
                case GpConnectInteraction.AppointmentCancel:
                    return AppointmentCancelConfiguration();
                case GpConnectInteraction.AppointmentRead:
                    return AppointmentReadConfiguration();
                case GpConnectInteraction.MetadataRead:
                    return MetadataReadConfiguration();
				case GpConnectInteraction.StructuredMetaDataRead:
					return StructuredMetaDataReadConfiguration();
                case GpConnectInteraction.DocumentsMetaDataRead:
                    return DocumentsMetaDataReadConfiguration();
                case GpConnectInteraction.DocumentsSearch:
                    return DocumentsSearchConfiguration();
                case GpConnectInteraction.DocumentsPatientSearch:
                    return DocumentsPatientSearchConfiguration();
                default:
                    return _httpRequestConfiguration;
            }
        }

        private static HttpRequestConfiguration GetCareRecordConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Post;
            _httpRequestConfiguration.RequestUrl = "Patient/$gpc.getcarerecord";
            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.GpcGetCareRecord);
            
            return _httpRequestConfiguration;
        }

        private static HttpRequestConfiguration GetStructuredRecordConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Post;
            _httpRequestConfiguration.RequestUrl = "Patient/$gpc.getstructuredrecord";
            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.GpcGetStructuredRecord);

            return _httpRequestConfiguration;
        }


        private static HttpRequestConfiguration OrganizationSearchConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Get;
            _httpRequestConfiguration.RequestUrl = "Organization";
            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.OrganizationSearch);

            return _httpRequestConfiguration;
        }

        private static HttpRequestConfiguration OrganizationReadConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Get;
            _httpRequestConfiguration.RequestUrl = "Organization/" + _httpRequestConfiguration.GetRequestId;

            if (!string.IsNullOrEmpty(_httpRequestConfiguration.GetRequestVersionId))
            {
                _httpRequestConfiguration.RequestUrl = _httpRequestConfiguration.RequestUrl + "/_history/" + _httpRequestConfiguration.GetRequestVersionId;
            }

            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.OrganizationRead);

            return _httpRequestConfiguration;
        }

        private static HttpRequestConfiguration PractitionerSearchConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Get;
            _httpRequestConfiguration.RequestUrl = "Practitioner";
            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.PractitionerSearch);

            return _httpRequestConfiguration;
        }

        private static HttpRequestConfiguration PractitionerReadConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Get;
            _httpRequestConfiguration.RequestUrl = "Practitioner/" + _httpRequestConfiguration.GetRequestId;

            if (!string.IsNullOrEmpty(_httpRequestConfiguration.GetRequestVersionId))
            {
                _httpRequestConfiguration.RequestUrl = _httpRequestConfiguration.RequestUrl + "/_history/" + _httpRequestConfiguration.GetRequestVersionId;
            }

            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.PractitionerRead);

            return _httpRequestConfiguration;
        }

        private static HttpRequestConfiguration PatientSearchConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Get;
            _httpRequestConfiguration.RequestUrl = "Patient";
            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.PatientSearch);

            return _httpRequestConfiguration;
        }

        private static HttpRequestConfiguration PatientReadConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Get;
            _httpRequestConfiguration.RequestUrl = "Patient/" + _httpRequestConfiguration.GetRequestId;

            if (!string.IsNullOrEmpty(_httpRequestConfiguration.GetRequestVersionId))
            {
                _httpRequestConfiguration.RequestUrl = _httpRequestConfiguration.RequestUrl + "/_history/" + _httpRequestConfiguration.GetRequestVersionId;
            }

            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.PatientRead);

            return _httpRequestConfiguration;
        }

        private static HttpRequestConfiguration LocationReadConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Get;
            _httpRequestConfiguration.RequestUrl = "Location/" + _httpRequestConfiguration.GetRequestId;
            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.LocationRead);

            return _httpRequestConfiguration;
        }

        private static HttpRequestConfiguration RegisterPatientConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Post;
            _httpRequestConfiguration.RequestUrl = "Patient/$gpc.registerpatient";
            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.RegisterPatient);

            return _httpRequestConfiguration;
        }

        private static HttpRequestConfiguration SearchForFreeSlotsConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Get;
            _httpRequestConfiguration.RequestUrl = "Slot";
            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.SlotSearch);

            return _httpRequestConfiguration;
        }

        private static HttpRequestConfiguration AppointmentCreateConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Post;
            _httpRequestConfiguration.RequestUrl = "Appointment";
            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.AppointmentCreate);

            return _httpRequestConfiguration;
        }

        private static HttpRequestConfiguration AppointmentSearchConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Get;
            _httpRequestConfiguration.RequestUrl = "Patient/" + _httpRequestConfiguration.GetRequestId + "/Appointment";
            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.AppointmentSearch);

            return _httpRequestConfiguration;
        }

        private static HttpRequestConfiguration AppointmentAmendConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Put;
            _httpRequestConfiguration.RequestUrl = "Appointment/" + _httpRequestConfiguration.GetRequestId;
            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.AppointmentAmend);

            return _httpRequestConfiguration;
        }

        private static HttpRequestConfiguration AppointmentCancelConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Put;
            _httpRequestConfiguration.RequestUrl = "Appointment/" + _httpRequestConfiguration.GetRequestId;
            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.AppointmentCancel);

            return _httpRequestConfiguration;
        }

        private static HttpRequestConfiguration AppointmentReadConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Get;
            _httpRequestConfiguration.RequestUrl = "Appointment/" + _httpRequestConfiguration.GetRequestId;
            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.AppointmentRead);

            return _httpRequestConfiguration;
        }

        private static HttpRequestConfiguration MetadataReadConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Get;
            _httpRequestConfiguration.RequestUrl = "metadata";
            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.MetadataRead);

            return _httpRequestConfiguration;
        }

		//SJD added for 1.2.6 
		private static HttpRequestConfiguration StructuredMetaDataReadConfiguration()
		{
			_httpRequestConfiguration.HttpMethod = HttpMethod.Get;
            _httpRequestConfiguration.RequestUrl = "metadata";

			_httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.StructuredMetaDataRead);

			return _httpRequestConfiguration;
		}

        private static HttpRequestConfiguration DocumentsMetaDataReadConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Get;
            _httpRequestConfiguration.RequestUrl = "metadata";

            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.DocumentsMetaDataRead);

            return _httpRequestConfiguration;
        }

        private static HttpRequestConfiguration DocumentsSearchConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Get;
            _httpRequestConfiguration.RequestUrl = "Patient/" + _httpRequestConfiguration.GetRequestId + "/DocumentReference";
            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.DocumentsSearch);

            return _httpRequestConfiguration;
        }


        private static HttpRequestConfiguration DocumentsPatientSearchConfiguration()
        {
            _httpRequestConfiguration.HttpMethod = HttpMethod.Get;
            _httpRequestConfiguration.RequestUrl = "Patient";
            _httpRequestConfiguration.RequestHeaders.ReplaceHeader(HttpConst.Headers.kSspInteractionId, SpineConst.InteractionIds.DocumentsPatientSearch);

            return _httpRequestConfiguration;
        }

    }
}
