param location string = resourceGroup().location // Location for all resources
var appServicePlanName = toLower('relay-calculator-asp')
var storageAccountName = toLower('relaycalculatorstorage')
var functionName = toLower('relaycalculator-function-app')
param appServiceHostingSkuName string
param appServiceSkuCapacity int
var apiName = 'relay-calculator-api'
// param sqlAdministratorLogin string
// param sqlAdministratorLoginPassword string
// var sqlName = 'relay-calculator-sql'
// var databaseName = 'relay-calculations'

resource appServicePlan 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: appServicePlanName
  location: location
  sku: {
    tier: 'Standard'
    name: appServiceHostingSkuName
    capacity: appServiceSkuCapacity
  }
}

resource api 'Microsoft.Web/sites@2020-06-01' = {
  name: apiName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
  }
}

resource storageaccount 'Microsoft.Storage/storageAccounts@2021-02-01' = {
  name: storageAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
}

resource storageaccount_tableService 'Microsoft.Storage/storageAccounts/tableServices@2021-08-01' = {
  name: 'default'
  parent: storageaccount
}

resource storageaccount_tableService_table 'Microsoft.Storage/storageAccounts/tableServices/tables@2021-08-01' = {
  name: 'clubRecords'
  parent: storageaccount_tableService
} 

resource azFunctionApp 'Microsoft.Web/sites@2021-03-01' = {
  name: functionName
  kind: 'functionapp'
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    httpsOnly: true
    serverFarmId: appServicePlan.id
    clientAffinityEnabled: true
    reserved: true
    siteConfig: {
      alwaysOn: true
      appSettings: [
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
      ]
    }
  }
}

// resource sqlServer 'Microsoft.Sql/servers@2020-11-01-preview' = {
//   name: sqlName
//   location: location
//   properties: {
//     administratorLogin: sqlAdministratorLogin
//     administratorLoginPassword: sqlAdministratorLoginPassword
//     version: '12.0'
//   }
// }

// resource relayCalculatorDatabase 'Microsoft.Sql/servers/databases@2021-05-01-preview' = {
//   name: databaseName
//   location: location
//   parent: sqlServer
//   sku: {
//     name: 'Basic'
//   }
//   properties: {}
// }
