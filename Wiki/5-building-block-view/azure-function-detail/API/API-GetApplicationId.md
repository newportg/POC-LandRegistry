## API Land Registry Info

## Calls that will illicit this response.


  <summary>
      <code>GET</code> 
      <code><b>/lrinfo/ByApplicationId/{App}/{id}</b></code> 
      <code>(Returns the information by the Source App Id)</code>
  </summary>

  #### Description
  Returns information held, by the source applications name and Id

  #### Parameters

  > | name | type | data type | description |
  > |---|---|---|---|
  > | `App Name`   | mandatory | string   | Application Name  |
  > | `App Id`   | mandatory | guid   | Application Id  |

  #### Responses

  > | http code     | content-type  | response ||
  > |---|--- |---|---|
  > | `200`| `application/json`   | `See Below`      | |
  > | `400`| `application/json`   | `{"code":"400","message":"Bad Request"}`  | The requested data does not exist |
  > | `404`| `application/json`   | `{"code":"404","message":"Not Found"}`    | The request was understood, but no data was found|

  #### Example cURL

  > ```javascript
  >  curl -X GET -H "Content-Type: application/json" http://localhost:8889/audit/ByApplicationId/hub/1111222233334444
  > ```

## Response

![image](.attachments/ServiceBusRes.png )

## Field Descriptions
## Example JSON Object validated against schema
```
{
    "id": "fb2e442e-374f-46a5-b4ef-f4b338a8a307",
    "audit": {
        "user": {
            "id": "fb2e442e-374f-46a5-b4ef-f4b338a8a307",
            "login": "newportg",
            "name": "Gary Newport",
            "email": "Gary.Newport@KnightFrank.com"
        },
        "impersonating": {
            "id": "fb2e442e-374f-46a5-b4ef-f4b338a8a30",
            "login": "AhmedA",
            "name": "Aneela Ahmed",
            "email": "Aneela.Ahmed@knightfrank.com"
        },
        "host": {
            "ipAddress": "127.0.0.1",
            "hostName": "MyLaptop"
        },
        "timestamp": "2025-01-01 12:05:05"
    },
    "sourceApp": {
        "sourceApp": "hub",
        "entityInfo": [
            {
                "entityType": "Activity",
                "entityId": "fb2e442e-374f-46a5-b4ef-f4b338a8a307"
            }
        ]
    },
    "property": {
        "titleNo": "string",
        "address": {
            "buildingNumber": "99",
            "buildingName": "A House",
            "streetName": "Street",
            "cityName": "City",
            "postcode": "AB12 3CD"
        },
        "documents": [
            {
                "documentId": "fb2e442e-374f-46a5-b4ef-f4b338a8a307",
                "fileInfo": {
                    "filename": "Deed",
                    "fileExtension": "png",
                    "localeIsoCode": "UK",
                    "documentType": "Land Registry Deed",
                    "version": 1,
                    "versionDate": "05-09-2025",
                    "orderNo": 0,
                    "size": 100,
                    "fileMetaData": [
                        {
                            "key": "key",
                            "value": "Value"
                        }
                    ]
                }
            }
        ]
    },
    "error": [
        {
            "Description": "String"
        }
    ]
}
```
## JSON Schema 

```json
{
    "$schema": "https://json-schema.org/draft/2020-12/schema",
    "type": "object",
    "required": [
        "audit",
        "id",
        "property",
        "sourceApp"
    ],
    "properties": {
        "id": {
            "type": "string",
            "format": "uuid"
        },
        "audit": {
            "type": "object",
            "required": [
                "host",
                "impersonating",
                "timestamp",
                "user"
            ],
            "properties": {
                "user": {
                    "type": "object",
                    "required": [
                        "email",
                        "id",
                        "login",
                        "name"
                    ],
                    "properties": {
                        "id": {
                            "type": "string",
                            "format": "uuid"
                        },
                        "login": {
                            "type": "string"
                        },
                        "name": {
                            "type": "string"
                        },
                        "email": {
                            "type": "string",
                            "format": "email"
                        }
                    }
                },
                "impersonating": {
                    "type": "object",
                    "required": [
                        "email",
                        "id",
                        "login",
                        "name"
                    ],
                    "properties": {
                        "id": {
                            "type": "string",
                            "format": "uuid"
                        },
                        "login": {
                            "type": "string"
                        },
                        "name": {
                            "type": "string"
                        },
                        "email": {
                            "type": "string",
                            "format": "email"
                        }
                    }
                },
                "host": {
                    "type": "object",
                    "required": [
                        "hostName",
                        "ipAddress"
                    ],
                    "properties": {
                        "ipAddress": {
                            "type": "string",
                            "format": "ipv4"
                        },
                        "hostName": {
                            "type": "string"
                        }
                    }
                },
                "timestamp": {
                    "type": "string"
                }
            }
        },
        "sourceApp": {
            "type": "object",
            "required": [
                "entityInfo",
                "sourceApp"
            ],
            "properties": {
                "sourceApp": {
                    "type": "string"
                },
                "entityInfo": {
                    "type": "array",
                    "items": {
                        "type": "object",
                        "required": [
                            "entityId",
                            "entityType"
                        ],
                        "properties": {
                            "entityType": {
                                "type": "string"
                            },
                            "entityId": {
                                "type": "string"
                            }
                        }
                    }
                }
            }
        },
        "property": {
            "type": "object",
            "required": [
                "address",
                "documents",
                "titleNo"
            ],
            "properties": {
                "titleNo": {
                    "type": "string"
                },
                "address": {
                    "type": "object",
                    "required": [
                        "buildingName",
                        "buildingNumber",
                        "cityName",
                        "postcode",
                        "streetName"
                    ],
                    "properties": {
                        "buildingNumber": {
                            "type": "string"
                        },
                        "buildingName": {
                            "type": "string"
                        },
                        "streetName": {
                            "type": "string"
                        },
                        "cityName": {
                            "type": "string"
                        },
                        "postcode": {
                            "type": "string"
                        }
                    }
                },
                "documents": {
                    "type": "array",
                    "items": {
                        "type": "object",
                        "required": [
                            "documentId",
                            "fileInfo"
                        ],
                        "properties": {
                            "documentId": {
                                "type": "string"
                            },
                            "fileInfo": {
                                "type": "object",
                                "required": [
                                    "documentType",
                                    "fileExtension",
                                    "fileMetaData",
                                    "filename",
                                    "localeIsoCode",
                                    "orderNo",
                                    "size",
                                    "version",
                                    "versionDate"
                                ],
                                "properties": {
                                    "filename": {
                                        "type": "string"
                                    },
                                    "fileExtension": {
                                        "type": "string"
                                    },
                                    "localeIsoCode": {
                                        "type": "string"
                                    },
                                    "documentType": {
                                        "type": "string"
                                    },
                                    "version": {
                                        "type": "integer"
                                    },
                                    "versionDate": {
                                        "type": "string"
                                    },
                                    "orderNo": {
                                        "type": "integer"
                                    },
                                    "size": {
                                        "type": "integer"
                                    },
                                    "fileMetaData": {
                                        "type": "array",
                                        "items": {
                                            "type": "object",
                                            "required": [
                                                "key",
                                                "value"
                                            ],
                                            "properties": {
                                                "key": {
                                                    "type": "string"
                                                },
                                                "value": {
                                                    "type": "string"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        },
        "error": {
            "type": "array",
            "items": {
                "type": "object",
                "required": [
                    "Description"
                ],
                "properties": {
                    "Description": {
                        "type": "string"
                    }
                }
            }
        }
    }
}

```

