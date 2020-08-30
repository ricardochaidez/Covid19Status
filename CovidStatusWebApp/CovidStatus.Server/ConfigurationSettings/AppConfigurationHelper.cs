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
            LoadConsultantSOQAPI(config);
            LoadSyncfusionSettings(config);
        }
        private void LoadGeneralSettings(IConfiguration config)
        {
            AppConfigurationSettings.Environment = config.GetSection("Environment").Value;
        }
        private void LoadConsultantSOQAPI(IConfiguration config)
        {
            AppConfigurationSettings.CovidDataAPIClientAddress = config.GetSection("CovidDataAPIClientAddress").Value;
        }
        private void LoadSyncfusionSettings(IConfiguration config)
        {
            AppConfigurationSettings.SyncfusionLicense = config.GetSection("SyncfusionLicense").Value;
        }
    }
}
