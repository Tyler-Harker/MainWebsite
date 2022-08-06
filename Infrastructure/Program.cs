using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using System;
using System.Collections.Generic;

return await Pulumi.Deployment.RunAsync(() =>
{
    var pulumiConfig = new Pulumi.Config();
    var config = pulumiConfig.RequireObject<TylerHarker.MainWebsite.Infrastructure.Configuration.Config>("config");

    var resourceGroup = new ResourceGroup($"tgh-mainwebsite-{config.EnvironmentAbbreviation}", new Pulumi.AzureNative.Resources.ResourceGroupArgs
    {
        ResourceGroupName = $"tgh-mainwebsite-{config.EnvironmentAbbreviation}"
    });

    var mainWebsiteAppServicePlan = new Pulumi.AzureNative.Web.AppServicePlan($"tgh-appserviceplan-mainwebsite-{config.EnvironmentAbbreviation}", new Pulumi.AzureNative.Web.AppServicePlanArgs
    {
        Name = $"tgh-appserviceplan-mainwebsite-{config.EnvironmentAbbreviation}",
        Kind = "app",
        ResourceGroupName = resourceGroup.Name,
        Reserved = true,
        Sku = new Pulumi.AzureNative.Web.Inputs.SkuDescriptionArgs
        {
            Name = "F1",
            Capacity = 1,
            Size = "F1",
            Tier = "Free"
        }
    });

    var mainWebsiteAppService = new Pulumi.AzureNative.Web.WebApp($"tgh-mainwebsite-{config.EnvironmentAbbreviation}", new Pulumi.AzureNative.Web.WebAppArgs
    {
        Name = $"tgh-mainwebsite-{config.EnvironmentAbbreviation}",
        ResourceGroupName = resourceGroup.Name,
        ServerFarmId = mainWebsiteAppServicePlan.Id,
        HttpsOnly = true,
        SiteConfig = new Pulumi.AzureNative.Web.Inputs.SiteConfigArgs
        {
            LinuxFxVersion = "DOTNETCORE|6.0"
        },
    });

    //var mainWebsiteAppservice = new Pulumi.AzureNative.Resources.Apps

    //// Create an Azure Resource Group
    ////var resourceGroup = new ResourceGroup("resourceGroup");

    //// Create an Azure resource (Storage Account)
    //var storageAccount = new StorageAccount("sa", new StorageAccountArgs
    //{
    //    ResourceGroupName = resourceGroup.Name,
    //    Sku = new SkuArgs
    //    {
    //        Name = SkuName.Standard_LRS
    //    },
    //    Kind = Kind.StorageV2
    //});

    //var storageAccountKeys = ListStorageAccountKeys.Invoke(new ListStorageAccountKeysInvokeArgs
    //{
    //    ResourceGroupName = resourceGroup.Name,
    //    AccountName = storageAccount.Name
    //});

    //var primaryStorageKey = storageAccountKeys.Apply(accountKeys =>
    //{
    //    var firstKey = accountKeys.Keys[0].Value;
    //    return Output.CreateSecret(firstKey);
    //});

    //// Export the primary key of the Storage Account
    //return new Dictionary<string, object?>
    //{
    //    ["primaryStorageKey"] = primaryStorageKey
    //};
});