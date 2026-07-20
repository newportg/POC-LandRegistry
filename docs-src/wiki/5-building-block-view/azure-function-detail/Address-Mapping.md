
# IGNORE

[[_TOC_]]

# Standards 
The ISO standard is a implementation of the UPU S42a-6 standard
* [ISO](https://www.iso.org/obp/ui/#iso:std:iso-iec:19773:ed-1:v1:en:term:3.16.1.19)
* [UPU S42a-6](https://www.upu.int/UPU/media/upu/publications/manualAddressingAddressingAndPostcodeManualEn.pdf)
* [United Kingdom of Great Britain and Northern Ireland](https://www.upu.int/UPU/media/upu/PostalEntitiesFiles/addressingUnit/gbrEn.pdf)
* [France](https://www.upu.int/UPU/media/upu/PostalEntitiesFiles/addressingUnit/fraEn.pdf)

Postal Address Template Description Language (PATDL)

![image.png](/.attachments/image-8e3e036a-7e7f-46e1-8f38-12acbcadd7ba.png)

## UK Address Format, as described by the UPU

|United Kingdom GBR, GB ||
|--|--|
|Line 1 | Organization 
|Line 2 | Department |
|Line 3 | PostBox |
|Line 4 | SubBuilding Building |
|Line 5 | Premise Thoroughfare |
|Line 6 | DoubleDependentLocality |
|Line 7 | DependentLocality |
|Line 8 | Locality |
|Line 9 | PostalCode|

# ISO 19160 or ISO 20022 ?
ISO cannot make up there mind.

ISO 20022 and ISO 19160 (UPU S42) serve different, though related, purposes: ISO 20022 defines a standardised, structured data format for financial messaging, including specific, granular fields for address components, primarily for efficiency and accuracy in financial transactions. In contrast, UPU S42 is a global postal addressing standard that focuses on the logical structure of postal addresses and provides country-specific templates to guide mailers and postal systems in accurately formatting addresses for efficient cross-border mail processing.

Key Differences 

*   **Scope:** 
    ISO 20022 is a financial messaging standard, while UPU S42 is a postal addressing standard. 
*   **Focus:** 
    ISO 20022 emphasises structured data for financial transactions, whereas S42 provides a flexible framework to format addresses for mail delivery. 
*   **Application:** 
    ISO 20022 is critical for financial operations and regulatory compliance, while UPU S42 supports postal operations and communication.

## ISO 19160 - UPU S42
*   **Purpose:** 
  To provide a universal framework for postal addressing that can be adapted by any country to standardise the format of their national addresses, thereby streamlining cross-border mail. 
*   **Structure:** 
  Consists of a generic list of address elements and country-specific templates that guide users on how to map these elements into a correctly formatted address. 
*   **Key Feature:** 
  Aims for easy adaptation by web applications and promotes efficient mail processing and data verification at national and international levels. 
*   **Context:** 
   Developed by the Universal Postal Union (UPU) for postal services to improve mail delivery and address management.

## ISO 20022
*   **Purpose:** 
    To standardise the capture and exchange of financial transaction data, ensuring consistency and reducing errors in payments and other financial services. 
*   **Structure:** 
    Uses a highly structured format with discrete data elements (like street name, postal code, town name) defined within XML tags. 
*   **Key Feature:** 
    Breaks down addresses into granular, machine-readable components, facilitating automated processing, data quality improvements, and compliance with regulations. 
*   **Evolution:** 
    The standard has evolved, with the 2019 version (PostalAddress24) offering a more detailed 15-field structure compared to the 2009 version (PostalAddress6). 
*   **Context:** 
    Primarily used by financial institutions and payment systems (e.g., SWIFT) for cross-border and high-value payments.
* **Important : The ISO 20022 standard currently only applies to 32 countries.**

### Key ISO 20022 Address Schema Elements

* **Department**: (<Dept>): For specific business units or departments. 
* **Sub Department**: (<SubDept>): The sub department name.
* **Street Name**: (<StrtNm>): The name of the street. 
* **Building Number**: (<BldgNb>): The number of the building. 
* **Building Name**: (<BldgNm>): The name of the building.
* **Floor**: (<Flr>) : The building floor.
* **Post Box Number**: <PstBx>) : The PostBox.
* **Room**: (<Room>) : Room number or name.
* **Postal Code**: (<PstCd>): The postal code for the address. 
* **Town Name**: (<TwnNm>): The name of the city or town. 
* **Town Location Name**: (<TwnLctnNm>): A specific location within a town, like a district. 
* **District Name**: (<DstrctNm>): The name of a larger district within a country. 
* **Country Subdivision**: (<CtrySubDvsn>): A state, province, or other governmental subdivision. 
* **Country**: (<Ctry>): A two-character ISO country code (e.g., US, GB). 
* **Address Line**: (<AdrLine>): An optional line for additional address information.

### References
* [ISO 20022: Understanding the Impact for Global Address Data | GB](https://www.loqate.com/en-gb/blog/iso-20022/)
* [ISO-20022-address-format-guide](https://www.postgrid.com/iso-20022-address-format-guide)
* [Structured Postal Address](https://www.swift.com/swift-resource/252113/download)
* [Best Practices for Storing Addresses : The Essential Guide](https://www.geopostcodes.com/blog/best-practices-storing-addresses/)

## Mapping between the two standards ISO 20022 / ISO 19160 (UPU S42)

| **ISO 20022 Element** | **UPU S42 Equivalent** | **Notes**|
|----|----|----|
| `<Dept>` (Department)                  | `Organisation Name` | |
| `<SubDept>` (Sub-Department)           | `Department Name`   | |
| `<StrtNm>` (Street Name)               | `Thoroughfare Name` | |
| `<BldgNb>` (Building Number)           | `Building Number`   | |
| `<BldgNm>` (Building Name)             | `Building Name`     | |
| `<Flr>` (Floor)                        | `Sub Building Name` | |
| `<PstBx>` (Post Box)                   | `PO Box`            | |
| `<Room>` (Room)                        | Not always present  | May be part of extended S42 templates|
| `<PstCd>` (Post Code)                  | `Postcode`          | |
| `<TwnNm>` (Town Name)                  | `Locality`          | |
| `<DstrctNm>` (District Name)           | `DependentLocality` | |
| `<CtrySubDvsn>` (Country Sub Division) | `AdministrativeArea`| |
| `<Ctry>` (Country Code)                | `Country Name`      | ISO 3166 code vs full name|
| `<AdrLine>` (Unstructured Line)        | Not directly mapped | S42 discourages unstructured formats|



# Mapping

**ISO/UPU - UK Address - HUB**

The examples are data mapped from the Loqate Capture API and applied to the ISO/UPU format.


| **Element** | **Field name** | **Description** | **Length**| **UK Address Format \ Loqate Capture** | **HUB Field** | **Example1**|**Example2**|**Example3**|
|----|----|----|----|----|----|----|----|----|
| Organisation | Organisation Name                 |                | 60| Organization          ||||Hochtief Uk Construction Ltd|
|              | Department Name                   |                | 60| Department            |||||
| Premises     | Sub Building Name                 |                | 30| SubBuilding           |Line1||Flat 409|2nd Floor|
|              | Building Name                     |                | 50| Building              |PropertyName||Caraway Apartments|Whitehill House|
|              | Building Number                   |                | 4 | Premise               |Line1/PropertyNumber|15|2|8|
| Thoroughfare | Dependent Thoroughfare Name       |                | 60| DependentThoroughfare |||| Windmill Hill Business Park
|              | Dependent Thoroughfare Descriptor |                | 20|                       |||||
|              | Thoroughfare Name                 | Street         | 60| Thoroughfare          |Line2|The St|Cayenne Court|Whitehill Way|
|              | Thoroughfare Descriptor           |                | 20|                       |||||
| Locality     | Double Dependent Locality         | Small villages | 35|                       |||||
|              | Dependent Locality                |                | 35| DependentLocality     |Line3|Hurn|||
|              | Post town                         |                | 30| Locality              |City|Christchurch|London|Swindon|
| Postcode     | Postcode                          |                | 7| PostalCode             |Postcode|BH23 6AA|SE1 2PP|SN5 6NX|
| PO Box       | PO Box                            |                | 6| PostBox                |||||
|              |                                   |                |  | AdministrativeArea     |County | Dorset | London|Wiltshire|
| Country Name |                                   |                |  | CountryName            | |United Kingdom |United Kingdom |United Kingdom | 
|              |                                   |                |  | ISO3166-2              | | GB| GB| GB| 
|              |                                   |                |  | ISO3166-3              | | GBR| GBR| GBR|
|              |                                   |                |  | ISO3166-N              | | 826| 826| 826|

## What is ?
### Dependent Thoroughfare Name
A Dependent Thoroughfare Name (DTN) is a secondary street or parade name used in addresses, such as "KINGS PARADE, HIGH STREET," to uniquely identify a delivery point when the primary thoroughfare name is ambiguous within the same Post Town. It provides further detail beyond the main street name to ensure a specific address is correctly located for postal and delivery services, distinguishing between different smaller streets or areas within a larger named road.

### Dependent Thoroughfare Descriptor
A Dependent Thoroughfare Descriptor is the descriptive suffix, like "Mews" or "Court," used to identify a specific delivery point when a standard thoroughfare name (street name) is repeated within the same Post Town in a UK address. It works in conjunction with a Dependent Thoroughfare Name to form a complete address, clarifying the delivery point's location when a simple street name isn't enough.

**N.B.** Not used by Loqate

### Thoroughfare Name
A thoroughfare name is the specific, descriptive name of a street or road, excluding its type (such as "Street" or "Road"), used to identify a delivery point in an address. For example, in the address "123 High Street," the thoroughfare name is "High" and the street type is "Street". This component is crucial for uniquely identifying a location, especially when combined with a house number or name and postcode, to ensure accurate mail delivery and navigation.

### Thoroughfare Descriptor
A Thoroughfare Descriptor is the word that defines the type of street or road, such as "Road," "Street," "Lane," or "Court". It distinguishes the nature of the passageway on which the property is located, clarifying the specific type of Thoroughfare when the street name alone might be ambiguous or when a further specific identifier is needed.

**N.B.** Not used by Loqate

### Double Dependent Locality?

A Double Dependent Locality is a specific location within a village or larger area (a Dependent Locality) that itself is contained within a larger postal area (the Post Town). It functions to distinguish between similar addresses within the same postcode area by providing a third layer of location information. 

Example of a UK Address with a Double Dependent Locality

Here's an example of how a Double Dependent Locality would appear in a UK address: 

*   **1 Upper Littleton**: (Building/House number + Double Dependent Locality)
*   **Winford**: (Dependent Locality)
*   **BRISTOL**: (Post Town)
*   **BS18 8HF**: (Postcode)
*   **UNITED KINGDOM**: (Country)

In this address:

*   "BRISTOL" is the Post Town, the main postal area. 
*   "Winford" is the Dependent Locality, a village or smaller area within Bristol. 
*   "Upper Littleton" is the Double Dependent Locality, indicating a more specific part of Winford or a distinction from another "Littleton" or street within Winford.

**N.B.** This would be received from Loqate as a Thoroughfare(Upper) and a DependentLocality(Littleton)

### Dependent Locality
In the UK postcode system, a Dependent Locality is a small town or village name included in an address to specify the location when the delivery point is outside the boundaries of the main Post Town that serves it, or to differentiate between addresses with the same street name in different areas of a Post Town. It acts as a geographical subdivision, making the address more precise for mail sorting and delivery, particularly in cases of duplicate street names within the same Post Town.

ISO/UPU - Why no Country Code.
The ISO/UPU  

# Loqate
Key | UM22-TE99-HT99-UB71

https://api.addressy.com/Cleansing/International/Batch/v1.10/json6.ws
```
curl --request POST \
  --url https://api.addressy.com/Cleansing/International/Batch/v1.10/json6.ws \
  --header 'Content-Type: application/json' \
  --data '{
  "Key": "UM22-TE99-HT99-UB71",
  "GeoCode": false,
  "Options": {
    "Process": "Verify",
    "Enhance": false
  },
  "Addresses": [
    {
      "Department": "",
      "PostalCode": "tf26rf",
      "Country": "United Kingdom",
      "Address": "93 teagues crescent"
    }
  ]
}
```
```
{  
"Address": "93 Teagues Crescent<br>Trench<br>Telford<br>TF2 6RF",  
"Address1": "93 Teagues Crescent",  
"Address2": "Trench",  
"Address3": "Telford",  
"Address4": "TF2 6RF",  
"DeliveryAddress": "93 Teagues Crescent<br>Trench",  
"DeliveryAddress1": "93 Teagues Crescent",  
"DeliveryAddress2": "Trench",  
"AdministrativeArea": "Shropshire",  
"Locality": "Telford",  
"DependentLocality": "Trench",  
"Thoroughfare": "Teagues Crescent",  
"Premise": "93",  
"PostalCode": "TF2 6RF",  
"CountryName": "United Kingdom",  
"ISO3166-2": "GB",  
"ISO3166-3": "GBR",  
"ISO3166-N": "826",  
"PostalCodePrimary": "TF2 6RF",  
"AVC": "V44-I44-P6-100",  
"AQI": "A",  
"Sequence": "1",  
"MatchRuleLabel": "Rlfnp",  
"HyphenClass": "B",  
"PremiseNumber": "93",  
"Country": "GB"  
}
```

```XML
<?xml version="1.0" encoding="UTF-8"?>
<PostalAddress xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
               xsi:noNamespaceSchemaLocation="UPU_S42a-6_PostalAddress.xsd">
  <Addressee>
    <PersonName>Gary Newport</PersonName>
    <Function>Associate, Solution Architect</Function>
  </Addressee>
  <Organisation>
    <OrganisationName>ExampleCorp Ltd</OrganisationName>
    <Department>Architecture Team</Department>
  </Organisation>
  <Premise>
    <BuildingName>Innovation House</BuildingName>
    <BuildingNumber>42</BuildingNumber>
    <Floor>5</Floor>
    <Room>501</Room>
  </Premise>
  <Thoroughfare>
    <ThoroughfareName>Tech Street</ThoroughfareName>
    <ThoroughfareNumber>42</ThoroughfareNumber>
  </Thoroughfare>
  <Locality>
    <PostTown>London</PostTown>
    <Postcode>EC2A 4DP</Postcode>
    <Country>
      <CountryName>United Kingdom</CountryName>
      <CountryCode>GB</CountryCode>
    </Country>
  </Locality>
</PostalAddress>

```

```XML
<?xml version="1.0" encoding="UTF-8"?>
<xs:schema xmlns:xs="http://www.w3.org/2001/XMLSchema"
           targetNamespace="http://www.upu.int/s42"
           xmlns="http://www.upu.int/s42"
           elementFormDefault="qualified">

  <xs:element name="PostalAddress">
    <xs:complexType>
      <xs:sequence>
        <xs:element name="Addressee" minOccurs="0">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="PersonName" type="xs:string"/>
              <xs:element name="Function" type="xs:string" minOccurs="0"/>
            </xs:sequence>
          </xs:complexType>
        </xs:element>

        <xs:element name="Organisation" minOccurs="0">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="OrganisationName" type="xs:string"/>
              <xs:element name="Department" type="xs:string" minOccurs="0"/>
            </xs:sequence>
          </xs:complexType>
        </xs:element>

        <xs:element name="Premise" minOccurs="0">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="BuildingName" type="xs:string" minOccurs="0"/>
              <xs:element name="BuildingNumber" type="xs:string"/>
              <xs:element name="Floor" type="xs:string" minOccurs="0"/>
              <xs:element name="Room" type="xs:string" minOccurs="0"/>
            </xs:sequence>
          </xs:complexType>
        </xs:element>

        <xs:element name="Thoroughfare" minOccurs="0">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="ThoroughfareName" type="xs:string"/>
              <xs:element name="ThoroughfareNumber" type="xs:string"/>
            </xs:sequence>
          </xs:complexType>
        </xs:element>

        <xs:element name="Locality">
          <xs:complexType>
            <xs:sequence>
              <xs:element name="PostTown" type="xs:string"/>
              <xs:element name="Postcode" type="xs:string"/>
              <xs:element name="Country">
                <xs:complexType>
                  <xs:sequence>
                    <xs:element name="CountryName" type="xs:string"/>
                    <xs:element name="CountryCode" type="xs:string"/>
                  </xs:sequence>
                </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>

      </xs:sequence>
    </xs:complexType>
  </xs:element>

</xs:schema>

```







