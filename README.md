# gpconnect-provider-testing
GP Connect automated test suite for API providers

Please see the [GP Connect Provider Testing Wiki](https://github.com/nhsconnect/gpconnect-provider-testing/wiki) for further details of test scenarios and steps.

14/12/2016 - Added some self signed certificates for server, client, expired client certificate and a java keystore for the server

26/01/2017 - Added a patient NHS Number mapping file which will take an nhs number from the feature files and substitue it for one in the mapping file. This additional bit of functionality is to simplify testing of different provider systems.

 A CSV file has been added to the "Data" directory in the root folder of the project, called "NHSNoMap". In this file the Native NHS numbers used in the feature files can be mapped to test patients on the providers systems. If there is an NHS number is used in the feature file which does not have a mapping in the "NHSNoMap" file, the test suite will just use the NHS number from the feature file for the test.

 By default the application configuration does not use this mapping file and uses the NHS numbers in the feature files but if you with to use this feature you can change the "app.config" file variable "<add key="mapNativeNHSNoToProviderNHSNo" value="true" />" so that the value is true and this will cause the test suite to start trying to map the NHS numbers from the feature files to alternative provider NHS numbers defined in the file.
 
