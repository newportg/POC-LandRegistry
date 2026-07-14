# Cosmos Db Component

A NoSql Cosmos DB container should be used as this service is similar in operation to other services, and also due to the variable length of Brochure array.

## Containers

```plantuml
{
    "id": "string<GUID>",    
    "audit": {
        "user": {
            "id" : "string<uuid>",
            "login": "string",
            "name": "string",
            "email": "string"
        },
        "impersonating": {
            "id" : "string<uuid>",
            "login": "string",
            "name": "string",
            "email": "string"
        },
        "host":{
            "ipAddress":"string<ipv4>",
            "hostName":"string<hostname>"
        },
        "timestamp": "string<datetime>"
    },
    "sourceApp": {
        "sourceApp": "string",
        "entityInfo": [
            {
                "entityType": "string",
                "entityId": "string<uuid>"
            }
        ]
    },
    "property" : {
        "titleNo": "string",
        "address" : {
            "buildingName":"string",
            "buildingNumber":"string",
            "streetName":"string",
            "cityName":"string",
            "postcode":"string"
        },
        "tenureFeeeHold": "boolean",
        "documents": [
            {
                "documentId": "string<uuid>",
                "fileInfo": {
                    "filename": "string",
                    "fileExtension": "string",
                    "localeIsoCode": "string<ISO 2 Char>",
                    "documentType": "string",
                    "version": "integer",
                    "versionDate": "string<datetime>",
                    "orderNo": "integer",
                    "size":"integer",
                    "fileMetaData": [
                        {
                            "key":"string",
                            "value":"string"
                        }
                    ]
                }
            }
        ]
    },
    "error" : [
      {
        "Description" : "String"
      }
    ]    
}

```

## Housekeeping

Data should be deleted from the datastore on a tenancy end date anniversary. 


