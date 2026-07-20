# Develop a mechanism to submit Outstanding Request requests to get information about requests where the response is not available synchronously.

When a request is sent to Business Gateway for which the response is not available synchronously, Business Gateway returns an acknowledgement response containing a unique message ID. This message ID can be used to query Business Gateway for the status of the request and to retrieve the response when it becomes available.
To implement this functionality, the following development activities are required:

1. A timer trigger or scheduling mechanism to periodically check for outstanding requests.
2. The message store will be checked for any missing or incomplete responces.
3. Develop a method to construct the Application Enquiry Request. 
   1. This message should include the unique message ID received in the previous acknowledgement responses.
4. Send the Application Enquiry Request to Business Gateway using the appropriate web service endpoint.
5. Handle the response received from Business Gateway. The response may indicate:
   1. The request is still being processed 
   2. May contain the final response if it is available. 
      1. If a responce is available, then get any Title Information, using the Title Known Request.
   3. An error occurred while processing the request.
      1. Such as missing or incomplete information.
      2. Log the error details for further investigation.
      3. Notify the relevant stakeholders about the error.
8. Update the message store with the status of the request and any response data received.


![OutstandingRequests](/.attachments/ServiceHandler/OutsandingRequest.png)