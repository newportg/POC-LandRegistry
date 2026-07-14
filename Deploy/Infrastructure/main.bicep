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
param para_svcPlanSku string = 'Dynamic'
param para_svcPlanSize string = 'Y1'
param para_svcPlanFamily string = 'Y'
param para_svcPlanCapacity string = '0'
param para_kvSecretsObject object
param para_svcPlan string

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
  }
}

var var_sub_id = subscription().subscriptionId
var var_ten_id = subscription().tenantId
var var_env_region_delim = toLower('${para_target_env}-${para_acronym_region}')
var var_env_region = toLower('${para_target_env}-${para_acronym_region}')
var var_application_name_delim = toLower('${para_appFamily_name}-${para_application_name}-')
var var_application_name = toLower(concat(para_application_name))
var var_str_name_var = concat(namingConvention.prefixes.Storage, take(toLower(para_application_name), 12), toLower(para_target_env), toLower(para_acronym_region))
var var_str_resId = resourceId(resourceGroup().name, 'Microsoft.Storage/storageAccounts', var_str_name_var)
var var_kv_name_var = '${namingConvention.prefixes.KeyVault}-${var_application_name_delim}${var_env_region_delim}'
var var_azf_name_var = '${namingConvention.prefixes.FunctionApp}-${var_application_name_delim}${var_env_region_delim}'
var var_appin_name_var = '${namingConvention.prefixes.AppInsights}-${var_application_name_delim}${var_env_region_delim}'
var var_id_name_var = '${namingConvention.prefixes.Identity}-${var_application_name_delim}${var_env_region_delim}'
var var_egt_name = '${namingConvention.prefixes.EventGridTopic}-${var_application_name_delim}${var_env_region_delim}'
var var_sr_name_var = '${namingConvention.prefixes.SignalR}-${var_application_name_delim}${var_env_region_delim}'
var var_swa_name_var = '${namingConvention.prefixes.StaticWebApp}-${var_application_name_delim}${var_env_region_delim}'
var var_evhns_name = '${namingConvention.prefixes.EventHubNS}-${var_application_name_delim}${var_env_region_delim}'
var var_evh_name = '${namingConvention.prefixes.EventHub}-${var_application_name_delim}${var_env_region_delim}'
var svcpln_name = '${namingConvention.prefixes.AppServicePlan}-${var_application_name_delim}${toLower(var_env_region_delim)}'
var var_svcpln_name_var = ((para_svcPlan == '') ? svcpln_name : para_svcPlan)
var var_uaid_name = '/subscriptions/${var_sub_id}/resourcegroups/${resourceGroup().name}/providers/Microsoft.ManagedIdentity/userAssignedIdentities/${toLower(var_id_name_var)}'

resource var_str_name 'Microsoft.Storage/storageAccounts@2016-01-01' = {
  kind: 'Storage'
  location: resourceGroup().location
  name: var_str_name_var
  properties: {
    encryption: {
      keySource: 'Microsoft.Storage'
      services: {
        blob: {
          enabled: true
        }
      }
    }
  }
  sku: {
    name: 'Standard_LRS'
    tier: 'Standard'
  }
  tags: {
    displayName: 'StorageAcct'
  }
  dependsOn: []
}

resource var_svcpln_name 'Microsoft.Web/serverfarms@2016-09-01' = {
  kind: 'app'
  location: resourceGroup().location
  name: var_svcpln_name_var
  properties: {
    adminSiteName: ''
    name: var_svcpln_name_var
    perSiteScaling: false
    reserved: false
    targetWorkerCount: 0
    targetWorkerSizeId: 0
    workerTierName: ''
  }
  scale: null
  sku: {
    name: para_svcPlanSize
    tier: para_svcPlanSku
    size: para_svcPlanSize
    family: para_svcPlanFamily
    capacity: para_svcPlanCapacity
  }
  tags: {
    displayName: 'Service Plan/ Farm'
  }
  dependsOn: [
    var_str_name
  ]
}

