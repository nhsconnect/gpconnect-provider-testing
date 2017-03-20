# gpconnect-provider-testing
This project provides a test suite to test server implementations of the [GP Connect](https://nhsconnect.github.io/gpconnect/) specification.

Please see the [GP Connect Provider Testing Wiki](https://github.com/nhsconnect/gpconnect-provider-testing/wiki) for further details of test scenarios and steps.

## Setup
The tests use a specific set of patients (numbered 1-15 e.g. patient1, patient2...) when making calls to endpoints. In order for these tests to pass, the data held on the target server must match the expected conditions defined in the patients file:
```
{projectRoot}\Data\Test Suite Required Patients.xlsx
```

Each of these numbered patients will have a corresponding NHS number on the target server. The following file should be used to map the patient number to their equivalent NHS number:
```
{projectRoot}\Data\NHSNoMap.csv
```

You will need to check out the [GP Connect FHIR](https://github.com/nhsconnect/gpconnect-fhir) project.

## Config
The project has a configuration file containing attributes you'll need to change:
```
{projectRoot}\GPConnect.Provider.AcceptanceTests\App.config
```
In here, there are a number of attribues you'll need to change:
* dataDirectory - {projectRoot}\Data
* fhirDirectory - \<path to checked out GP Connect FHIR project\>

By default, the test suite points to the public [Demonstrator](http://ec2-54-194-109-184.eu-west-1.compute.amazonaws.com) (code available [here](https://github.com/nhs-digital/gpconnect).) To change this, modify the following properties:
* useTLS
* serverUrl
* serverPort

## Running in Visual Studio
Note: *This assumes the project has already been checked out and the appropriate modifications described in the Setup and Config sections have already been made.*

Install 2 plugins:
* SpecFlow
* NUnit 3 Test Adapter

Open the project then open the Test Explorer Window (Test -> Windows -> Test Explorer). Here you should have to option to Run All tests.

## Running on command line
Note: *This assumes the project has already been checked out and the appropriate modifications described in the Setup and Config sections have already been made.*

Run the following commands:
```sh
$ cd {projectRoot}
$ nuget restore
$ msbuild
$ nunit3-console "GPConnect.Provider.AcceptanceTests\bin\Debug\GPConnect.Provider.AcceptanceTests.dll" --result=TestResult.xml;format=nunit2
```
This will generate a TestResult.xml file containing the test results.