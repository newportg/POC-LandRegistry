# 3, System Scope and Context

## Business Context

A internal user will have the ability to pass property information, to this service and receive a property information. 

![image](/.attachments/BusinessContext.png)

## Technical Context

![image](/.attachments/TechnicalContext.png)

* The API runs as a microservice on our cloud platform, and will be accessible via a API Management interface.
* The connection to the external service provider is over HTTPS.
  *	Authorisation is via a Secure Root Certificate published by the UK Govt. Land Registry
* The data passed in this application is sensitive and will be covered by GDPR controls.
* Any data retuned by the external provider will be stored, by our document management and scanned for viruses.
* A service bus interface will be available, for asynchronous queueing of jobs.
* A http interface will be available, for ad-hoc querying of information.
* On completion of any tenancy information stored by the third party will be deleted.

| Business Interface         | Channel          |
| -------------------------- | ---------------- |
| API for business functions | internet (https) |
| API for admin / audit      | internet (https) |
| Bulk Updates               | Service Bus      |