resource var_appin_name 'Microsoft.Insights/components@2015-05-01' = {
  kind: 'app'
  location: resourceGroup().location
  name: var_appin_name_var
  properties: {
    Application_Type: 'web'
    ApplicationId: var_appin_name_var
  }
  tags: {
    displayName: 'AppInsightsComponent'
  }
  dependsOn: [
    var_str_name
  ]
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

resource var_kv_name 'Microsoft.KeyVault/vaults@2016-10-01' = {
  location: resourceGroup().location
  name: var_kv_name_var
  properties: {
    accessPolicies: [
      {
        tenantId: '55a71488-bbff-4451-a18d-a1bfa479293b'
        objectId: 'cb3baa6a-f545-4caf-bd91-ba457692cbb9'
        permissions: {
          certificates: [
            'Get'
            'List'
            'Update'
            'Create'
            'Import'
            'Delete'
            'Recover'
            'ManageContacts'
            'ManageIssuers'
            'GetIssuers'
            'ListIssuers'
            'SetIssuers'
            'DeleteIssuers'
          ]
          keys: [
            'Get'
            'List'
            'Update'
            'Create'
            'Import'
            'Delete'
            'Recover'
            'Backup'
            'Restore'
            'Decrypt'
            'Encrypt'
            'UnwrapKey'
            'WrapKey'
            'Verify'
            'Sign'
            'Purge'
          ]
          secrets: [
            'Get'
            'List'
            'Set'
            'Delete'
            'Recover'
            'Backup'
            'Restore'
            'Purge'
          ]
        }
      }
    ]
    enabledForDeployment: false
    enabledForDiskEncryption: false
    enabledForTemplateDeployment: true
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: var_ten_id
  }
  scale: null
  tags: {
    displayName: 'Key Vault'
  }
  dependsOn: []
}

resource var_kv_name_StorageConnectionString 'Microsoft.KeyVault/vaults/secrets@2015-06-01' = {
  parent: var_kv_name
  name: 'StorageConnectionString'
  properties: {
    contentType: 'text/plain'
    value: 'DefaultEndpointsProtocol=https;AccountName=${var_str_name_var};AccountKey=${listKeys(var_str_name.id,providers('Microsoft.Storage','storageAccounts').apiVersions[0]).keys[0].value};'
  }
  tags: {
    displayName: 'Key Vault Secret'
  }
}

resource var_kv_name_para_kvSecretsObject_secrets_secret 'Microsoft.KeyVault/vaults/secrets@2015-06-01' = [
  for i in range(0, length(para_kvSecretsObject.secrets)): {
    name: '${var_kv_name_var}/${para_kvSecretsObject.secrets[i].secretName}'
    properties: {
      value: para_kvSecretsObject.secrets[i].secretValue
    }
    tags: {
      displayName: 'Key Vault Secrets'
    }
    dependsOn: [
      var_kv_name
    ]
  }
]

resource var_azf_name_appsettings 'Microsoft.Web/sites/config@2018-11-01' = {
  parent: var_azf_name
  location: resourceGroup().location
  name: 'appsettings'
  properties: {
    AzureWebJobsDashboard: 'DefaultEndpointsProtocol=https;AccountName=${var_str_name_var};AccountKey=${listKeys(var_str_resId,'2015-05-01-preview').key1}'
    AzureWebJobsStorage: 'DefaultEndpointsProtocol=https;AccountName=${var_str_name_var};AccountKey=${listKeys(var_str_resId,'2015-05-01-preview').key1}'
    APPINSIGHTS_INSTRUMENTATIONKEY: var_appin_name.properties.InstrumentationKey
    WEBSITE_ENABLE_SYNC_UPDATE_SITE: 'true'
    FUNCTIONS_EXTENSION_VERSION: '~4'
    FUNCTIONS_WORKER_RUNTIME: 'dotnet-isolated'
    StorageConnectionString: '@Microsoft.KeyVault(SecretUri=${var_kv_name_StorageConnectionString.properties.secretUriWithVersion})'
    LandRegistryCertificates: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryCertificates').secretUriWithVersion})'
    LandRegistryUserId: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryUserId').secretUriWithVersion})'
    LandRegistryPassword: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryPassword').secretUriWithVersion})'
    CertName: '@Microsoft.KeyVault(SecretUri=${reference('CertName').secretUriWithVersion})'
    // KeyVaultUri: '@Microsoft.KeyVault(SecretUri=${reference('KeyVaultUri').secretUriWithVersion})'
    KeyVaultUri: 'https://${var_kv_name_var}.vault.azure.net/' 
    LandRegistryBaseAddress: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryBaseAddress').secretUriWithVersion})'
    LandRegistryApplicationEnquiry: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryApplicationEnquiry').secretUriWithVersion})'
    LandRegistryLCBankruptcySearch: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryLCBankruptcySearch').secretUriWithVersion})'
    LandRegistryDischargeActivity: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryDischargeActivity').secretUriWithVersion})'
    LandRegistryEnquiryByPropertyDescription: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryEnquiryByPropertyDescription').secretUriWithVersion})'
    LandRegistryLCFullSearch: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryLCFullSearch').secretUriWithVersion})'
    LandRegistryOfficialCopyTitleKnown: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryOfficialCopyTitleKnown').secretUriWithVersion})'
    LandRegistryOfficialSearchWhole: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryOfficialSearchWhole').secretUriWithVersion})'
    LandRegistryOfficialSearchPart: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryOfficialSearchPart').secretUriWithVersion})'
    LandRegistryPollApplicationEnquiry: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryPollApplicationEnquiry').secretUriWithVersion})'
    LandRegistryPollLCBankruptcySearch: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryPollLCBankruptcySearch').secretUriWithVersion})'
    LandRegistryPollDischargeActivity: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryPollDischargeActivity').secretUriWithVersion})'
    LandRegistryPollEnquiryByPropertyDescription: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryPollEnquiryByPropertyDescription').secretUriWithVersion})'
    LandRegistryPollLCFullSearch: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryPollLCFullSearch').secretUriWithVersion})'
    LandRegistryPollOfficialSearchWhole: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryPollOfficialSearchWhole').secretUriWithVersion})'
    LandRegistryPollOfficialSearchPart: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryPollOfficialSearchPart').secretUriWithVersion})'
    LandRegistryExpectedPrice: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryExpectedPrice').secretUriWithVersion})'
    LandRegistryContinueIfFeeExceedsExpectedPrice: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryContinueIfFeeExceedsExpectedPrice').secretUriWithVersion})'
    LandRegistryContactName: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryContactName').secretUriWithVersion})'
    LandRegistryContactPhone: '@Microsoft.KeyVault(SecretUri=${reference('LandRegistryContactPhone').secretUriWithVersion})'
  }
  tags: {
    displayName: 'AppSettings'
    environment: 'parameters(\'para_target_env\')'
    'hidden-link:${resourceGroup().id}/providers/Microsoft.Web/serverfarms/${var_svcpln_name_var}': 'Resource'
    project: 'parameters(\'para_application_name\')'
  }
  dependsOn: [
    var_kv_name
    var_kv_name_para_kvSecretsObject_secrets_secret
  ]
}

resource var_kv_name_add 'Microsoft.KeyVault/vaults/accessPolicies@2016-10-01' = {
  parent: var_kv_name
  name: 'add'
  properties: {
    accessPolicies: [
      {
        objectId: reference(var_azf_name.id, '2019-08-01', 'Full').identity.principalId
        tenantId: var_ten_id
        permissions: {
          certificates: [
            'get'
            'list'
            'update'
            'create'
            'import'
            'delete'
            'recover'
            'managecontacts'
            'manageissuers'
            'getissuers'
            'listissuers'
            'setissuers'
            'deleteissuers'
          ]
          keys: [
            'get'
            'list'
            'update'
            'create'
            'import'
            'delete'
            'recover'
            'backup'
            'restore'
            'decrypt'
            'encrypt'
            'unwrapKey'
            'wrapKey'
            'verify'
            'sign'
            'purge'
          ]
          secrets: [
            'get'
            'list'
            'set'
            'delete'
            'recover'
            'backup'
            'restore'
            'purge'
          ]
        }
      }
    ]
  }
  tags: {
    displayName: 'KV Access Policies'
  }
}
