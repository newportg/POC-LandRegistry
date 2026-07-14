# Nimbus Document Purchase API
API service for purchasing Nimbus-provided documents, including land registry (HMLR) documents and other property-related documents.

This API enables customers to:
- Check document availability and pricing for a title number
- View pending applications on the title
- Purchase documents with token-based billing
- Receive documents via webhook delivery

**N.B. This is a Markdown version of the OpenaAPI YAML file which can be found [Here](../../.attachments/nimbus-service-handler/openapi-docs.yaml)**

## Authentication

**N.B. OAuth details to be supplied**

This API uses OAuth 2.0 for authentication. Users must authenticate and authorize access to their account.

OAuth 2.0 Flow:
1. Direct users to the authorization endpoint to log in
2. User grants permission to your application
3. Receive an authorization code
4. Exchange the code for an access token
5. Include the access token in the `Authorization: Bearer <token>` header for all API requests

Access tokens expire after 1 hour. Use the refresh token to obtain a new access token without requiring the user to log in again.

## Rate Limits
- 60 requests per minute per user
- 1,000 requests per hour per user

## Workflow
1. Call `/check-availability` to see available documents and costs
2. Call `/purchase` with the documents you want to buy
3. Documents are delivered to your webhook URL when ready
4. Optionally poll `/orders/{order_id}` to check status


## Version: 1.0.0

**Contact information:**  
Nimbus Support  
support@nimbusproperty.co.uk  

**License:** Proprietary

<details>
 <summary>
    <code>GET</code> 
    <code><b>/check-availability</b></code> 
</summary>

##### Summary:

Check document availability and costs

##### Description:

Check which HMLR documents are available for a given title and the token cost for each.
Also returns information about any pending applications on the title.

You can query by either:
- **title_number**: UK Land Registry title number (e.g., AB123456)
- **title_id**: Internal UUID identifier for the title

Exactly one of these parameters must be provided.

This endpoint:
- Queries HMLR for available documents
- Queries HMLR for pending applications
- Calculates token costs based on document types
- Returns current account balance

Results are cached for 5 minutes in Azure API Management.


##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| title_number | query | UK Land Registry title number (e.g., AB123456). Either title_number or title_id must be provided. | No | string |
| title_id | query | Internal UUID identifier for the title. Either title_number or title_id must be provided. | No | string (uuid) |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Document availability information retrieved successfully |
| 400 |  |
| 401 |  |
| 404 | Title not found |
| 429 |  |
| 500 |  |
| 502 |  |

##### Security

| Security Schema | Scopes |
| --- | --- |
| OAuth2 | docs.read |


</details>

<details>
 <summary>
    <code>POST</code> 
    <code><b>/purchase</b></code> 
</summary>

##### Summary:

Purchase documents

##### Description:

Purchase one or more documents for a title.

This endpoint:
- Validates document availability with the document provider
- Checks account balance
- Deducts tokens from your account via the Ledger API
- Queues a background job to order and deliver documents
- Returns immediately with an order ID

Documents are delivered asynchronously to your webhook URL when ready.
Use the webhook_secret to verify webhook authenticity via HMAC signature.


##### Responses

| Code | Description |
| ---- | ----------- |
| 201 | Order created successfully and processing started |
| 400 |  |
| 401 |  |
| 402 |  |
| 409 | Duplicate order in progress.  This occurs when ANY of the requested documents are already being processed in another order (status: PENDING_PAYMENT or PROCESSING) for the same title.  The response will indicate which documents are in progress and in which order(s). You should either: - Wait for the existing order(s) to complete, then order again - Modify your request to exclude the documents that are in progress  **Note:** Documents that were previously purchased and delivered CAN be purchased again, as they may be out of date. The 'previously_purchased' field in the availability check is informational only to help you decide, but does not prevent repurchasing. |
| 429 |  |
| 500 |  |
| 502 |  |

##### Security

| Security Schema | Scopes |
| --- | --- |
| OAuth2 | docs.purchase |
</details>

<details>
 <summary>
    <code>GET</code> 
    <code><b>/orders/{order_id}</b></code> 
</summary>

##### Summary:

Get order status

##### Description:

Retrieve the current status of a document order.

Order statuses:
- `PENDING_PAYMENT`: Payment is being processed
- `PROCESSING`: Documents are being ordered from the provider
- `COMPLETED`: Documents have been delivered to webhook
- `FAILED`: Order failed (see error_message)
- `WEBHOOK_FAILED`: Documents ready but webhook delivery failed


##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| order_id | path | The unique identifier of the order | Yes | string (uuid) |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Order status retrieved successfully |
| 401 |  |
| 404 | Order not found |
| 429 |  |
| 500 |  |

##### Security

| Security Schema | Scopes |
| --- | --- |
| OAuth2 | docs.read |
</details>

<details>
 <summary>
    <code>GET</code> 
    <code><b>/download/{document_id}</b></code> 
</summary>

##### Summary:

Download a purchased document

##### Description:

Download a document from a completed order using its unique document ID.

This endpoint allows you to:
- Re-download documents that were previously delivered via webhook
- Download documents when webhook delivery failed
- Access documents from past orders

**No additional charge** - You've already paid for the document.

Documents are stored indefinitely so you can always re-download your previously purchased documents.

**Getting the document_id:** The unique document ID is provided in the order status response
after the document has been downloaded. Use the `document_id` field (UUID) from the DocumentDeliveryStatus.


##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| document_id | path | The unique document ID for the document (UUID from order status response) | Yes | string (uuid) |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Document retrieved successfully |
| 401 |  |
| 404 | Order or document not found |
| 422 | Document not yet ready for download |
| 429 |  |
| 500 |  |

##### Security

| Security Schema | Scopes |
| --- | --- |
| OAuth2 | docs.read |
</details>

<details>
 <summary>
    <code>POST</code> 
    <code><b>/ownership</b></code> 
</summary>

##### Summary:

Get registered owner of a title

##### Description:

Retrieve the registered owner(s) of a property title using HMLR's Online Owner Verification (OOV) service.

This endpoint:
- Queries HMLR for the registered owner(s) of a title by title number
- Returns complete ownership information for all registered proprietors
- Useful for due diligence, pre-transaction checks, and KYC processes


##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Ownership information retrieved successfully |
| 400 |  |
| 401 |  |
| 404 | Title not found |
| 429 |  |
| 500 |  |
| 502 |  |

##### Security

| Security Schema | Scopes |
| --- | --- |
| OAuth2 | docs.purchase |
</details>

<details>
 <summary>
    <code>GET</code> 
    <code><b>/health</b></code> 
</summary>

##### Summary:

Health check endpoint

##### Description:

Check the health status of the API service and its dependencies.

Returns 200 if all systems are operational.
Returns 503 if any critical dependency is unavailable.


##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Service is healthy |
| 503 | Service is unhealthy |

null
</details>

