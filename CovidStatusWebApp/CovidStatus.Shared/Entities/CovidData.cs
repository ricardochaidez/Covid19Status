﻿using System;

namespace CovidStatus.Shared.Entities
{
    public class CovidData
    {
        public int ID { get; set; }
        public double TotalCountConfirmed { get; set; }
        public double NewCountDeaths { get; set; }
        public double TotalCountDeaths { get; set; }        
        public string County { get; set; }
        public double NewCountConfirmed { get; set; }
        public DateTime Date { get; set; }
        public decimal? CriticalDaysMovingCasesRateChange { get; set; }
        public decimal? CriticalDaysMovingAverageCases { get; set; }
        public decimal? CriticalDaysMovingAverageAvailableICUBeds { get; set; }
        public decimal? CriticalDaysMovingAvailableICUBedsRateChange { get; set; }
        public decimal? CriticalDaysMovingCasesPerOneHundredThousand { get; set; }
        public decimal? CriticalDaysMovingDeathsPerOneHundredThousand { get; set; }
        public decimal? CriticalDaysMovingAverageDeaths { get; set; }
        public decimal ICUCovidPatientCount { get; set; }
        public decimal ICUAvailableBedsCount { get; set; }

    }
}