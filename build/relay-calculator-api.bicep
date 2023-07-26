param location string = resourceGroup().location // Location for all resources
var appServicePlanName = toLower('relay-calculator-asp')
param appServiceHostingSkuName string
param appServiceSkuCapacity int
param sqlAdministratorLogin string
param sqlAdministratorLoginPassword string
var apiName = 'relay-calculator-api'
var sqlName = 'relay-calculator-sql'
var databaseName = 'relay-calculations'

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

resource sqlServer 'Microsoft.Sql/servers@2020-11-01-preview' = {
  name: sqlName
  location: location
  properties: {
    administratorLogin: sqlAdministratorLogin
    administratorLoginPassword: sqlAdministratorLoginPassword
    version: '12.0'
  }
}

resource relayCalculatorDatabase 'Microsoft.Sql/servers/databases@2021-05-01-preview' = {
  name: databaseName
  location: location
  parent: sqlServer
  sku: {
    name: 'Basic'
  }
  properties: {}
}
