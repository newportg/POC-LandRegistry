# 7, Deployment View

![image](.attachments/DeploymentView.png =750x)

## Security

The application is accessed by Internal web applications.

* Internal are within the Company network.
* Not accessible from the public internet.
* Managed Identity will be used between APIM and the Azure Function Service.
* The Azure Function will validate the MI token.
* The service will authenticate with the LR Service using a Secure Certificate.

## Application

* The Application resource group will contain the usual components for a microservice.
* The components will follow our naming convention.

| Component            | Name                 |                                                |
| -------------------- | -------------------- | ---------------------------------------------- |
| Azure Function       | func-landRegistry-env-ne |                                                |
| Application Insights | appi-landRegistry-env-ne |                                                |
| Storage Account      | stLandRegistryEnvNe      | Camelcase for readability, should be lowercase |
| Key Vault            | key-landRegistry-env-ne  |                                                |

**N.B. Env should be changed to match the deployment environment.**

The Application will be registered in Active Directory (Entra).

## DataStore.

* The application uses a Cosmos Database as its data store.
* A new cosmos DB container of NOSQL type should be set up.
