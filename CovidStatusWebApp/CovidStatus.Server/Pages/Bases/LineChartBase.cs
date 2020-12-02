using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidStatus.Shared.Entities;
using CovidStatus.Shared.Enum;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts;

namespace CovidStatus.Server.Pages.Bases
{
    public class LineChartBase : ComponentBase
    {
        [Parameter] public List<CovidData> CovidDataList { get; set; }
        [Parameter] public string ChartSeriesName { get; set; }
        [Parameter] public string ChartSeriesName2 { get; set; }
        [Parameter] public string ChartTitle { get; set; }
        [Parameter] public string LineColor { get; set; }
        [Parameter] public string LineColor2 { get; set; }
        [Parameter] public CovidDataTypeEnum CovidDataType { get; set; }


        public List<LineChartModel> ChartData { get; set; }
        public double MaxChartNumber { get; set; }
        public double MinChartNumber { get; set; }
        public double ChartInterval { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (String.IsNullOrEmpty(LineColor))
            {
                LineColor = "#000000";
                LineColor2 = "#808080";
            }

            ChartData = await GetChartData(CovidDataList);
        }

        protected override async Task OnParametersSetAsync()
        {
            ChartData = await GetChartData(CovidDataList);
        }

        public void ChartLoad(ILoadedEventArgs args)
        {
            args.Theme = ChartTheme.Material;
        }

        private async Task<List<LineChartModel>> GetChartData(List<CovidData> covidData)
        {
            var chartData = new List<LineChartModel>();
            foreach (CovidData record in covidData)
            {
                LineChartModel dataPoint = new LineChartModel();
                dataPoint.xValue = record.Date;
                switch (CovidDataType)
                {
                    case CovidDataTypeEnum.NewCases:
                        dataPoint.yValue = record.NewCountConfirmed;
                        break;
                    case CovidDataTypeEnum.CasesMovingAverage:
                        dataPoint.yValue = (double)record.CriticalDaysMovingAverageCases;
                        break;
                    case CovidDataTypeEnum.NewDeaths:
                        dataPoint.yValue = record.NewCountDeaths;
                        break;
                    case CovidDataTypeEnum.DeathsMovingAverage:
                        dataPoint.yValue = (double)record.CriticalDaysMovingAverageDeaths;
                        break;
                    case CovidDataTypeEnum.CasesMovingAveragePerOneThousandPopulation:
                        dataPoint.yValue = (double)record.CriticalDaysMovingCasesPerOneHundredThousand;
                        break;
                    case CovidDataTypeEnum.DeathsMovingAveragePerOneThousandPopulation:
                        dataPoint.yValue = (double)record.CriticalDaysMovingDeathsPerOneHundredThousand;
                        break;
                    case CovidDataTypeEnum.ICUCovidPatients:
                        dataPoint.yValue = record.ICUCovidPatientCount;
                        dataPoint.yValue2 = record.ICUAvailableBedsCount;
                        break;
                    default:
                        dataPoint.yValue = record.NewCountConfirmed;
                        break;
                }

                chartData.Add(dataPoint);
            }

            MaxChartNumber = chartData.OrderByDescending(x => x.yValue).FirstOrDefault()?.yValue ?? 0;
            var bufferMaxChartNumber = MaxChartNumber * 0.10;
            MaxChartNumber = MaxChartNumber + bufferMaxChartNumber;
            var dataMinValue = chartData.OrderBy(x => x.yValue).FirstOrDefault()?.yValue ?? 0;

            if (dataMinValue > 0)
            {
                MinChartNumber = 0;
            }
            else
            {
                MinChartNumber = dataMinValue;
            }

            ChartInterval = Math.Round(MaxChartNumber / 10);
            StateHasChanged();
            return chartData;
        }
    }
}

