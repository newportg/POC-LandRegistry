# Development activities

The Business Gateway listed the following tasks that should be completed in order to integrate with their service:

1. [Generate SSL certificate request and procure SSL certificates from HM Land Registry.](./DevelopmentActivities-CertificateRequest.md)
1. [Install SSL certificates issued by HM Land Registry as trusted root.](./DevelopmentActivities-InstallCertificates.md)
1. [Install client digital certificate issued by HM Land Registry into client Azure key vault](./DevelopmentActivities-InstallAzureKeyVaultCertificate.md)
1. [Develop a mechanism to generate and store unique message IDs with respect to the request sent to Business Gateway](./DevelopmentActivities-MessageID.md)
1. [Consume the WSDL offered by Business Gateway and develop the following functionality based on the web services exposed](./DevelopmentActivities-ServiceReference.md)
1. [Develop a mechanism to send requests to Business Gateway for each web service that has been exposed](./DevelopmentActivities-SendRequests.md)
1. [Develop a mechanism to submit Outstanding Request requests to get information about requests where the response is not available synchronously](./DevelopmentActivities-OutstandingRequests.md)
1. Develop a mechanism to send polling requests for the requests returned by the Outstanding Request service
   1. I think this was covered in the Outstanding Requests development activity.
   2. I dont think we will have a need for the Outstanding Requests service or Polled requests.
2. [Develop a mechanism to process responses returned by Business Gateway](./DevelopmentActivities-ProcessResponses.md)
3. Develop a mechanism to deal with exceptions
   1. Exceptions can occur but I think they can be minimised by following the other development activities correctly.
      1. For Example, Any data received by the API should be checked and validated against the schema, so we dont send invalid data to Business Gateway.
      2. If the WSDL is consumed correctly, then we should be able to avoid serialization exceptions.
      3. If the SSL certificates are installed correctly, then we should be able to avoid connection exceptions.
   2. If they do occur, then we should log them and notify the relevant stakeholders.
4. [For each request sent, the SOAP header must contain the user ID, password and locale information](./DevelopmentActivities-SOAPHeader.md)
5. For each request sent, the HTTPS channel will be mutually authenticated using certificate exchange
   1. See the CertificateProvider in the POC code for an example of how to do this.
   2. The Certificate Provider pulls the certificate from the Azure Key Vault, and loads it into memory.
   3. The CertificateProvider is injecteed into the request clients so that the WSClient method can apply the certificate to the request.