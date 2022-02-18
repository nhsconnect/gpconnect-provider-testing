<img src="logo.png" height=72>

# **GP Connect Provider Automated Test Suite**

This project provides a test suite to test server implementations of the [GP Connect](https://nhsconnect.github.io/gpconnect/) specification.

Please see the [GP Connect Provider Testing Wiki](https://github.com/nhsconnect/gpconnect-provider-testing/wiki) for further details of test scenarios and steps.

## Setup
The tests use a specific set of patients (numbered 1-15 e.g. patient1, patient2...) when making calls to endpoints. In order for these tests to pass, the data held on the target server must match the expected conditions defined in the patients file:
```
{projectRoot}\Data\Test Suite Prerequisites.xlsx
```

Each of these numbered patients will have a corresponding NHS number on the target server. The following file should be used to map the patient number to their equivalent NHS number:
```
{projectRoot}\Data\NHSNoMap.csv
```

You will need to check out the [GP Connect FHIR](https://github.com/nhsconnect/gpconnect-fhir) project.

## Configuration
The project has a configuration file containing attributes you'll need to change:
```
{projectRoot}\GPConnect.Provider.AcceptanceTests\App.config
```
In here, there are a number of attribues you'll need to change:
* dataDirectory - {projectRoot}\Data
* fhirDirectory - \<path to checked out GP Connect FHIR project\>

By default, the test suite points to the public [Demonstrator](http://ec2-54-194-109-184.eu-west-1.compute.amazonaws.com) (code available [here](https://github.com/nhs-digital/gpconnect).) To change this, modify the following properties:

For Foundation/Appointments Tests
* useTLSFoundationsAndAppmts
* serverUrlFoundationsAndAppmts
* serverHttpsPortFoundationsAndAppmts
* serverHttpPortFoundationsAndAppmts
* serverBaseFoundationsAndAppmts

For Structured Tests
* useTLSStructured
* serverUrlStructured
* serverHttpsPortStructured
* serverHttpPortStructured
* serverBaseStructured

## Running in Visual Studio ( Must be at least Visual Studio 2017)
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

# External Reporting
The test suite is also able to send JSON test results to a configurable provider system endpoint. 

#### Notes
The current implementation:
- will either send no requests or a request after **every** scenario.
- will **not** send requests for scenarios tagged with **@ignore** 

#### Configuration

As above, the project has a configuration file containing attributes you'll need to change, found at:
```
{projectRoot}\GPConnect.Provider.AcceptanceTests\App.config
```
The attributes used to configure this functionality are:

```xml
<add key="Reporting:Enabled" value="true"/>
<add key="Reporting:BaseUrl" value="localhost"/>
<add key="Reporting:Endpoint" value="/api/result"/> 
<add key="Reporting:Port" value="14947"/>
<add key="Reporting:Tls" value="false"/>
```
These values are used to construct the URL to which a HTTP POST request will be sent.

In the example above, the request would be sent to:
```
http://localhost:14947/api/result
```
#### JSON Body
The structure of the JSON object sent in the request body is:

```javascript
{
  "TestRunId": "string", //TestRunId is a System.Guid 
  "ScenarioName": "string",
  "ScenarioOutcome": "string",
  "ErrorMessage": "string",
  "HttpRequest": {}, //HttpRequest is a HttpRequestConfiguration object
  "HttpResponse": {} //HttpResponse is a HttpResponse object
}
```
For example:

```javascript
{
    "TestRunId": "dc312935-dbf6-45ba-ae64-0ba91482153f",
    "ScenarioName": "Location Read with invalid resource path in URL",
    "ScenarioOutcome": "Pass",
    "ErrorMessage": null,
    "HttpRequest": {
        "DecompressionMethod": 0,
        "UseTls": true,
        "Protocol": "https://",
        "UseWebProxy": false,
        "WebProxyUrl": "localhost",
        "WebProxyPort": "8889",
        "WebProxyAddress": "https://localhost:8889",
        "UseSpineProxy": false,
        "SpineProxyUrl": "msg.dev.spine2.ncrs.nhs.uk",
        "SpineProxyPort": "443",
        "SpineProxyAddress": "https://msg.dev.spine2.ncrs.nhs.uk:443",
        "RequestMethod": null,
        "RequestUrl": "Location!/1",
        "RequestContentType": "application/json+fhir",
        "RequestBody": null,
        "ConsumerASID": "200000000359",
        "ProviderASID": "200000000359",
        "FhirServerUrl": "localhost",
        "FhirServerPort": "19192",
        "FhirServerFhirBase": "/fhir",
        "ProviderAddress": "https://localhost:19192/fhir",
        "EndpointAddress": "https://localhost:19192/fhir",
        "BaseUrl": "https://localhost:19192/fhir/",
        "RequestHeaders": {},
        "RequestParameters": {},
        "FhirServerHttpPort": "19191",
        "FhirServerHttpsPort": "19192",
        "BodyParameters": {
            "Parameter": [],
            "IdElement": null,
            "Meta": null,
            "ImplicitRulesElement": null,
            "LanguageElement": null
        },
        "HttpMethod": {
            "Method": "GET"
        },
        "GetRequestId": "1",
        "GetRequestVersionId": null
    },
    "HttpResponse": {
        "StatusCode": 404,
        "ContentType": "application/json+fhir",
        "Headers": {
            "Pragma": "no-cache",
            "Transfer-Encoding": "chunked",
            "Cache-Control": "no-store, no-cache",
            "Date": "Thu, 14 Sep 2017 14:00:29 GMT",
            "X-Powered-By": "HAPI FHIR 2.5 REST Server (FHIR Server; FHIR 1.0.2/DSTU2)",
            "Content-Type": "application/json+fhir; charset=UTF-8",
            "Expires": "0"
        },
        "ResponseJSON": {
            "resourceType": "OperationOutcome",
            "meta": {
                "profile": [
                    "http://fhir.nhs.net/StructureDefinition/gpconnect-operationoutcome-1"
                ]
            },
            "issue": [
                {
                    "severity": "error",
                    "code": "invalid",
                    "details": {
                        "coding": [
                            {
                                "system": "http://fhir.nhs.net/ValueSet/gpconnect-error-or-warning-code-1",
                                "code": "REFERENCE_NOT_FOUND",
                                "display": "REFERENCE_NOT_FOUND"
                            }
                        ],
                        "text": "Request containts invalid resource (/fhir/Location!/1)"
                    }
                }
            ]
        },
        "ResponseXML": null,
        "ResponseTimeInMilliseconds": 1503,
        "ResponseTimeAcceptable": false,
        "CurlCode": 0,
        "Redirected": false,
        "ConnectionClosed": false
    }
}
```
