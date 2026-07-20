# Develop a mechanism to generate and store unique message IDs with respect to the request sent to Business Gateway

Each request sent to Business Gateway must contain a unique message ID in the SOAP header. This message ID is used to track the request and its corresponding response. 

The message ID must be generated in a way that ensures its uniqueness across all requests. A common approach is to use a UUID (Universally Unique Identifier) or a combination of timestamp and a random component.

The generated message ID should be stored in a persistent storage mechanism (e.g., database, key-value store) along with the request details to facilitate tracking and correlation with the response received from Business Gateway.

When a response is received from Business Gateway, the message ID in the response should be used to look up the corresponding request in the storage mechanism to process the response appropriately.

**N.B.** The message ID generation and storage mechanism should be designed to handle high concurrency and ensure that message IDs are not duplicated, even under heavy load conditions.


![MessageId Sequence](.attachments/service/MessageID.png)

