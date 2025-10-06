@description('The name of the resource group')
param resourceGroupName string = 'hotelbooking-rg'
@description('The location for all resources')
param location string = resourceGroup().location
@description('The name of the app service')
param appServiceName string = 'hotelbooking-admg'
@description('The name of the app service plan')
param appServicePlanName string = 'hotelbooking-plan-admg'

@description('The name of the Application Insights resource')
param appInsightsName string = 'hotelbooking-insights-admg'

@description('The name of the SQL Server')
param sqlServerName string = 'hotelbooking-sql-admg'

@description('The name of the SQL Database')
param sqlDatabaseName string = 'hotelbooking-db'

@description('SQL Server administrator login')
param sqlAdminLogin string = 'hotelbookingadmin'

@description('SQL Server administrator password')
@secure()
param sqlAdminPassword string = ''

// Log Analytics Workspace (Free Tier - 5GB/month free)
resource appInsightsWorkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: 'hotelbooking-workspace-${uniqueString(resourceGroup().id)}'
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    workspaceCapping:{dailyQuotaGb: json('0.17')}
    retentionInDays: 30
  }
}

// Azure SQL Server (Free Tier)
resource sqlServer 'Microsoft.Sql/servers@2023-02-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: sqlAdminLogin
    administratorLoginPassword: sqlAdminPassword
    version: '12.0'
  }
}

// Azure SQL Database (Free Tier - 2GB storage)
resource sqlDatabase 'Microsoft.Sql/servers/databases@2023-02-01-preview' = {
  parent: sqlServer
  name: sqlDatabaseName
  location: location
  sku: {
    name: 'Free'
    tier: 'Free'
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
  }
}

// Firewall rule to allow Azure services
resource sqlFirewallRule 'Microsoft.Sql/servers/firewallRules@2023-02-01-preview' = {
  parent: sqlServer
  name: 'AllowAzureServices'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

// Application Insights (Free Tier - 5GB/month free)
resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Request_Source: 'rest'
    WorkspaceResourceId: appInsightsWorkspace.id
  }
}

// App Service Plan (Free Tier)
resource appServicePlan 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'F1'
    tier: 'Free'
    size: 'F1'
    family: 'F'
    capacity: 0
  }
  properties: {
    reserved: true
  }
}

// App Service (Free Tier - F1)
resource appService 'Microsoft.Web/sites@2023-01-01' = {
  name: appServiceName
  location: location

  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: 'DOTNET|9.0' 
      appCommandLine: 'dotnet HotelBooking.Api.dll' 
      appSettings: [
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'Production'
        }
        {
          name: 'ASPNETCORE_URLS'
          value: 'https://+:443;http://+:80'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsights.properties.ConnectionString
        }
        {
          name: 'ConnectionStrings__DefaultConnection'
          value: 'Server=tcp:${sqlServer.properties.fullyQualifiedDomainName},1433;Initial Catalog=${sqlDatabase.name};Persist Security Info=False;User ID=${sqlServer.properties.administratorLogin};Password=${sqlAdminPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
        }
      ]
      alwaysOn: false
      use32BitWorkerProcess: true
      netFrameworkVersion: 'v8.0'
      ftpsState: 'Disabled' // Disable FTP for security
      minTlsVersion: '1.2'
    }
    httpsOnly: true
  }
}

// Output the URL and monitoring info
output appServiceUrl string = 'https://${appService.properties.defaultHostName}'
output appServiceName string = appService.name
output appInsightsUrl string = 'https://portal.azure.com/#@${subscription().tenantId}/resource${appInsights.id}'
output appInsightsInstrumentationKey string = appInsights.properties.InstrumentationKey
output sqlServerName string = sqlServer.name
output sqlDatabaseName string = sqlDatabase.name
output sqlConnectionString string = 'Server=tcp:${sqlServer.properties.fullyQualifiedDomainName},1433;Initial Catalog=${sqlDatabase.name};Persist Security Info=False;User ID=${sqlServer.properties.administratorLogin};Password=${sqlAdminPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
