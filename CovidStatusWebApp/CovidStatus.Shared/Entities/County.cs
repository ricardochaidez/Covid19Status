using System.Collections.Generic;
using CovidStatus.Shared.Enum;

namespace CovidStatus.Shared.Entities
{
    public class County
    {
        public byte CountyID { get; set; }
        public string CountyName { get; set; }
        public int Population { get; set; }
        public decimal? SevenDayMovingCasesAverage { get; set; }
        public decimal? SevenDayMovingCasesPerOneHundredThousandAverage { get; set; }
        public decimal? SevenDayMovingRateChange { get; set; }
        public List<CountyRiskLevel> RiskLevels { get; set; }
        public CountyRiskLevel CurrentRiskLevel { get; set; }
    }
}
