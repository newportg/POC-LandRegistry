



**N.B.** Steps 1 and 2 are one-time activities and need to be performed only during initial setup. The certificates will have a validity period after which they will expire and new certificates will need to be procured from HM Land Registry and installed. If you havnt installed the certificates locally, then you will not be able to connect to the Business Gateway service.

## Generate Service Reference
You can generate a service reference using the visual studio IDE or by using the command line tool `dotnet-svcutil`.

**Unfortunately**, The Land registry does not provide the correct WSDL configuration so .Net Core tools cannot generate the service reference correctly.
Also, as the Internet Options configuration has been locked down by organisational policy, you may not be able to download the WSDL file using dotnet-svcutil.

So....

You will need to use a browser and download the WSDL file manually by navigating to the following URL:

https://bgtest.landregistry.gov.uk/b2b/BGStubService/ApplicationEnquiryV1_0PollRequestWebService?wsdl

Then download the individual XSD files that are referenced in the WSDL file, and save them to your local machine.

https://bgtest.landregistry.gov.uk/b2b/BGStubService/ApplicationEnquiryV1_0PollRequestWebService?xsd=PollRequest.xsd

https://bgtest.landregistry.gov.uk/b2b/BGStubService/ApplicationEnquiryV1_0PollRequestWebService?xsd=ResponseApplicationEnquiryV1_0.xsd

and then create a service reference by combining the WSDL file and the XSD files using the following command:

```bash 
dotnet-svcutil wsdl\ApplicationEnquiryV1_0PollRequestWebService.wsdl xsd\PollRequest.xsd xsd\ResponseApplicationEnquiryV1_0.xsd

```
This will generate a ServiceReference folder which will contain a References.cs file that you can include in your project, and a dotnet-svcutil.params.json file that contains the configuration for the service reference.

By running the folloeing command, the service reference can be regenerated in future without needing to download the WSDL and XSD files again: 
```bash
dotnet-svcutil .\dotnet-svcutil.params.json
```
# Batch download WSDL and XSD files

Download all the WSDL and XSD files from HM Land Registry, and store them in a folder within the service handler project.
```
\BusinessGateway
    \WSDL
    \XSD
    \ServiceReference
```

once downloaded, you can generate the service reference by running the following command from within the BusinessGateway folder:

```bash
dotnet-svcutil WSDL\*.wsdl \XSD\*.xsd
```
This will generate a single service reference that contains all the web services exposed by Business Gateway.

The download URLS for the WSDL and XSD files are:

## WSDL
* https://bgtest.landregistry.gov.uk/b2b/BGStubService/ApplicationEnquiryV1_0PollRequestWebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/BGStubService/ApplicationEnquiryV1_0WebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/BGStubService/BankruptcySearchV2_0PollRequestWebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/BGStubService/BankruptcySearchV2_0WebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/BGStubService/BankruptcySearchV2_1PollRequestWebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/BGStubService/BankruptcySearchV2_1WebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/BGStubService/DischargeActivityV1_0PollRequestWebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/BGStubService/DischargeActivityV1_0WebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/ECBG_StubService/EnquiryByPropertyDescriptionV2_0WebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/BGStubService/FullSearchV2_0PollRequestWebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/BGStubService/FullSearchV2_0WebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/BGStubService/FullSearchV2_1PollRequestWebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/BGStubService/FullSearchV2_1WebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/ECBG_StubService/OfficialCopyTitleKnownV2_1WebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/ECBG_StubService/OfficialCopyTitleKnownV2_0WebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/BGStubService/OfficialCopyWithSummaryV2_1WebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/BGStubService/OfficialSearchOfPartV2_0PollRequestWebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/BGStubService/OfficialSearchOfPartV2_0WebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/BGStubService/OfficialSearchOfPartV2_1PollRequestWebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/BGStubService/OfficialSearchOfPartV2_1WebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/ECBG_StubService/OfficialSearchV2_0PollRequestWebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/ECBG_StubService/OfficialSearchV2_0WebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/ECBG_StubService/OfficialSearchV2_1WebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/BGStubService/SearchOfIndexMapV2_0PollRequestWebService?wsdl
* https://bgtest.landregistry.gov.uk/b2b/BGStubService/SearchOfIndexMapV2_0WebService?wsdl

