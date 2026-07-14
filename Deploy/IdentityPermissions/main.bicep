@allowed([
  'ne'
  'we'
])
param para_acronym_region string = 'ne'

@allowed([
  'vse'
  'dev'
  'alp'
  'bra'
  'cha'
  'pre'
  'uat'
  'prd'
])
param para_target_env string = 'vse'

@maxLength(3)
param para_appFamily_name string

@maxLength(12)
param para_application_name string
param para_role1 string = newGuid()
param para_role2 string = newGuid()


var namingConvention = {
  prefixes: {
    Storage: 'st'
    FunctionApp: 'func'
    AppInsights: 'appi'
    AppServicePlan: 'plan'
    KeyVault: 'kv'
    NetworkInterface: 'nic'
    PrivateEndpoint: 'pe'
    ResourceGroup: 'rg'
    SqlServer: 'sql'
    Identity: 'id'
    EventGridTopic: 'evgt'
    SignalR: 'sigr'
    StaticWebApp: 'stapp'
    EventHubNS: 'evhns'
    EventHub: 'evh'
    CommSvc: 'acs'
    CommSvcEmail: 'acse'
    CommSvcEmailDom: 'acsedm'
  }
}
var var_sub_id = subscription().subscriptionId
var var_ten_id = subscription().tenantId
var var_env_region_delim = toLower('${para_target_env}-${para_acronym_region}')
var var_application_name_delim = toLower('${para_appFamily_name}-${para_application_name}-')
var var_id_name = '${namingConvention.prefixes.Identity}-${var_application_name_delim}${var_env_region_delim}'
var var_azf_name = '${namingConvention.prefixes.FunctionApp}-${var_application_name_delim}${var_env_region_delim}'


resource para_role1_resource 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: para_role1
  properties: {
    roleDefinitionId: '/subscriptions/${subscription().subscriptionId}/providers/Microsoft.Authorization/roleDefinitions/4633458b-17de-408a-b874-0445c86b69e6'
    principalId: reference(resourceId('Microsoft.ManagedIdentity/userAssignedIdentities', var_id_name), '2023-01-31', 'full').properties.principalId
  }
  tags: {
    displayName: 'Microsoft.Authorization/roleAssignments - Key Vault Secrets User'
    environment: para_target_env
    project: para_application_name
  }
}

resource para_role2_resource 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: para_role2
  properties: {
    roleDefinitionId: '/subscriptions/${subscription().subscriptionId}/providers/Microsoft.Authorization/roleDefinitions/db79e9a7-68ee-4b58-9aeb-b90e7c24fcba'
    principalId: reference(resourceId('Microsoft.ManagedIdentity/userAssignedIdentities', var_id_name), '2023-01-31', 'full').properties.principalId
  }
  tags: {
    displayName: 'Microsoft.Authorization/roleAssignments - Key Vault Certificate User'
    environment: para_target_env
    project: para_application_name
  }
}

{
            "type": "Microsoft.ManagedIdentity/userAssignedIdentities",
            "apiVersion": "2023-07-31-preview",
            "name": "[parameters('userAssignedIdentities_id_poc_landregistry_vse_ne_name')]",
            "location": "northeurope",
            "tags": {
                "displayName": "UserAssignedIdentities",
                "environment": "vse",
                "project": "landregistry"
            }
        }

resource var_azf_name 'Microsoft.Web/sites@2016-08-01' = {
  identity: {
    type: 'SystemAssigned'
  }
  kind: 'functionapp'
  location: resourceGroup().location
  name: var_azf_name_var
  properties: {
    name: var_azf_name_var
    siteConfig: {
      alwaysOn: false
    }
    clientAffinityEnabled: false
    serverFarmId: var_svcpln_name_var
    hostNameSslStates: []
  }
  tags: {
    displayName: 'Az Function'
  }
  dependsOn: [
    var_svcpln_name
  ]
}

resource var_evgt_name_EmailSubscription 'Microsoft.EventGrid/systemTopics/eventSubscriptions@2022-06-15' = {
  name: '${var_evgt_name}/EmailSubscription'
  properties: {
    destination: {
      properties: {
        resourceId: resourceId('Microsoft.Web/sites/functions', var_azf_name, 'MailEventGridSubscription')
        maxEventsPerBatch: 1
        preferredBatchSizeInKilobytes: 64
      }
      endpointType: 'AzureFunction'
    }
    filter: {
      includedEventTypes: [
        'Microsoft.Communication.EmailEngagementTrackingReportReceived'
        'Microsoft.Communication.EmailDeliveryReportReceived'
      ]
      enableAdvancedFilteringOnArrays: true
    }
    labels: []
    eventDeliverySchema: 'EventGridSchema'
    retryPolicy: {
      maxDeliveryAttempts: 30
      eventTimeToLiveInMinutes: 1440
    }
  }
}

