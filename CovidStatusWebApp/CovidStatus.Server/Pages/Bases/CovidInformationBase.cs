using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazor.Analytics;
using CovidStatus.Server.ConfigurationSettings;
using CovidStatus.Server.Helper;
using CovidStatus.Server.Services.Interfaces;
using CovidStatus.Shared.Entities;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Grids;

namespace CovidStatus.Server.Pages.Bases
{
    public class CovidInformationBase : ComponentBase
    {
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] private ICovidDataService CovidDataService { get; set; }
        [Inject] private IAnalytics Analytics { get; set; }
        public List<CovidData> CovidDataList { get; set; }
        public List<County> CountyList { get; set; }
        public County SelectedCounty { get; set; }

        public List<CriticalDay> CriticalDayList { get; set; }
        public int CriticalDaysCount { get; set; }
        public int DefaultCriticalDaysCount { get; set; }
        public byte DefaultCounty { get; set; }
        public string CriticalDaysMessage { get; set; }
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }

        public DateTime LastUpdateDate { get; set; }
        public SfGrid<CovidData> CovidDataGrid;

        protected override async Task OnInitializedAsync()
        {
            string countyName = "San Bernardino";
            CountyList = await CovidDataService.GetCountyList();
            SelectedCounty = CountyList.FirstOrDefault(x => x.CountyName == countyName);
            DefaultCounty = SelectedCounty?.CountyID ?? 0;

            CriticalDayList = await GetCriticalDayList();
            DefaultCriticalDaysCount = AppConfigurationSettings.CriticalDaysCount == 0 ? 7 : AppConfigurationSettings.CriticalDaysCount;
            CriticalDaysCount = DefaultCriticalDaysCount;

            await GetCriticalDaysMessage(CriticalDaysCount);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GetData(SelectedCounty.CountyName, DefaultCriticalDaysCount);
            }
        }

        private async Task GetData(string countyName, int criticalDaysCount)
        {
            try
            {
                DateTime startTime = DateTime.Now;

                await GetCovidData(countyName, criticalDaysCount);
                
                IsError = false;
                ErrorMessage = string.Empty;

                DateTime endTime = DateTime.Now;
                TimeSpan span = startTime - endTime;
                await Analytics.TrackEvent($"Get Data for {countyName} for {criticalDaysCount} days moving average", $"{span.TotalMilliseconds} milliseconds");
            }
            catch (Exception e)
            {
                IsError = true;
                ErrorMessage = $"<h4>CA Open Data API Is Unavailable</h4><br /><a target=\"_blank\" href=\"https://data.ca.gov/group/covid-19\">California Open Data</a> <br /><a target=\"_blank\" href=\"{AppConfigurationSettings.CaliforniaCovidOpenDataAddress}\">COVID-19 Cases</a><br /><a target=\"_blank\" href=\"{AppConfigurationSettings.CaliforniaCovidHospitalOpenDataAddress}\">COVID-19 Hospital Data</a>";
            }
            StateHasChanged();
        }

        private async Task GetCovidData(string countyName, int criticalDaysCount)
        {
            var covidRecords = await CovidDataService.GetCovidDataByCounty(countyName);

            DateTime lastUpdate = covidRecords.Select(x => x.Date).OrderByDescending(x => x.Date).FirstOrDefault();
            LastUpdateDate = lastUpdate;

            var covidDataHelper = new CovidDataHelper();
            covidDataHelper.PopulateAggregatesCovidData(covidRecords, SelectedCounty, criticalDaysCount);
            covidDataHelper.PopulateCountyAggregates(covidRecords, SelectedCounty, lastUpdate, criticalDaysCount);

            CovidDataList = covidRecords;
            StateHasChanged();
        }

        private async Task GetCriticalDaysMessage(int criticalDaysCount)
        {
            CriticalDaysMessage = $"{criticalDaysCount}-Day Moving Averages";
            StateHasChanged();
        }

        private async Task<List<CriticalDay>> GetCriticalDayList()
        {
            var criticalDays = new List<CriticalDay>
            {
                new CriticalDay {CriticalDayCount = 3},
                new CriticalDay {CriticalDayCount = 7},
                new CriticalDay {CriticalDayCount = 10},
                new CriticalDay {CriticalDayCount = 14},
                new CriticalDay {CriticalDayCount = 21},
                new CriticalDay {CriticalDayCount = 30}
            };

            if (!criticalDays.Exists(x => x.CriticalDayCount == AppConfigurationSettings.CriticalDaysCount))
            {
                criticalDays.Add(new CriticalDay{CriticalDayCount = AppConfigurationSettings.CriticalDaysCount });
            }
            return criticalDays;
        }

        public async Task OnCountyChanged(Syncfusion.Blazor.DropDowns.ChangeEventArgs<byte> args)
        {
            byte countyID = args.Value;
            SelectedCounty = CountyList.FirstOrDefault(x => x.CountyID == countyID);
            await GetData(SelectedCounty?.CountyName, CriticalDaysCount);
        }

        public async Task OnCriticalDaysChanged(Syncfusion.Blazor.DropDowns.ChangeEventArgs<int> args)
        {
            int criticalDays = args.Value;
            CriticalDaysCount = criticalDays;
            await GetCriticalDaysMessage(CriticalDaysCount);
            await GetData(SelectedCounty?.CountyName, CriticalDaysCount);
        }

        public async Task ExcelExport()
        {
            var exportProperties = new ExcelExportProperties();
            exportProperties.IncludeHiddenColumn = true;
            await CovidDataGrid.ExcelExport(exportProperties);
        }

        public async Task GetCasesRawDataPerCounty()
        {
            string rawData = await CovidDataService.GetCARawCovidJsonDataByCounty(SelectedCounty?.CountyName);
            await DownloadJsonFile(rawData, $"{SelectedCounty?.CountyName}_CAOpenData_COVID_{DateTime.Now:yyyy-MM-dd}.json");
        }

        public async Task GetHospitalRawDataPerCounty()
        {
            string rawData = await CovidDataService.GetCARawCovidHospitalJsonDataByCounty(SelectedCounty?.CountyName);
            await DownloadJsonFile(rawData, $"{SelectedCounty?.CountyName}_CAOpenData_Hospital_{DateTime.Now:yyyy-MM-dd}.json");
        }

        public async Task GetAllCovidRawData()
        {
            var allCountiesRawData = new Dictionary<string, string>();

            foreach (var county in CountyList.Where(county => !allCountiesRawData.ContainsKey(county.CountyName)))
            {
                string countyRawData = await CovidDataService.GetCARawCovidJsonDataByCounty(county.CountyName);
                allCountiesRawData.Add(county.CountyName, countyRawData);
            }

            await DownloadZip(allCountiesRawData, "COVID_Data", "CAOpenData_COVID_All_Counties");
        }

        public async Task GetAllHospitalRawData()
        {
            var allCountiesRawData = new Dictionary<string, string>();

            foreach (var county in CountyList.Where(county => !allCountiesRawData.ContainsKey(county.CountyName)))
            {
                string countyRawData = await CovidDataService.GetCARawCovidHospitalJsonDataByCounty(county.CountyName);
                allCountiesRawData.Add(county.CountyName, countyRawData);
            }

            await DownloadZip(allCountiesRawData, "Hospital_Data", "CAOpenData_Hospital_All_Counties");
        }

        private async Task DownloadZip(Dictionary<string, string> countyData, string zippedFilesName, string zipFileName)
        {
            var zipStream = new MemoryStream();
            var zip = new ZipOutputStream(zipStream);

            foreach (var countyRawData in countyData)
            {
                var fileContent = Encoding.UTF8.GetBytes(countyRawData.Value);

                AddFileToZip($"{countyRawData.Key}_{zippedFilesName}.json", fileContent, zip);
            }

            zip.Close();

            FileHelper fileHelper = new FileHelper(JSRuntime);
            await fileHelper.DownloadFileFromBytes($"{zipFileName}_{DateTime.Now:yyyy-MM-dd}.zip", zipStream.ToArray());
        }

        private static void AddFileToZip(string fileName, byte[] fileContent, ZipOutputStream zip)
        {
            var zipEntry = new ZipEntry(fileName) { DateTime = DateTime.Now, Size = fileContent.Length };
            zip.PutNextEntry(zipEntry);
            zip.Write(fileContent, 0, fileContent.Length);
            zip.CloseEntry();
        }

        private async Task DownloadJsonFile(string jsonData, string fileName)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(jsonData);
            FileHelper fileHelper = new FileHelper(JSRuntime);
            await fileHelper.DownloadFileFromBytes(fileName, bytes);
        }
    }
}