## XML Schema Definitions (XSD)
Where XSD's have been duplicated across multiple services, only one URL is listed, or needs to be downloaded.

### Requests
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/ApplicationEnquiryV1_0PollRequestWebService?   xsd=PollRequest.xsd
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/ApplicationEnquiryV1_0WebService?              xsd=RequestApplicationEnquiryV1_0.xsd
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/BankruptcySearchV2_0WebService?                xsd=RequestLandChargesBankruptcySearchV2_0.xsd
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/BankruptcySearchV2_1WebService?                xsd=RequestLandChargesBankruptcySearchV2_1.xsd
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/DischargeActivityV1_0WebService?               xsd=RequestDischargeActivityV1_0.xsd
  * https://bgtest.landregistry.gov.uk/b2b/ECBG_StubService/EnquiryByPropertyDescriptionV2_0WebService? xsd=RequestSearchByPropertyDescriptionV2_0.xsd
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/FullSearchV2_0WebService?                      xsd=RequestLandChargesFullSearchV2_0.xsd
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/FullSearchV2_1WebService?                      xsd=RequestLandChargesFullSearchV2_1.xsd
  * https://bgtest.landregistry.gov.uk/b2b/ECBG_StubService/OfficialCopyTitleKnownV2_1WebService?       xsd=RequestTitleKnownOfficialCopyV2_1.xsd
  * https://bgtest.landregistry.gov.uk/b2b/ECBG_StubService/OfficialCopyTitleKnownV2_0WebService?       xsd=RequestTitleKnownOfficialCopyV2_0.xsd
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/OfficialCopyWithSummaryV2_1WebService?         xsd=RequestOCWithSummaryV2_0.xsd
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/OfficialSearchOfPartV2_0WebService?            xsd=RequestOfficialSearchOfPartWithPriorityV2_0.xsd
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/OfficialSearchOfPartV2_1WebService?            xsd=RequestOfficialSearchOfPartWithPriorityV2_1.xsd
  * https://bgtest.landregistry.gov.uk/b2b/ECBG_StubService/OfficialSearchV2_0WebService?               xsd=RequestOfficialSearchOfWholeWithPriorityV2_0.xsd
  * https://bgtest.landregistry.gov.uk/b2b/ECBG_StubService/OfficialSearchV2_1WebService?               xsd=RequestOfficialSearchOfWholeWithPriorityV2_1.xsd
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/SearchOfIndexMapV2_0PollRequestWebService?     xsd=RequestSearchOfIndexMapV2_0.xsd
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/SearchOfIndexMapV2_0WebService?                xsd=RequestTitleKnownOfficialCopyV2_1.xsd

### Responses
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/ApplicationEnquiryV1_0WebService?              xsd=ResponseApplicationEnquiryV1_0.xsd
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/BankruptcySearchV2_0PollRequestWebService?     xsd=ResponseLandChargesBankruptcySearchV2_0.xsd
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/DischargeActivityV1_0WebService?               xsd=ResponseDischargeActivityV1_0.xsd
  * https://bgtest.landregistry.gov.uk/b2b/ECBG_StubService/EnquiryByPropertyDescriptionV2_0WebService? xsd=ResponseSearchByPropertyDescriptionV2_0.xsd
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/FullSearchV2_1WebService?                      xsd=ResponseLandChargesFullSearchV2_0.xsd
  * https://bgtest.landregistry.gov.uk/b2b/ECBG_StubService/OfficialCopyTitleKnownV2_0WebService?       xsd=ResponseTitleKnownOfficialCopyV2_0.xsd
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/OfficialCopyWithSummaryV2_1WebService?         xsd=ResponseOCWithSummaryV2_1.xsd
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/OfficialSearchOfPartV2_1WebService?            xsd=ResponseOfficialSearchOfPartWithPriorityV2_0.xsd
  * https://bgtest.landregistry.gov.uk/b2b/ECBG_StubService/OfficialSearchV2_1WebService?               xsd=ResponseOfficialSearchOfWholeWithPriorityV2_0.xsd
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/SearchOfIndexMapV2_0PollRequestWebService?     xsd=ResponseSearchOfIndexMapV2_0.xsd
  * https://bgtest.landregistry.gov.uk/b2b/BGStubService/SearchOfIndexMapV2_0WebService?                xsd=ResponseTitleKnownOfficialCopyV2_0.xsd





