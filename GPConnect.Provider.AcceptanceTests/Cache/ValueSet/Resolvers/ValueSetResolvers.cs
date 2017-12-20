namespace GPConnect.Provider.AcceptanceTests.Cache.ValueSet.Resolvers
{
    using System;
    using System.Collections.Generic;
    using Helpers;
    using Hl7.Fhir.Rest;
    using Hl7.Fhir.Specification.Source;

    internal static class ValueSetResolvers
    {
        internal static MultiResolver GetResolver()
        {
            var resolvers = new List<IResourceResolver>();

            if (AppSettingsHelper.FhirCheckDisk)
                resolvers.Add(GetDirectorySource());

            if (AppSettingsHelper.FhirCheckWeb)
                resolvers.Add(GetWebResolver());

            if (AppSettingsHelper.FhirCheckWebFirst)
                resolvers.Reverse();

            return new MultiResolver(resolvers);
        }

        internal static WebResolver GetWebResolver()
        {
            return new WebResolver(GetFhirClientFactory());
        }

        internal static DirectorySource GetDirectorySource()
        {
            var directory = @"C:\Development\gpconnect-provider-testing\FHIR";
            return new DirectorySource(directory, true);
        }

        private static Func<Uri, FhirClient> GetFhirClientFactory()
        {
            return uri =>
            {
                var client = new FhirClient(uri)
                {
                    PreferredFormat = ResourceFormat.Json,
                };

                return client;
            };
        }
    }
}
