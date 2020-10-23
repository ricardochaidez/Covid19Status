using System;
using CovidStatus.Shared.Enum;

namespace CovidStatus.Shared.Entities
{
    public class CountyRiskLevel
    {
        public RiskLevel RiskLevel { get; set; }
        public string RiskLevelName { get; set; }
        public byte RiskLevelOrder { get; set; }
        public decimal RiskLelvelCasesMin { get; set; }
        public decimal RiskLelvelCasesMax { get; set; }
        public bool IsCurrentRiskLevel { get; set; }
        public bool IsPassedRiskLevel { get; set; }
        public bool IsFutureRiskLevel { get; set; }
        
        public DateTime? EstimateRiskLevelDate { get; set; }
        public DateTime? EstimateRiskLevelDateQualification { get; set; }
        public string EstimateRiskLevelDateDisplay { get; set; }
        public string EstimateRiskLevelDateQualificationDisplay { get; set; }

        public string CSSClassBackgroundColor { get; set; }
        public string CSSClassLightBackgroundColor { get; set; }
    }
}
