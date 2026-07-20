# API Audit Get Host

## Calls that will illicit this response.

 <summary>
    <code>GET</code> 
    <code><b>/audit/host/{option}/{variable}</b></code> 
    <code>(Returns the count of requests made by a impersonating user)</code>
</summary>

#### Description
The request returns a list made by host name or ip address.

if no parameters are specified then the request will return a list of host names and a count of requests made by that host.

If either a Hostname or Ip address is specified then the request will return list of the request made by that resource.

 
#### Parameters

> | name | type | data type | description |
> |---|---|---|---|
> | One Of |||
> | ||||
> | `HostName`   | optional | string   | Host Name  |
> | `hostname`   | optional | string   | hostname  |
> | ||||
> | `IPAddress`  | optional | string   | Ip Address  |
> | `ipaddress`  | optional | string   | Ip Address  |


#### Responses

> | http code     | content-type  | response ||
> |---|--- |---|---|
> | `200`| `application/json`   | `See Below`      | |
> | `400`| `application/json`   | `{"code":"400","message":"Bad Request"}`  | The requested data does not exist |
> | `404`| `application/json`   | `{"code":"404","message":"Not Found"}`    | The request was understood, but no data was found|


#### Example cURL

> ```javascript
>  curl -X GET -H "Content-Type: application/json" http://localhost:8889/audit/host
> ```


## Response

![Get Host Response](.attachments/API/GetByHostCountRes.png)

![Get Host Detail Response](.attachments/API/GetByHostDetailRes.png)



## Field Descriptions
## Example JSON Object
```json
{
    "Host":{
        "ipAddress":"string<ipv4>",
        "hostName":"string<hostname>"
    },
    "Count": "string<number>"
}

```

```json
{
    "Host":{
        "ipAddress":"string<ipv4>",
        "hostName":"string<hostname>"
    },
    "Count": "string<number>",
    "Requests" : [
        {
            "Id": "string",
            "When" : "string<DateTime>",
            "SourceApp": "string"
        }
    ]
} 
```

## JSON Schema 

```json
{
  "$schema": "http://json-schema.org/draft/2020-12/schema",
  "$id": "https://knightfrank.com/act/hostres.schema.json",
  "title": "Audit Get Host response",
  "description": "Response from the internal ACT system",
  "type": "object",
  "properties": {
    "Host": {
      "type": "object",
      "properties": {
        "ipAddress": {
          "type": "string",
          "format": "ip-address"
        },
        "hostName": {
          "type": "string"
        }
      },
      "required": [
        "ipAddress",
        "hostName"
      ]
    },
    "Count": {
      "type": "number"
    },
    "Requests": {
      "type": "array",
      "items": [
        {
          "type": "object",
          "properties": {
            "Id": {
              "type": "string",
              "format": "UUID"
            },
            "When": {
              "type": "string",
              "format": "datetime"
            },
            "SourceApp": {
              "type": "string"
            }
          },
          "required": [
            "Id",
            "When",
            "SourceApp"
          ]
        }
      ]
    }
  },
  "required": [
    "Host",
    "Count"
  ]
}
```

## JSON validated against schema

```json
{
    "Host": {
    	"ipAddress": "11.22.33.44",
        "hostName": "FreddoFrog"
    },
    "Count": 50,
    "Requests": [
      {
        "Id": "3e4666bf-d5e5-4aa7-b8ce-cefe41c7568a",
        "When": "2025-01-01 12:05:05",
        "SourceApp": "hub"
      }
    ]
}
```