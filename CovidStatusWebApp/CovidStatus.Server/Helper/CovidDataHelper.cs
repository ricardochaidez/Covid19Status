using System;
using System.Collections.Generic;
using System.Linq;
using CovidStatus.Server.ConfigurationSettings;
using CovidStatus.Shared.Entities;
using CovidStatus.Shared.Enum;

namespace CovidStatus.Server.Helper
{
    public class CovidDataHelper
    {
        public void PopulateAggregatesCovidData(List<CovidData> covidRecords, County selectedCounty, int criticalDaysCount)
        {
            //Get seven day moving
            foreach (var covidRecord in covidRecords)
            {
                var lastcriticalDaysCovidData = covidRecords.Where(x => x.Date > covidRecord.Date.AddDays(-(criticalDaysCount)) && x.Date <= covidRecord.Date).ToList();

                decimal? criticalDaysMovingSum = 0;
                int count = 0;
                foreach (var record in lastcriticalDaysCovidData)
                {
                    criticalDaysMovingSum = criticalDaysMovingSum + record.NewCountConfirmed;
                    count++;
                }

                decimal? criticalDaysMovingAverage = criticalDaysMovingSum / (count == 0 ? 1 : count);
                decimal? covidCasesPerOneHundredThousand = (decimal)(criticalDaysMovingAverage / ((decimal)selectedCounty.Population / (decimal)100000));

                covidRecord.CriticalDaysMovingAverage = criticalDaysMovingAverage;
                covidRecord.CriticalDaysMovingCasesPerOneHundredThousand = covidCasesPerOneHundredThousand;
            }

            //Get rate change based on seven/fourteen day moving
            foreach (var covidRecord in covidRecords)
            {
                decimal? previousDatecriticalDaysMovingAverage = covidRecords.FirstOrDefault(x => x.Date == covidRecord.Date.AddDays(-1))?.CriticalDaysMovingAverage;

                decimal? sevenRateChange = 0;
                if (covidRecord.CriticalDaysMovingAverage != null && previousDatecriticalDaysMovingAverage != null && previousDatecriticalDaysMovingAverage != 0)
                {
                    sevenRateChange = (decimal)covidRecord.CriticalDaysMovingAverage / (decimal)previousDatecriticalDaysMovingAverage;
                    sevenRateChange = sevenRateChange - 1;
                }
                covidRecord.CriticalDaysMovingRateChange = sevenRateChange;
            }
        }

        public void PopulateCountyAggregates(List<CovidData> covidRecords, County selectedCounty, DateTime lastUpdateDate, int criticalDaysCount)
        {
            //Get seven day moving averages
            var criticalDaysCovidData = covidRecords.Where(x => x.Date > lastUpdateDate.AddDays(-(criticalDaysCount)) && x.Date <= lastUpdateDate).ToList();

            decimal? criticalDaysMovingCasesPerOneHundredThousandSum = 0;
            decimal? criticalDaysMovingRateChangeSum = 0;
            decimal? criticalDaysMovingCasesSum = 0;
            int criticalDayscount = 0;
            foreach (var record in criticalDaysCovidData)
            {
                criticalDaysMovingCasesPerOneHundredThousandSum = criticalDaysMovingCasesPerOneHundredThousandSum + record.CriticalDaysMovingCasesPerOneHundredThousand;
                criticalDaysMovingRateChangeSum = criticalDaysMovingRateChangeSum + record.CriticalDaysMovingRateChange;
                criticalDaysMovingCasesSum = criticalDaysMovingCasesSum + record.CriticalDaysMovingAverage;
                criticalDayscount++;
            }

            decimal? criticalDaysMovingCasesPerOneHundredThousandAverage = criticalDaysMovingCasesPerOneHundredThousandSum / (criticalDayscount == 0 ? 1 : criticalDayscount);
            decimal? criticalDaysMovingRateChangeAverage = criticalDaysMovingRateChangeSum / (criticalDayscount == 0 ? 1 : criticalDayscount);
            decimal? criticalDaysMovingCasesAverage = criticalDaysMovingCasesSum / (criticalDayscount == 0 ? 1 : criticalDayscount);

            selectedCounty.CriticalDaysMovingCasesPerOneHundredThousandAverage = criticalDaysMovingCasesPerOneHundredThousandAverage;
            selectedCounty.CriticalDaysMovingRateChange = criticalDaysMovingRateChangeAverage;
            selectedCounty.CriticalDaysMovingCasesAverage = criticalDaysMovingCasesAverage;
            selectedCounty.RiskLevels = GetCountyRiskLevels(selectedCounty, lastUpdateDate);
        }

