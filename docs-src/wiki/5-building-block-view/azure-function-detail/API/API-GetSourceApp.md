# API Audit Get By Source Application

## Calls that will illicit this response.

<details>
 <summary>
    <code>GET</code> 
    <code><b>/audit/BySourceApp/{app}</b></code> 
    <code>(Returns the count of requests made by source Application)</code>
</summary>

#### Description
Returns a list of source application name and a count. The count is the number of times a request has been made by that application.
If a Application name is specified then only the count for that application will be returned.
If no Application name is specified then a list of all application names and their counts will be returned.

#### Parameters

> | name | type | data type | description |
> |---|---|---|---|
> | `App Name`   | optional | string   | Application Name  |

#### Responses

> | http code     | content-type  | response ||
> |---|--- |---|---|
> | `200`| `application/json`   | `See Below`      | |
> | `400`| `application/json`   | `{"code":"400","message":"Bad Request"}`  | The requested data does not exist |
> | `404`| `application/json`   | `{"code":"404","message":"Not Found"}`    | The request was understood, but no data was found|


#### Example cURL

> ```javascript
>  curl -X GET -H "Content-Type: application/json" http://localhost:8889/audit/BySourceApp/hub
> ```

</details>

## Response

![Get By Source App Response](/.attachments/API/GetBySourceAppRes.png)

## Field Descriptions
## Example JSON Object
```json
[
    {
        "SourceApp": "string",
        "Count": "string<number>"
    }
]
```
## JSON Schema 

```json
{
  "$schema": "http://json-schema.org/draft/2020-12/schema",
  "$id": "https://knightfrank.com/act/sourceappres.schema.json",
  "title": "Get By Source App response",
  "description": "Response from the internal ACT system",
  "type": "array",
  "items": [
    {
      "type": "object",
      "properties": {
        "SourceApp": {
          "type": "string"
        },
        "Count": {
          "type": "number"
        }
      },
      "required": [
        "SourceApp",
        "Count"
      ]
    }
  ]
}

```

## JSON validated against schema

```json

  [
    {
      "SourceApp": "hub",
      "Count": 50
    }
  ]

```