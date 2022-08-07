using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using System;
using System.Collections.Generic;


/**
 * 
 * I'm going to be running everything under 1 appservice plan to save money for my own needs.
 * 
 */

return await Pulumi.Deployment.RunAsync(() =>
{
    var pulumiConfig = new Pulumi.Config();
    var config = pulumiConfig.RequireObject<TylerHarker.MainWebsite.Infrastructure.Configuration.Config>("config");

    var resourceGroup = new ResourceGroup($"tgh-mainwebsite", new Pulumi.AzureNative.Resources.ResourceGroupArgs
    {
        ResourceGroupName = $"tgh-mainwebsite"
    });

    var mainWebsiteAppServicePlan = new Pulumi.AzureNative.Web.AppServicePlan($"tgh-appserviceplan-mainwebsite", new Pulumi.AzureNative.Web.AppServicePlanArgs
    {
        Name = $"tgh-appserviceplan-mainwebsite",
        Kind = "app",
        ResourceGroupName = resourceGroup.Name,
        Reserved = true,
        Sku = new Pulumi.AzureNative.Web.Inputs.SkuDescriptionArgs
        {
            Name = config.AppServicePlanSize,
            Capacity = 1,
            Size = config.AppServicePlanSize,
            Tier = ""
        }
    });

    var mainWebsiteAppServiceStaging = new Pulumi.AzureNative.Web.WebApp($"tgh-mainwebsite-stg", new Pulumi.AzureNative.Web.WebAppArgs
    {
        Name = $"tgh-mainwebsite-stg",
        ResourceGroupName = resourceGroup.Name,
        ServerFarmId = mainWebsiteAppServicePlan.Id,
        HttpsOnly = true,
        SiteConfig = new Pulumi.AzureNative.Web.Inputs.SiteConfigArgs
        {
            LinuxFxVersion = "DOTNETCORE|6.0"
        },
    });
    var mainWebsiteAppServiceProduction = new Pulumi.AzureNative.Web.WebApp($"tgh-mainwebsite-prd", new Pulumi.AzureNative.Web.WebAppArgs
    {
        Name = $"tgh-mainwebsite-prd",
        ResourceGroupName = resourceGroup.Name,
        ServerFarmId = mainWebsiteAppServicePlan.Id,
        HttpsOnly = true,
        SiteConfig = new Pulumi.AzureNative.Web.Inputs.SiteConfigArgs
        {
            LinuxFxVersion = "DOTNETCORE|6.0"
        },
    });
});