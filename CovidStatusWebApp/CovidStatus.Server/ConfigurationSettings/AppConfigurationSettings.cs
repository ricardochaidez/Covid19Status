namespace CovidStatus.Server.ConfigurationSettings
{
    public class AppConfigurationSettings
    {
        public static string Environment;
        public static string CovidDataAPIClientAddress;
        public static string SyncfusionLicense;

        public static decimal MinimalMin;
        public static decimal MinimalMax;

        public static decimal ModerateMin;
        public static decimal ModerateMax;

        public static decimal SubstantialMin;
        public static decimal SubstantialMax;

        public static decimal WidespreadMin;
        public static decimal WidespreadMax;

        public static int CaliforniaWaitTimeRequirement;
        public static int CriticalDaysCount;

        public static string CaliforniaCovidOpenDataAddress;
        public static string CaliforniaCovidHospitalOpenDataAddress;
        public static int CaliforniaCovidOpenDataLimit;
    }
}
