using Microsoft.Extensions.Configuration;

namespace CovidStatus.API.ConfigurationSettings
{
    public class AppConfigurationHelper
    {
        public void LoadConfigurationSettings()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            LoadGeneralSettings(config);
        }

        private void LoadGeneralSettings(IConfiguration config)
        {
            AppConfigurationSettings.CaliforniaCovidOpenDataAddress = config.GetSection("CaliforniaCovidOpenDataAddress").Value;
            AppConfigurationSettings.CaliforniaCovidHospitalOpenDataAddress = config.GetSection("CaliforniaCovidHospitalOpenDataAddress").Value;
            AppConfigurationSettings.CaliforniaCovidOpenDataLimit = int.Parse(config.GetSection("CaliforniaCovidOpenDataLimit").Value);
            AppConfigurationSettings.CaliforniaCovidOpenDataFallbackAddress = config.GetSection("CaliforniaCovidOpenDataFallbackAddress").Value;
            AppConfigurationSettings.CaliforniaCovidHospitalOpenDataFallbackAddress = config.GetSection("CaliforniaCovidHospitalOpenDataFallbackAddress").Value;
        }
    }
}
