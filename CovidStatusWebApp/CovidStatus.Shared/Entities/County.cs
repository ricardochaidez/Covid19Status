using System.Collections.Generic;
using CovidStatus.Shared.Enum;

namespace CovidStatus.Shared.Entities
{
    public class County
    {
        public byte CountyID { get; set; }
        public string CountyName { get; set; }
        public int Population { get; set; }
        public decimal? CriticalDaysMovingCasesAverage { get; set; }
        public decimal? CriticalDaysMovingAvailableICUBedsAverage { get; set; }
        public decimal? CriticalDaysMovingCasesPerOneHundredThousandAverage { get; set; }
        public decimal? CriticalDaysMovingCasesRateChange { get; set; }
        public decimal? CriticalDaysMovingAvailableICUBedsRateChange { get; set; }
        public List<CountyRiskLevel> RiskLevels { get; set; }
        public CountyRiskLevel CurrentRiskLevel { get; set; }
        public bool AreCasesRising { get; set; }
        public bool AreAvailableICUBedsDeclining { get; set; }
        public bool AreThereAnyAvailableICUBedsNow { get; set; }
    }
}