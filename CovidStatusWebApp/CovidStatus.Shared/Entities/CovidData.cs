using System;

namespace CovidStatus.Shared.Entities
{
    public class CovidData
    {
        public int ID { get; set; }
        public double TotalCountConfirmed { get; set; }
        public int NewCountDeaths { get; set; }
        public double TotalCountDeaths { get; set; }
        public double Rank { get; set; }
        public string County { get; set; }
        public int NewCountConfirmed { get; set; }
        public DateTime Date { get; set; }
        public decimal? CriticalDaysMovingRateChange { get; set; }
        public decimal? CriticalDaysMovingAverageCases { get; set; }
        public decimal? CriticalDaysMovingCasesPerOneHundredThousand { get; set; }
        public decimal? CriticalDaysMovingDeathsPerOneHundredThousand { get; set; }
        public decimal? CriticalDaysMovingAverageDeaths { get; set; }
        public double ICUCovidPatientCount { get; set; }
        public double ICUAvailableBedsCount { get; set; }

    }
}