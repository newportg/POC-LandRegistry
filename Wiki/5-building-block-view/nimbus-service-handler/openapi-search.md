# Nimbus Property Search API
Direct access to Nimbus property data Elasticsearch index with safety guardrails.
Request structure mirrors Elasticsearch Query DSL.
For field reference and examples, see the Elasticsearch Schema documentation.

**N.B. This is a Markdown version of the OpenaAPI YAML file which can be found [Here](../../.attachments/nimbus-service-handler/openapi-search.yaml)**

## Version: 1.0.0

**Contact information:**  
Nimbus API Support  
support@nimbusproperty.co.uk  

## URL 
https://api-preprod.nimbusmaps.xyz/docs/v1


<details>
 <summary>
    <code>GET</code> 
    <code><b>/titles</b></code> 
</summary>

##### Summary:

Find a single title by ID or number

##### Description:

Retrieve a property title by UUID or title number. Supports lookups by ID (direct), or by number with optional country code (defaults to ENG). If both parameters provided, ID takes precedence.


##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | query | Title UUID for direct lookup | No | string (uuid) |
| number | query | Title number (case-insensitive, e.g., "LA135828") | No | string |
| country | query | Country ISO code (only used with 'number' parameter) | No | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Title found successfully |
| 400 | Missing or invalid parameters |
| 401 |  |
| 403 |  |
| 404 | Title not found |
| 429 |  |
| 500 |  |

##### Security

| Security Schema | Scopes |
| --- | --- |
| OAuth2 | search.titles |

</details>

<details>
 <summary>
    <code>POST</code> 
    <code><b></b></code> 
</summary>

##### Summary:

Search titles using Elasticsearch query DSL

##### Description:

Search property titles using Elasticsearch Query DSL. Limits: max 25 results, 5s timeout, from+size≤10000. Supports match, term, bool, range queries. See schema docs for field reference.


##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Search results (Elasticsearch response format) |
| 400 |  |
| 401 |  |
| 403 |  |
| 429 |  |
| 500 |  |
| 504 |  |

##### Security

| Security Schema | Scopes |
| --- | --- |
| OAuth2 | search.titles |

</details>

<details>
 <summary>
    <code>GET</code> 
    <code><b>/address</b></code> 
</summary>

##### Summary:

Search for an address

##### Description:

Find matching addresses by free-text query. Returns up to 10 results with full address, coordinates, title ID, tenure, and UPRN ID.

This endpoint calls a downstream address lookup service and returns simplified results optimized for address selection.


##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| address | query | Free-text address to search (e.g., "30 Penarth Road Bolton" or "BL3 5RJ") | Yes | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Address search results |
| 400 | Missing or invalid address parameter |
| 401 |  |
| 403 |  |
| 429 |  |
| 500 |  |
| 504 |  |

##### Security

| Security Schema | Scopes |
| --- | --- |
| OAuth2 | search.address |

#### Example cURL

##### Javascript
```javascript
curl -X GET "https://api-preprod.nimbusmaps.xyz/search/v1/address?address=55+Baker+Street+London" \
     -H "Ocp-Apim-Subscription-Key: 1be0002cb9c045379feaca296cc7db98"
```

##### Powershell
```Powershell
Invoke-WebRequest -Method Get `
  -Uri "https://api-preprod.nimbusmaps.xyz/search/v1/address?address=55+Baker+Street+London" `
  -Headers @{"Ocp-Apim-Subscription-Key" = "1be0002cb9c045379feaca296cc7db98"}

```


##### Response
```json
{
  "jsonapi": {
    "version": "1.0"
  },
  "data": [
    {
      "type": "searchIdResult",
      "attributes": {
        "uprnRef": {
          "id": "80cde373-7a28-4519-be3a-a1d7eaaeb9b4"
        },
        "titleRef": {
          "id": "04e9ee4c-d723-49fe-bd6c-00087b7c9476",
          "tenure": "Freehold"
        },
        "fullAddress": ", 55 BAKER STREET, LONDON, W1U 8EW",
        "latitude": 51.51890563964844,
        "longitude": -0.15634340047836304
      }
    }
}

```

</details>

<details>
 <summary>
    <code>GET</code> 
    <code><b>/health</b></code> 
</summary>

##### Summary:

Health check

##### Description:

Check the health status of the API

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Service is healthy |
| 503 | Service is unhealthy |

null
</details>

