using System;
using System.Collections.Generic;
using System.Text;

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
        public decimal? RateChange { get; set; }
        public decimal? SevenDayMovingAverage { get; set; }
        public decimal? CovidCasesPerOneHundredThousand { get; set; }
    }
}
