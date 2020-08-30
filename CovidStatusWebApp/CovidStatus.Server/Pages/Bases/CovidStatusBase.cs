using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidStatus.Server.Services.Interfaces;
using CovidStatus.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace CovidStatus.Server.Pages.Bases
{
    public class CovidStatusBase : ComponentBase
    {
        [Inject] private ICovidDataService CovidDataService { get; set; }
        public List<CovidData> CovidDataList { get; set; }
        public List<County> CountyList { get; set; }
        public County SelectedCounty { get; set; }
        public byte DefaultCounty { get; set; }

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

            //Get seven day moving average
            foreach (var covidRecord in covidRecords)
            {
                var lastSevenDayCovidData = covidRecords.Where(x => x.Date > covidRecord.Date.AddDays(-7) && x.Date <= covidRecord.Date).ToList();

                decimal? sevenDayMovingAverage = null;
                decimal? sevenDayMovingSum = 0;
                int count = 0;
                foreach (var record in lastSevenDayCovidData)
                {
                    sevenDayMovingSum = sevenDayMovingSum + record.NewCountConfirmed;
                    count++;
                }

                sevenDayMovingAverage = sevenDayMovingSum / (count == 0 ? 1 : count);
                covidRecord.SevenDayMovingAverage = sevenDayMovingAverage;
                covidRecord.CovidCasesPerOneHundredThousand = covidRecord.SevenDayMovingAverage / (SelectedCounty.Population / 100000);
            }

            //Get rate change based on seven day moving average
            foreach (var covidRecord in covidRecords)
            {
                decimal? previousDateSevenDayMovingAverage = covidRecords.FirstOrDefault(x => x.Date == covidRecord.Date.AddDays(-1))?.SevenDayMovingAverage;

                decimal? rateChange = null;
                if (covidRecord.SevenDayMovingAverage != null && previousDateSevenDayMovingAverage != null && previousDateSevenDayMovingAverage != 0)
                {
                    rateChange = (decimal)covidRecord.SevenDayMovingAverage / (decimal)previousDateSevenDayMovingAverage;
                    rateChange = rateChange - 1;
                }

                covidRecord.RateChange = rateChange;
            }

            CovidDataList = covidRecords;

            StateHasChanged();
        }

        public async Task OnCountyChanged(Syncfusion.Blazor.DropDowns.ChangeEventArgs<byte> args)
        {
            var countyID = args.Value;
            SelectedCounty = CountyList.FirstOrDefault(x => x.CountyID == countyID);
            await GetCovidData(SelectedCounty?.CountyName);
        }
    }
}
