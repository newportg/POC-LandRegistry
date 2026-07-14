## API

### Land Registry Information

<details>
   <summary>
      <code>POST</code>
      <code><b>/lrinfo/GetTitleDetails</b></code> 
      <code>(Returns Land Registry Documents)</code>
   </summary>

#### Description
Calls the Land Registry, and returns document ids relevant to the passed address.

#### Request
![image](.attachments/ServiceBusReq.png )

#### Responses

> | http code     | content-type  | response ||
> |---|--- |---|---|
> | `200`| `application/json`   | [Json Response](/5-building-block-view/azure-function-detail/API/API-Response.md)      | |
> | `400`| `application/json`   | `{"code":"400","message":"Bad Request"}`  | The requested data does not exist |
> | `404`| `application/json`   | `{"code":"404","message":"Not Found"}`    | The request was understood, but no data was found|
> | `409`| `application/json`   | `{"code":"409","message":"Conflict"}`     | The request Application Id already exists |

</details>

<details>
 <summary>
    <code>GET</code> 
    <code><b>/lrinfo/ByApplicationId/{id}</b></code> 
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
> | `200`| `application/json`   | [Json Response](/5-building-block-view/azure-function-detail/API/API-Response.md)      | |
> | `400`| `application/json`   | `{"code":"400","message":"Bad Request"}`  | The requested data does not exist |
> | `404`| `application/json`   | `{"code":"404","message":"Not Found"}`    | The request was understood, but no data was found|

#### Example cURL

> ```javascript
>  curl -X GET -H "Content-Type: application/json" http://localhost:8889/audit/ByApplicationId/hub/1111222233334444
> ```


</details>


### Audit

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
> | `200`| `application/json`   | [Json Response](/5-building-block-view/azure-function-detail/API/API-GetSourceApp.md)      | |
> | `400`| `application/json`   | `{"code":"400","message":"Bad Request"}`  | The requested data does not exist |
> | `404`| `application/json`   | `{"code":"404","message":"Not Found"}`    | The request was understood, but no data was found|


#### Example cURL

> ```javascript
>  curl -X GET -H "Content-Type: application/json" http://localhost:8889/audit/BySourceApp/hub
> ```

</details>

<details>
 <summary>
    <code>GET</code> 
    <code><b>/audit/User/{option}/{variable}</b></code> 
    <code>(Returns the count of requests made by a user)</code>
</summary>

#### Description
The request returns a list of users and number of requests that user has made.

If no user details are passed then the request will return a list of all users and their request counts, but will not return the detail.

If either the User Id/Login Name or Email Address is passed then the details for that user will be returned.

 
#### Parameters

> | name | type | data type | description |
> |---|---|---|---|
> | One Of |||
> | ||||
> | `ID`   | optional | string   | User Id  |
> | `User Id`   | optional | string   | User Id  |
> | ||||
> | `Login`  | optional | string   | Login Name  |
> | `Login Name`  | optional | string   | Login Name  |
> | ||||
> | `Email`  | optional | string   | Email Address  |
> | `User Email Address`  | optional | string   | Email Address  |



#### Responses

> | http code     | content-type  | response ||
> |---|--- |---|---|
> | `200`| `application/json`   | [Json Response](/5-building-block-view/azure-function-detail/API/API-GetUser.md)      | |
> | `400`| `application/json`   | `{"code":"400","message":"Bad Request"}`  | The requested data does not exist |
> | `404`| `application/json`   | `{"code":"404","message":"Not Found"}`    | The request was understood, but no data was found|


#### Example cURL

> ```javascript
>  curl -X GET -H "Content-Type: application/json" http://localhost:8889/audit/User/login/newportg
> ```

</details>

<details>
 <summary>
    <code>GET</code> 
    <code><b>/audit/impersonating/{option}/{variable}</b></code> 
    <code>(Returns the count of requests made by a impersonating user)</code>
</summary>

#### Description
The request returns a list of users and number of requests that user has made.

If no user details are passed then the request will return a list of all users and their request counts, but will not return the detail.

If either the User Id/Login Name or Email Address is passed then the details for that user will be returned.

 
#### Parameters

> | name | type | data type | description |
> |---|---|---|---|
> | One Of |||
> | ||||
> | `ID`   | optional | string   | Application Name  |
> | `User Id`   | optional | string   | User Id  |
> | ||||
> | `Login`  | optional | string   | Application Name  |
> | `Login Name`  | optional | string   | Login Name  |
> | ||||
> | `Email`  | optional | string   | Application Name  |
> | `User Email Address`  | optional | string   | Application Name  |



#### Responses

> | http code     | content-type  | response ||
> |---|--- |---|---|
> | `200`| `application/json`   | [Json Response](/5-building-block-view/azure-function-detail/API/API-GetImpersonating.md)      | |
> | `400`| `application/json`   | `{"code":"400","message":"Bad Request"}`  | The requested data does not exist |
> | `404`| `application/json`   | `{"code":"404","message":"Not Found"}`    | The request was understood, but no data was found|


#### Example cURL

> ```javascript
>  curl -X GET -H "Content-Type: application/json" http://localhost:8889/audit/impersonating/login/newportg
> ```

</details>

<details>
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
> | `200`| `application/json`   | [Json Response](/5-building-block-view/azure-function-detail/API/API-GetHost.md)      | |
> | `400`| `application/json`   | `{"code":"400","message":"Bad Request"}`  | The requested data does not exist |
> | `404`| `application/json`   | `{"code":"404","message":"Not Found"}`    | The request was understood, but no data was found|


#### Example cURL

> ```javascript
>  curl -X GET -H "Content-Type: application/json" http://localhost:8889/audit/host
> ```

</details>
