# **NOT IN SCOPE**

[[_TOC_]]

# Service Handler

Land Registry is a Government department which administers the registration of Land within England and Wales.

We need to obtain from the Land Registry the:-

### Title
The title register usually includes:
*   The title number
*   Who owns the property
*   How much the property was last sold for
*   Whether the property has a mortgage
*   Details of any ‘restrictive covenants’ - promises to not do certain things with the land, like not building on a particular area
*   Details of any ‘easements’ - the rights of one piece of land over another, like a right of way

### Title Plan
The title plan includes the property’s location and general boundaries.

## Land Registry API
[HM Land Registry - API Catalogue](https://www.api.gov.uk/hmlr/#hm-land-registry)

## Interface
The Land Registry interface is XML SOAP based API, and secured by root certificates.

### Authentication
Authentication with the Land Registry is based on SSL Root Certificates. The certificate is supplied by the Land Registry in three parts the Root CA Certificate, Intermediary Cert, and Issuer public key. These three files need to be combined so you can generate a PFX string which can be stored in a Azure Key Vault.

Please See :- items 1,2 and 3 of the [Development Activities](./landregistry-service-handler/DevelopmentActivities.md) for more information.

## Request Process

### Sequence Diagram
![image](.attachments/ServiceSeq.png =750x)

### Land Registry Requests
#### Search By Property Description.
  * Request
    * Address 
      * A specific address that can be used for the delivery of physical mail.
    * BuildingName 
      * The name of the building or house on a street of this address
    * BuildingNumber 
      * The number of a building or house on a street of this address. Where the building or house occupies a range of numbers on the street (e.g. "1-9 Main St"), this will be the lower number on the range.
    * StreetName 
      * Name of a street or thoroughfare
    * CityName 
      * The name of the city, town or village of this address.
    * PostcodeZone 
      * The identifier for one or more properties according to the UK postal service; a group of letters and numbers added to the postal address to assist in the sorting of mail, as defined by the Royal Mail.
  * Response
    * Title Number
    * Tenure 

##### Request
![SearchByPropertyDescription](.attachments/Service/Req_SearchByPropertyDescription.png )
##### Response
![SearchByPropertyDescription](.attachments/Service/Res_SearchByPropertyDescription.png )


#### Official Search Title Known
* This includes requesting 
  * Official copy of the Register 
  * Title Plan
  * Certificate in form CI (OC1)
  * Other documents referred to in the Register that have not been designated exempt (OC2) associated with the title. 
* If the chosen method of delivery is 'electronic', and the document can be provided electronically then the requested document will be returned.
* If the chosen method of delivery is 'electronic', and the document **CANNOT** be provided electronically then the requested document will sent by post.

##### Request
![OfficialSearchTitleKnown](.attachments/Service/Req_OfficialSearchTitleKnown.png )
##### Response
![OfficialSearchTitleKnown](.attachments/Service/Res_OfficialSearchTitleKnown.png )

# [Development activities](./landregistry-service-handler/DevelopmentActivities.md)