        private List<CountyRiskLevel> GetCountyRiskLevels(County selectedCounty, DateTime lastUpdateDate)
        {
            var defaultDateDisplay = selectedCounty.CriticalDaysMovingRateChange >= 0 ? "Cases are rising" : "N/A";
            var riskLevelList = new List<CountyRiskLevel>();
            riskLevelList.Add(new CountyRiskLevel { RiskLevelOrder = 1, RiskLevel = RiskLevel.Minimal, RiskLelvelCasesMin = AppConfigurationSettings.MinimalMin, RiskLelvelCasesMax = AppConfigurationSettings.MinimalMax, EstimateRiskLevelDateDisplay = defaultDateDisplay, EstimateRiskLevelDateQualificationDisplay = defaultDateDisplay });
            riskLevelList.Add(new CountyRiskLevel { RiskLevelOrder = 2, RiskLevel = RiskLevel.Moderate, RiskLelvelCasesMin = AppConfigurationSettings.ModerateMin, RiskLelvelCasesMax = AppConfigurationSettings.ModerateMax, EstimateRiskLevelDateDisplay = defaultDateDisplay, EstimateRiskLevelDateQualificationDisplay = defaultDateDisplay });
            riskLevelList.Add(new CountyRiskLevel { RiskLevelOrder = 3, RiskLevel = RiskLevel.Substantial, RiskLelvelCasesMin = AppConfigurationSettings.SubstantialMin, RiskLelvelCasesMax = AppConfigurationSettings.SubstantialMax, EstimateRiskLevelDateDisplay = defaultDateDisplay, EstimateRiskLevelDateQualificationDisplay = defaultDateDisplay });
            riskLevelList.Add(new CountyRiskLevel { RiskLevelOrder = 4, RiskLevel = RiskLevel.Widespread, RiskLelvelCasesMin = AppConfigurationSettings.WidespreadMin, RiskLelvelCasesMax = AppConfigurationSettings.WidespreadMax, EstimateRiskLevelDateDisplay = defaultDateDisplay, EstimateRiskLevelDateQualificationDisplay = defaultDateDisplay });

            foreach (var countyRiskLevel in riskLevelList)
            {
                PopulateRiskLevel(countyRiskLevel, selectedCounty.CriticalDaysMovingCasesPerOneHundredThousandAverage, selectedCounty.CriticalDaysMovingRateChange, lastUpdateDate);
            }

            selectedCounty.CurrentRiskLevel = riskLevelList.FirstOrDefault(x => x.IsCurrentRiskLevel) ?? new CountyRiskLevel { RiskLevel = RiskLevel.Widespread };

            //California guideline date requirement
            foreach (var countyRiskLevel in riskLevelList.OrderByDescending(x => x.RiskLevelOrder))
            {
                var previousRiskLevel = riskLevelList.FirstOrDefault(x => x.RiskLevelOrder == (countyRiskLevel.RiskLevelOrder + 1));
                if (previousRiskLevel == null) continue;

                DateTime? estimateRiskLevelDate = countyRiskLevel.EstimateRiskLevelDate > previousRiskLevel.EstimateRiskLevelDateQualification ? countyRiskLevel.EstimateRiskLevelDate : previousRiskLevel.EstimateRiskLevelDateQualification;

                //Use current county's risk level date for previous levels and do not add more days to estimate
                int daysToAdd = 0;
                if (previousRiskLevel.RiskLevelOrder > selectedCounty.CurrentRiskLevel.RiskLevelOrder)
                {
                    estimateRiskLevelDate = countyRiskLevel.EstimateRiskLevelDateQualification;
                }
                else
                {
                    daysToAdd = AppConfigurationSettings.CaliforniaWaitTimeRequirement;
                }
                if (estimateRiskLevelDate == null) continue;

                var estimatedRiskLevelDateQualification = estimateRiskLevelDate.Value.AddDays(daysToAdd); //California requires to meet risk level criteria for 2 weeks, county must stay in that level for at least 3 weeks before moving to new level
                countyRiskLevel.EstimateRiskLevelDateQualification = estimatedRiskLevelDateQualification;
                countyRiskLevel.EstimateRiskLevelDateQualificationDisplay = $"{estimatedRiskLevelDateQualification:MMMM d, yyyy}";
            }

            return riskLevelList;
        }

        private void PopulateRiskLevel(CountyRiskLevel countyRiskLevel, decimal? criticalDaysMovingCasesPerOneHundredThousandAverage, decimal? criticalDaysMovingRateChange, DateTime latestUpdateDate)
        {
            if (criticalDaysMovingCasesPerOneHundredThousandAverage >= countyRiskLevel.RiskLelvelCasesMin &&
                criticalDaysMovingCasesPerOneHundredThousandAverage <= countyRiskLevel.RiskLelvelCasesMax)
            {
                countyRiskLevel.IsCurrentRiskLevel = true;
            }
            DateTime? riskLevelDate = EstimateCountyRiskLevelDate(criticalDaysMovingCasesPerOneHundredThousandAverage, criticalDaysMovingRateChange, countyRiskLevel.RiskLelvelCasesMax);
            countyRiskLevel.EstimateRiskLevelDate = riskLevelDate;
            string riskLevelDateDisplay = riskLevelDate != null ? $"{riskLevelDate:MMMM d, yyyy}" : criticalDaysMovingRateChange >= 0 ? "Cases are rising" : "N/A";
            countyRiskLevel.EstimateRiskLevelDateDisplay = riskLevelDateDisplay;
            countyRiskLevel.EstimateRiskLevelDateQualification = riskLevelDate;
            countyRiskLevel.EstimateRiskLevelDateQualificationDisplay = riskLevelDateDisplay;
        }

        private DateTime? EstimateCountyRiskLevelDate(decimal? criticalDaysMovingCasesPerOneHundredThousandAverage, decimal? criticalDaysMovingRateChange, decimal riskLelvelCasesMax)
        {
            //Cases are increasing, return
            if (criticalDaysMovingRateChange >= 0)
            {
                return null;
            }

            DateTime countyRiskLevelDate = DateTime.Today;
            decimal? casesPerOneHundredThousandAverage = criticalDaysMovingCasesPerOneHundredThousandAverage;
            int daysToAdd = 0;

            while (casesPerOneHundredThousandAverage > riskLelvelCasesMax)
            {
                var casesChange = (decimal)(casesPerOneHundredThousandAverage * (decimal)criticalDaysMovingRateChange);
                casesPerOneHundredThousandAverage += casesChange;
                daysToAdd++;
            }

            return countyRiskLevelDate.AddDays(daysToAdd);
        }
    }
}
