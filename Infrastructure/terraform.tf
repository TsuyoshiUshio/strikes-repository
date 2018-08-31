
# Service Principle
variable "subscription_id" {}
variable "client_id" {}
variable "client_secret" {}
variable "tenant_id" {}

# Resource Group
variable "resource_group" {
  default = "strikes-repository-rg"
}
variable "location" {
  default = "japaneast"
}
variable "tag_name" {
  default = "strikes-repository"
}

variable "environment_base_name" {
  default = "strikesback"
}

# Storage Account (Repository)

variable "repository_name" {
  default = "strikesrepo"
}


resource "azurerm_resource_group" "test" {
  name     = "${var.resource_group}"
  location = "${var.location}"
  tags {
    name = "${var.tag_name}"
  }
}

resource "random_string" "suffix" {
  length = 8
  special = false
  upper = false
}

resource "azurerm_storage_account" "test" {
  name                     = "${var.environment_base_name}sa${random_string.suffix.result}"
  resource_group_name      = "${azurerm_resource_group.test.name}"
  location                 = "${azurerm_resource_group.test.location}"
  account_tier             = "Standard"
  account_replication_type = "LRS"
}


resource "azurerm_storage_account" "repo" {
  name                     = "${var.repository_name}${random_string.suffix.result}"
  resource_group_name      = "${azurerm_resource_group.test.name}"
  location                 = "${azurerm_resource_group.test.location}"
  account_tier             = "Standard"
  account_replication_type = "LRS"
}


resource "azurerm_app_service_plan" "test" {
  name                = "${var.environment_base_name}plan"
  location            = "${azurerm_resource_group.test.location}"
  resource_group_name = "${azurerm_resource_group.test.name}"
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_application_insights" "test" {
  name                = "${var.environment_base_name}ai"
  location            = "eastus"
  resource_group_name = "${azurerm_resource_group.test.name}"
  application_type    = "Web"
}

# CosmosDB

resource "random_integer" "ri" {
    min = 10000
    max = 99999
}

resource "azurerm_cosmosdb_account" "db" {
    name                = "${var.environment_base_name}-db-${random_integer.ri.result}"
    location            = "${azurerm_resource_group.test.location}"
    resource_group_name = "${azurerm_resource_group.test.name}"
    offer_type          = "Standard"
    kind                = "GlobalDocumentDB"

    enable_automatic_failover = true

    consistency_policy {
        consistency_level       = "BoundedStaleness"
        max_interval_in_seconds = 10
        max_staleness_prefix    = 200
    }

    geo_location {
        location          = "${azurerm_resource_group.test.location}"
        failover_priority = 0
    }
}


resource "azurerm_function_app" "test" {
  name                      = "${var.environment_base_name}app"
  location                  = "${azurerm_resource_group.test.location}"
  resource_group_name       = "${azurerm_resource_group.test.name}"
  app_service_plan_id       = "${azurerm_app_service_plan.test.id}"
  storage_connection_string = "${azurerm_storage_account.test.primary_connection_string}"

  app_settings {
    "APPINSIGHTS_INSTRUMENTATIONKEY" = "${azurerm_application_insights.test.instrumentation_key}"
    "FUNCTIONS_EXTENSION_VERSION" = "beta"
    "assetServerUri" = "https://asset.simplearchitect.club/"
    "repoConnectionString" = "${azurerm_storage_account.repo.primary_connection_string}"
    "cosmosDBConnection" = "AccountEndpoint=${azurerm_cosmosdb_account.db.endpoint};AccountKey=${azurerm_cosmosdb_account.db.primary_master_key};"
  }
}

resource "azurerm_search_service" "test" {
  name  = "${var.environment_base_name}search"
  location                  = "japanwest"
  resource_group_name       = "${azurerm_resource_group.test.name}"
  sku  = "standard"

  tags {
    environment = "production"
    database = "${azurerm_cosmosdb_account.test.name}"
  }
}