using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidStatus.Server.Helper;
using CovidStatus.Server.Services.Interfaces;
using CovidStatus.Shared.Entities;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;

namespace CovidStatus.Server.Pages.Bases
{
    public class CovidStatusBase : ComponentBase
    {
        [Inject] private ICovidDataService CovidDataService { get; set; }
        public List<CovidData> CovidDataList { get; set; }
        public List<County> CountyList { get; set; }
        public County SelectedCounty { get; set; }
        public byte DefaultCounty { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public SfGrid<CovidData> CovidDataGrid;

        protected override async Task OnInitializedAsync()
        {
            string countyName = "San Bernardino";
            CountyList = await CovidDataService.GetCountyList();
            SelectedCounty = CountyList.FirstOrDefault(x => x.CountyName == countyName);
            DefaultCounty = SelectedCounty?.CountyID ?? 0;

            await GetCovidData(countyName);
        }

        private async Task GetCovidData(string countyName)
        {
            var covidRecords = await CovidDataService.GetCovidDataByCounty(countyName);

            DateTime lastUpdate = covidRecords.Select(x => x.Date).OrderByDescending(x => x.Date).FirstOrDefault();
            LastUpdateDate = lastUpdate;

            var covidDataHelper = new CovidDataHelper();
            covidDataHelper.PopulateAggregatesCovidData(covidRecords, SelectedCounty);
            covidDataHelper.PopulateCountyAggregates(covidRecords, SelectedCounty, lastUpdate);

            CovidDataList = covidRecords;

            StateHasChanged();
        }

        public async Task OnCountyChanged(Syncfusion.Blazor.DropDowns.ChangeEventArgs<byte> args)
        {
            var countyID = args.Value;
            SelectedCounty = CountyList.FirstOrDefault(x => x.CountyID == countyID);
            await GetCovidData(SelectedCounty?.CountyName);
        }

        public async Task ExcelExport()
        {
            var exportProperties = new ExcelExportProperties();
            exportProperties.IncludeHiddenColumn = true;
            await CovidDataGrid.ExcelExport(exportProperties);
        }
    }
}
