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

                decimal? criticalDaysMovingCasesSum = 0;
                decimal? criticalDaysMovingDeathsSum = 0;
                int count = 0;
                foreach (var record in lastcriticalDaysCovidData)
                {
                    criticalDaysMovingCasesSum = criticalDaysMovingCasesSum + record.NewCountConfirmed;
                    criticalDaysMovingDeathsSum = criticalDaysMovingDeathsSum + record.NewCountDeaths;
                    count++;
                }

                decimal? criticalDaysMovingAverageCases = criticalDaysMovingCasesSum / (count == 0 ? 1 : count);
                decimal? criticalDaysMovingAverageDeaths = criticalDaysMovingDeathsSum / (count == 0 ? 1 : count);
                decimal? covidCasesPerOneHundredThousand = (decimal)(criticalDaysMovingAverageCases / ((decimal)selectedCounty.Population / (decimal)100000));
                decimal? covidDeathsPerOneHundredThousand = (decimal)(criticalDaysMovingAverageDeaths / ((decimal)selectedCounty.Population / (decimal)100000));

                covidRecord.CriticalDaysMovingAverageCases = criticalDaysMovingAverageCases;
                covidRecord.CriticalDaysMovingAverageDeaths = criticalDaysMovingAverageDeaths;
                covidRecord.CriticalDaysMovingCasesPerOneHundredThousand = covidCasesPerOneHundredThousand;
                covidRecord.CriticalDaysMovingDeathsPerOneHundredThousand = covidDeathsPerOneHundredThousand;
            }

            //Get rate change based on seven/fourteen day moving
            foreach (var covidRecord in covidRecords)
            {
                decimal? previousDatecriticalDaysMovingAverage = covidRecords.FirstOrDefault(x => x.Date == covidRecord.Date.AddDays(-1))?.CriticalDaysMovingAverageCases;

                decimal? sevenRateChange = 0;
                if (covidRecord.CriticalDaysMovingAverageCases != null && previousDatecriticalDaysMovingAverage != null && previousDatecriticalDaysMovingAverage != 0)
                {
                    sevenRateChange = (decimal)covidRecord.CriticalDaysMovingAverageCases / (decimal)previousDatecriticalDaysMovingAverage;
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
                criticalDaysMovingCasesSum = criticalDaysMovingCasesSum + record.CriticalDaysMovingAverageCases;
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
            selectedCounty.AreCasesRising = selectedCounty.CriticalDaysMovingRateChange >= 0;

            var defaultDateDisplay = selectedCounty.AreCasesRising ? "-" : "N/A";
            var riskLevelList = new List<CountyRiskLevel>();
            riskLevelList.Add(new CountyRiskLevel { RiskLevelOrder = 1, RiskLevel = RiskLevel.Minimal, RiskLevelName = "Minimal", RiskLelvelCasesMin = AppConfigurationSettings.MinimalMin, RiskLelvelCasesMax = AppConfigurationSettings.MinimalMax, EstimateRiskLevelDateDisplay = defaultDateDisplay, EstimateRiskLevelDateQualificationDisplay = defaultDateDisplay, CSSClassBackgroundColor = "minimalBackgroundColor", CSSClassLightBackgroundColor = "minimalBackgroundLightColor" });
            riskLevelList.Add(new CountyRiskLevel { RiskLevelOrder = 2, RiskLevel = RiskLevel.Moderate, RiskLevelName = "Moderate", RiskLelvelCasesMin = AppConfigurationSettings.ModerateMin, RiskLelvelCasesMax = AppConfigurationSettings.ModerateMax, EstimateRiskLevelDateDisplay = defaultDateDisplay, EstimateRiskLevelDateQualificationDisplay = defaultDateDisplay, CSSClassBackgroundColor = "moderateBackgroundColor", CSSClassLightBackgroundColor = "moderateBackgroundLightColor" });
            riskLevelList.Add(new CountyRiskLevel { RiskLevelOrder = 3, RiskLevel = RiskLevel.Substantial, RiskLevelName = "Substantial", RiskLelvelCasesMin = AppConfigurationSettings.SubstantialMin, RiskLelvelCasesMax = AppConfigurationSettings.SubstantialMax, EstimateRiskLevelDateDisplay = defaultDateDisplay, EstimateRiskLevelDateQualificationDisplay = defaultDateDisplay, CSSClassBackgroundColor = "substantialBackgroundColor", CSSClassLightBackgroundColor = "substantialBackgroundLightColor" });
            riskLevelList.Add(new CountyRiskLevel { RiskLevelOrder = 4, RiskLevel = RiskLevel.Widespread, RiskLevelName = "Widespread", RiskLelvelCasesMin = AppConfigurationSettings.WidespreadMin, RiskLelvelCasesMax = AppConfigurationSettings.WidespreadMax, EstimateRiskLevelDateDisplay = defaultDateDisplay, EstimateRiskLevelDateQualificationDisplay = defaultDateDisplay, CSSClassBackgroundColor = "widespreadBackgroundColor", CSSClassLightBackgroundColor = "widespreadBackgroundLightColor" });

            foreach (var countyRiskLevel in riskLevelList)
            {
                PopulateRiskLevel(countyRiskLevel, selectedCounty.CriticalDaysMovingCasesPerOneHundredThousandAverage, selectedCounty.CriticalDaysMovingRateChange, lastUpdateDate);
            }

            selectedCounty.CurrentRiskLevel = riskLevelList.FirstOrDefault(x => x.IsCurrentRiskLevel) ?? new CountyRiskLevel { RiskLevel = RiskLevel.Widespread };

            //California guideline date requirement
            foreach (var countyRiskLevel in riskLevelList.OrderByDescending(x => x.RiskLevelOrder))
            {
                if (selectedCounty.AreCasesRising && countyRiskLevel.IsFutureRiskLevel) continue;

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
            if (criticalDaysMovingCasesPerOneHundredThousandAverage < countyRiskLevel.RiskLelvelCasesMin)
            {
                countyRiskLevel.IsPassedRiskLevel = true;
            }
            if (criticalDaysMovingCasesPerOneHundredThousandAverage > countyRiskLevel.RiskLelvelCasesMax)
            {
                countyRiskLevel.IsFutureRiskLevel = true;
            }

            DateTime? riskLevelDate = EstimateCountyRiskLevelDate(countyRiskLevel, criticalDaysMovingCasesPerOneHundredThousandAverage, criticalDaysMovingRateChange);
            countyRiskLevel.EstimateRiskLevelDate = riskLevelDate;
            string riskLevelDateDisplay = riskLevelDate != null ? $"{riskLevelDate:MMMM d, yyyy}" : criticalDaysMovingRateChange >= 0 ? "-" : "N/A";
            countyRiskLevel.EstimateRiskLevelDateDisplay = riskLevelDateDisplay;
            countyRiskLevel.EstimateRiskLevelDateQualification = riskLevelDate;
            countyRiskLevel.EstimateRiskLevelDateQualificationDisplay = riskLevelDateDisplay;
        }

        private DateTime? EstimateCountyRiskLevelDate(CountyRiskLevel countyRiskLevel, decimal? criticalDaysMovingCasesPerOneHundredThousandAverage, 
            decimal? criticalDaysMovingRateChange)
        {
            bool areCasesRising = criticalDaysMovingRateChange >= 0;

            DateTime countyRiskLevelDate = DateTime.Today;

            if (areCasesRising)
            {
                if (countyRiskLevel.IsPassedRiskLevel || countyRiskLevel.IsCurrentRiskLevel)
                {
                    countyRiskLevelDate = EstimateCountyRiskLevelDateRisingCases(criticalDaysMovingCasesPerOneHundredThousandAverage, criticalDaysMovingRateChange, countyRiskLevel.RiskLelvelCasesMin);
                }
                //If cases are rising and this is a less restrictive level, return null
                if(countyRiskLevel.IsFutureRiskLevel)
                {
                    return null;
                }
            }
            else
            {
                countyRiskLevelDate = EstimateCountyRiskLevelDateDecliningCases(criticalDaysMovingCasesPerOneHundredThousandAverage, criticalDaysMovingRateChange, countyRiskLevel.RiskLelvelCasesMax);
            }

            return countyRiskLevelDate;
        }

        private DateTime EstimateCountyRiskLevelDateRisingCases(decimal? criticalDaysMovingCasesPerOneHundredThousandAverage,
            decimal? criticalDaysMovingRateChange, decimal riskLevelCasesMin)
        {
            DateTime countyRiskLevelDate = DateTime.Today;
            decimal? casesPerOneHundredThousandAverage = criticalDaysMovingCasesPerOneHundredThousandAverage;
            int daysToAdd = 0;

            while (casesPerOneHundredThousandAverage < riskLevelCasesMin)
            {
                var casesChange = (decimal)(casesPerOneHundredThousandAverage * (decimal)criticalDaysMovingRateChange);
                casesPerOneHundredThousandAverage += casesChange;
                daysToAdd++;
            }

            return countyRiskLevelDate.AddDays(daysToAdd);
        }

        private DateTime EstimateCountyRiskLevelDateDecliningCases(decimal? criticalDaysMovingCasesPerOneHundredThousandAverage,
            decimal? criticalDaysMovingRateChange, decimal riskLelvelCasesMax)
        {
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
