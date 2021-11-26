﻿namespace GPConnect.Provider.AcceptanceTests.Http
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Hl7.Fhir.Model;

    public class FhirResponse
    {
        public Resource Resource { get; set; }

        public List<Bundle.EntryComponent> Entries => ((Bundle)Resource).Entry;

        public List<Patient> Patients => GetResources<Patient>();
        public List<Organization> Organizations => GetResources<Organization>();
        public List<Composition> Compositions => GetResources<Composition>();
        public List<Device> Devices => GetResources<Device>();
        public List<Practitioner> Practitioners => GetResources<Practitioner>();
        public List<Location> Locations => GetResources<Location>();
        public Bundle Bundle => (Bundle)Resource;
        public List<Slot> Slots => GetResources<Slot>();
        public List<Appointment> Appointments => GetResources<Appointment>();
        public List<Schedule> Schedules => GetResources<Schedule>();
        public List<CapabilityStatement> CapabilityStatements => GetResources<CapabilityStatement>();
        public List<AllergyIntolerance> AllergyIntolerances => GetResources<AllergyIntolerance>();
        public List<Medication> Medications => GetResources<Medication>();
        public List<MedicationStatement> MedicationStatements => GetResources<MedicationStatement>();
        public List<MedicationRequest> MedicationRequests => GetResources<MedicationRequest>();
        public List<List> Lists => GetResources<List>();
        public List<HealthcareService> HealthcareService => GetResources<HealthcareService>();

        private List<T> GetResources<T>() where T : Resource
        {
            //Need to consider cases where T isn't in ResourceTypeMap (and implementation!!)
            var type = typeof(T);

            if (Resource.ResourceType == ResourceType.Bundle)
            {
                return Entries
                    .Where(entry => entry.Resource.ResourceType.Equals(ResourceTypeMap[type]))
                    .Select(entry => (T)entry.Resource)
                    .ToList();
            }

            return new List<T>
            {
                (T)Resource
            };
        }

        private static Dictionary<Type, ResourceType> ResourceTypeMap => new Dictionary<Type, ResourceType>
        {
            {typeof(Patient), ResourceType.Patient},
            {typeof(Organization), ResourceType.Organization},
            {typeof(Composition), ResourceType.Composition},
            {typeof(Device), ResourceType.Device},
            {typeof(Practitioner), ResourceType.Practitioner},
            {typeof(Location), ResourceType.Location},
            {typeof(Slot), ResourceType.Slot},
            {typeof(Appointment), ResourceType.Appointment},
            {typeof(Schedule), ResourceType.Schedule},
            {typeof(CapabilityStatement), ResourceType.CapabilityStatement},
            {typeof(AllergyIntolerance), ResourceType.AllergyIntolerance},
            {typeof(List), ResourceType.List},
            {typeof(Medication), ResourceType.Medication},
            {typeof(MedicationStatement), ResourceType.MedicationStatement},
            {typeof(MedicationRequest), ResourceType.MedicationRequest},
            {typeof(HealthcareService), ResourceType.HealthcareService}
        };
    }
}
