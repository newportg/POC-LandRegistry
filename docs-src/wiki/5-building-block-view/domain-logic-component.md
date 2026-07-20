# Domain Logic Component

The domain logic will handle requests from both Service Bus and Ad-hoc via a RESTful API interface. Audit enquires will be routed to the audit component.

## Flowchart

![image](.attachments/ProcessFlow.png )

## Schemas and Maps

This service takes a standard integration approach to mapping internal and external data structures, this design breaks dependencies so that the external interface may change without affecting internal systems.

Objects received from interfaces should follow a standard VETER integration pattern (Validate, Extract, Transform, Enrich, Route)

* Validate
  * Objects should be validated and a default rules set should be applied to validate the object.
* Extract
  * Extract specific information from the message, such as audit etc.
* TransForm
  * The Message should be transformed into the next type in the process.
* Enrich
  * Adds additional data to the message, often from external sources
* Route
  * Directs the processed message to the appropriate destination based on certain criteria.

![image](.attachments/VETER.png )

### Request Schema

![image](.attachments/ServiceBusReq.png )

### Validation Rules

* Given : A Request
* When : ID is a GUID
* AND : Audit Is Valid
  * SourceApp is not Null
  * User is not Null
  * host is not Null
  * TimeStamp is valid today, current hour
* AND : Request is Valid
  * FixFloId is not Null, GUID
  * At least one 
    * Tenant
    * Landlord
  * Property Manager is not Null
  * StartDate is not Null
  * EndDate is not Null
    * EndDate is in the future
  * DueDate is Optional
    * DueDate is in the future
  * BranchName is not Null
  * additionaComments are Optional
* Then : I can send it to the third party

### Response Schema

![image](.attachments/ServiceBusRes.png )

### Response Message Type

* Provisional
  * Contacted\response is false
  * Contacted\inProgress is true
* Booked
  * dueDate is DateTime
  * Contacted\response is true
  * Contacted\inProgress is true
* RebookRequested
  * dueDate is Null
  * Contacted\response is false
  * Contacted\inProgress is false
* Cancelled
  * Reason is not Null 
  * proposedDate is Null
* Aborted
  * Reason is not Null 
  * proposedDate is Null
* Complete
  * ReportUrl is not Null
  * DocumentId is not Null


