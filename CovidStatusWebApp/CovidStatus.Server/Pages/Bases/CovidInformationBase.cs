using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidStatus.Server.ConfigurationSettings;
using CovidStatus.Server.Helper;
using CovidStatus.Server.Services.Interfaces;
using CovidStatus.Shared.Entities;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

namespace CovidStatus.Server.Pages.Bases
{
    public class CovidInformationBase : ComponentBase
    {
        [Inject] private ICovidDataService CovidDataService { get; set; }
        public List<CovidData> CovidDataList { get; set; }
        public List<County> CountyList { get; set; }
        public County SelectedCounty { get; set; }

        public List<CriticalDay> CriticalDayList { get; set; }
        public int CriticalDaysCount { get; set; }
        public int DefaultCriticalDaysCount { get; set; }
        public byte DefaultCounty { get; set; }
        public string CriticalDaysMessage { get; set; }

        public DateTime LastUpdateDate { get; set; }
        public SfGrid<CovidData> CovidDataGrid;

        protected override async Task OnInitializedAsync()
        {
            string countyName = "San Bernardino";
            CountyList = await CovidDataService.GetCountyList();
            SelectedCounty = CountyList.FirstOrDefault(x => x.CountyName == countyName);
            DefaultCounty = SelectedCounty?.CountyID ?? 0;

            CriticalDayList = await GetCriticalDayList();
            DefaultCriticalDaysCount = AppConfigurationSettings.CriticalDaysCount;
            CriticalDaysCount = DefaultCriticalDaysCount;

            await GetCriticalDaysMessage(CriticalDaysCount);
            await GetData(countyName, DefaultCriticalDaysCount);
        }

        private async Task GetData(string countyName, int criticalDaysCount)
        {
            await GetCovidData(countyName, criticalDaysCount);
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
    }
}
