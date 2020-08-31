﻿using System;
using Microsoft.Extensions.Configuration;

namespace CovidStatus.Server.ConfigurationSettings
{
    public class AppConfigurationHelper
    {
        public void LoadConfigurationSettings()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            LoadGeneralSettings(config);
            LoadCovidStatusAPI(config);
            LoadSyncfusionSettings(config);
            LoadRiskLevelSettings(config);
        }
        private void LoadGeneralSettings(IConfiguration config)
        {
            AppConfigurationSettings.Environment = config.GetSection("Environment").Value;
        }
        private void LoadRiskLevelSettings(IConfiguration config)
        {
            AppConfigurationSettings.MinimalMin = Convert.ToDecimal(config.GetSection("RiskLevel").GetSection("MinimalMin").Value);
            AppConfigurationSettings.MinimalMax = Convert.ToDecimal(config.GetSection("RiskLevel").GetSection("MinimalMax").Value);
            AppConfigurationSettings.ModerateMin = Convert.ToDecimal(config.GetSection("RiskLevel").GetSection("ModerateMin").Value);
            AppConfigurationSettings.ModerateMax = Convert.ToDecimal(config.GetSection("RiskLevel").GetSection("ModerateMax").Value);
            AppConfigurationSettings.SubstantialMin = Convert.ToDecimal(config.GetSection("RiskLevel").GetSection("SubstantialMin").Value);
            AppConfigurationSettings.SubstantialMax = Convert.ToDecimal(config.GetSection("RiskLevel").GetSection("SubstantialMax").Value);
            AppConfigurationSettings.WidespreadMin = Convert.ToDecimal(config.GetSection("RiskLevel").GetSection("WidespreadMin").Value);
            AppConfigurationSettings.WidespreadMax = Convert.ToDecimal(config.GetSection("RiskLevel").GetSection("WidespreadMax").Value);
        }
        private void LoadCovidStatusAPI(IConfiguration config)
        {
            AppConfigurationSettings.CovidDataAPIClientAddress = config.GetSection("CovidDataAPIClientAddress").Value;
        }
        private void LoadSyncfusionSettings(IConfiguration config)
        {
            AppConfigurationSettings.SyncfusionLicense = config.GetSection("SyncfusionLicense").Value;
        }
    }
}