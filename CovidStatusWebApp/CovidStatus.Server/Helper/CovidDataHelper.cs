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
                List<CovidData> lastCriticalDaysCovidData = covidRecords.Where(x => x.Date > covidRecord.Date.AddDays(-(criticalDaysCount)) && x.Date <= covidRecord.Date).ToList();

                decimal? criticalDaysMovingCasesSum = 0;
                decimal? criticalDaysMovingDeathsSum = 0;
                decimal? criticalDaysMovingAvailableICUBedsSum = 0;
                int count = 0;
                foreach (var record in lastCriticalDaysCovidData)
                {
                    criticalDaysMovingCasesSum = criticalDaysMovingCasesSum + record.NewCountConfirmed;
                    criticalDaysMovingDeathsSum = criticalDaysMovingDeathsSum + record.NewCountDeaths;
                    criticalDaysMovingAvailableICUBedsSum = criticalDaysMovingAvailableICUBedsSum + record.ICUAvailableBedsCount;
                    count++;
                }

                decimal? criticalDaysMovingAverageCases = criticalDaysMovingCasesSum / (count == 0 ? 1 : count);
                decimal? criticalDaysMovingAverageDeaths = criticalDaysMovingDeathsSum / (count == 0 ? 1 : count);
                decimal? criticalDaysMovingAverageAvailableICUBeds = criticalDaysMovingAvailableICUBedsSum / (count == 0 ? 1 : count);
                decimal? covidCasesPerOneHundredThousand = (decimal)(criticalDaysMovingAverageCases / ((decimal)selectedCounty.Population / (decimal)100000));
                decimal? covidDeathsPerOneHundredThousand = (decimal)(criticalDaysMovingAverageDeaths / ((decimal)selectedCounty.Population / (decimal)100000));

                covidRecord.CriticalDaysMovingAverageCases = criticalDaysMovingAverageCases;
                covidRecord.CriticalDaysMovingAverageDeaths = criticalDaysMovingAverageDeaths;
                covidRecord.CriticalDaysMovingAverageAvailableICUBeds = criticalDaysMovingAverageAvailableICUBeds;
                covidRecord.CriticalDaysMovingCasesPerOneHundredThousand = covidCasesPerOneHundredThousand;
                covidRecord.CriticalDaysMovingDeathsPerOneHundredThousand = covidDeathsPerOneHundredThousand;
            }

            //Get rate change based on critical days moving
            foreach (CovidData covidRecord in covidRecords)
            {
                //Cases
                decimal? previousDatecriticalDaysMovingAverageCases = covidRecords.FirstOrDefault(x => x.Date == covidRecord.Date.AddDays(-1))?.CriticalDaysMovingAverageCases;

                decimal? criticalCasesRateChange = 0;
                if (covidRecord.CriticalDaysMovingAverageCases != null && previousDatecriticalDaysMovingAverageCases != null && previousDatecriticalDaysMovingAverageCases != 0)
                {
                    criticalCasesRateChange = (decimal)covidRecord.CriticalDaysMovingAverageCases / (decimal)previousDatecriticalDaysMovingAverageCases;
                    criticalCasesRateChange = criticalCasesRateChange - 1;
                }
                covidRecord.CriticalDaysMovingCasesRateChange = criticalCasesRateChange;

                //Available ICU Beds
                decimal? previousDatecriticalDaysMovingAverageAvailableICUBeds = covidRecords.FirstOrDefault(x => x.Date == covidRecord.Date.AddDays(-1))?.CriticalDaysMovingAverageAvailableICUBeds;

                decimal? criticalAvailableICUBedsRateChange = 0;
                if (covidRecord.CriticalDaysMovingAverageAvailableICUBeds != null && previousDatecriticalDaysMovingAverageAvailableICUBeds != null && previousDatecriticalDaysMovingAverageAvailableICUBeds != 0)
                {
                    criticalAvailableICUBedsRateChange = (decimal)covidRecord.CriticalDaysMovingAverageAvailableICUBeds / (decimal)previousDatecriticalDaysMovingAverageAvailableICUBeds;
                    criticalAvailableICUBedsRateChange = criticalAvailableICUBedsRateChange - 1;
                }
                covidRecord.CriticalDaysMovingAvailableICUBedsRateChange = criticalAvailableICUBedsRateChange;
            }
        }

        public void PopulateCountyAggregates(List<CovidData> covidRecords, County selectedCounty, DateTime lastUpdateDate, int criticalDaysCount)
        {
            //Get critical day moving averages
            List<CovidData> criticalDaysCovidData = covidRecords.Where(x => x.Date > lastUpdateDate.AddDays(-(criticalDaysCount)) && x.Date <= lastUpdateDate).ToList();

            decimal? criticalDaysMovingCasesPerOneHundredThousandSum = 0;
            decimal? criticalDaysMovingRateChangeSum = 0;
            decimal? criticalDaysMovingCasesSum = 0;
            decimal? criticalDaysMovingAvailableICUBedsSum = 0;
            decimal? criticalDaysMovingAvailableICUBedsRateChangeSum = 0;
            int criticalDayscount = 0;

            foreach (CovidData record in criticalDaysCovidData)
            {
                criticalDaysMovingCasesPerOneHundredThousandSum = criticalDaysMovingCasesPerOneHundredThousandSum + record.CriticalDaysMovingCasesPerOneHundredThousand;
                criticalDaysMovingRateChangeSum = criticalDaysMovingRateChangeSum + record.CriticalDaysMovingCasesRateChange;
                criticalDaysMovingCasesSum = criticalDaysMovingCasesSum + record.CriticalDaysMovingAverageCases;
                criticalDaysMovingAvailableICUBedsSum = criticalDaysMovingAvailableICUBedsSum + record.CriticalDaysMovingAverageAvailableICUBeds;
                criticalDaysMovingAvailableICUBedsRateChangeSum = criticalDaysMovingAvailableICUBedsRateChangeSum + record.CriticalDaysMovingAvailableICUBedsRateChange;
                criticalDayscount++;
            }

            decimal? criticalDaysMovingCasesPerOneHundredThousandAverage = criticalDaysMovingCasesPerOneHundredThousandSum / (criticalDayscount == 0 ? 1 : criticalDayscount);
            decimal? criticalDaysMovingRateChangeAverage = criticalDaysMovingRateChangeSum / (criticalDayscount == 0 ? 1 : criticalDayscount);
            decimal? criticalDaysMovingCasesAverage = criticalDaysMovingCasesSum / (criticalDayscount == 0 ? 1 : criticalDayscount);
            decimal? criticalDaysMovingAvailableICUBedsAverage = criticalDaysMovingAvailableICUBedsSum / (criticalDayscount == 0 ? 1 : criticalDayscount);
            decimal? criticalDaysMovingAvailableICUBedsRateChangeAverage = criticalDaysMovingAvailableICUBedsRateChangeSum / (criticalDayscount == 0 ? 1 : criticalDayscount);

            selectedCounty.CriticalDaysMovingCasesPerOneHundredThousandAverage = criticalDaysMovingCasesPerOneHundredThousandAverage;
            selectedCounty.CriticalDaysMovingCasesRateChange = criticalDaysMovingRateChangeAverage;
            selectedCounty.CriticalDaysMovingCasesAverage = criticalDaysMovingCasesAverage;
            selectedCounty.CriticalDaysMovingAvailableICUBedsAverage = criticalDaysMovingAvailableICUBedsAverage;
            selectedCounty.CriticalDaysMovingAvailableICUBedsRateChange = criticalDaysMovingAvailableICUBedsRateChangeAverage;
            selectedCounty.RiskLevels = GetCountyRiskLevels(selectedCounty, lastUpdateDate);
            selectedCounty.AreThereAnyAvailableICUBedsNow = criticalDaysCovidData.FirstOrDefault(x => x.Date == lastUpdateDate)?.ICUAvailableBedsCount > 0;
        }

        private List<CountyRiskLevel> GetCountyRiskLevels(County selectedCounty, DateTime lastUpdateDate)
        {
            selectedCounty.AreCasesRising = selectedCounty.CriticalDaysMovingCasesRateChange >= 0;
            selectedCounty.AreAvailableICUBedsDeclining = selectedCounty.CriticalDaysMovingAvailableICUBedsRateChange < 0;

            var defaultRiskLevelDateDisplay = selectedCounty.AreCasesRising ? "-" : "N/A";
            var defaultZeroAvailableICUBedsDateDisplay = "N/A";

            var riskLevelList = new List<CountyRiskLevel>();
            riskLevelList.Add(new CountyRiskLevel { RiskLevelOrder = 1, RiskLevel = RiskLevel.Minimal, RiskLevelName = "Minimal", RiskLelvelCasesMin = AppConfigurationSettings.MinimalMin, RiskLelvelCasesMax = AppConfigurationSettings.MinimalMax, EstimateRiskLevelDateDisplay = defaultRiskLevelDateDisplay, EstimateRiskLevelDateQualificationDisplay = defaultRiskLevelDateDisplay, EstimateZeroAvailableICUBedsDateDisplay = defaultZeroAvailableICUBedsDateDisplay, CSSClassBackgroundColor = "minimalBackgroundColor", CSSClassLightBackgroundColor = "minimalBackgroundLightColor" });
            riskLevelList.Add(new CountyRiskLevel { RiskLevelOrder = 2, RiskLevel = RiskLevel.Moderate, RiskLevelName = "Moderate", RiskLelvelCasesMin = AppConfigurationSettings.ModerateMin, RiskLelvelCasesMax = AppConfigurationSettings.ModerateMax, EstimateRiskLevelDateDisplay = defaultRiskLevelDateDisplay, EstimateRiskLevelDateQualificationDisplay = defaultRiskLevelDateDisplay, EstimateZeroAvailableICUBedsDateDisplay = defaultZeroAvailableICUBedsDateDisplay, CSSClassBackgroundColor = "moderateBackgroundColor", CSSClassLightBackgroundColor = "moderateBackgroundLightColor" });
            riskLevelList.Add(new CountyRiskLevel { RiskLevelOrder = 3, RiskLevel = RiskLevel.Substantial, RiskLevelName = "Substantial", RiskLelvelCasesMin = AppConfigurationSettings.SubstantialMin, RiskLelvelCasesMax = AppConfigurationSettings.SubstantialMax, EstimateRiskLevelDateDisplay = defaultRiskLevelDateDisplay, EstimateRiskLevelDateQualificationDisplay = defaultRiskLevelDateDisplay, EstimateZeroAvailableICUBedsDateDisplay = defaultZeroAvailableICUBedsDateDisplay, CSSClassBackgroundColor = "substantialBackgroundColor", CSSClassLightBackgroundColor = "substantialBackgroundLightColor" });
            riskLevelList.Add(new CountyRiskLevel { RiskLevelOrder = 4, RiskLevel = RiskLevel.Widespread, RiskLevelName = "Widespread", RiskLelvelCasesMin = AppConfigurationSettings.WidespreadMin, RiskLelvelCasesMax = AppConfigurationSettings.WidespreadMax, EstimateRiskLevelDateDisplay = defaultRiskLevelDateDisplay, EstimateRiskLevelDateQualificationDisplay = defaultRiskLevelDateDisplay, EstimateZeroAvailableICUBedsDateDisplay = defaultZeroAvailableICUBedsDateDisplay, CSSClassBackgroundColor = "widespreadBackgroundColor", CSSClassLightBackgroundColor = "widespreadBackgroundLightColor" });

            foreach (var countyRiskLevel in riskLevelList)
            {
                PopulateRiskLevel(countyRiskLevel, selectedCounty, lastUpdateDate);
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

        private void PopulateRiskLevel(CountyRiskLevel countyRiskLevel, County selectedCounty, DateTime latestUpdateDate)
        {
            if (selectedCounty.CriticalDaysMovingCasesPerOneHundredThousandAverage >= countyRiskLevel.RiskLelvelCasesMin &&
                selectedCounty.CriticalDaysMovingCasesPerOneHundredThousandAverage <= countyRiskLevel.RiskLelvelCasesMax)
            {
                countyRiskLevel.IsCurrentRiskLevel = true;
            }
            if (selectedCounty.CriticalDaysMovingCasesPerOneHundredThousandAverage < countyRiskLevel.RiskLelvelCasesMin)
            {
                countyRiskLevel.IsPassedRiskLevel = true;
            }
            if (selectedCounty.CriticalDaysMovingCasesPerOneHundredThousandAverage > countyRiskLevel.RiskLelvelCasesMax)
            {
                countyRiskLevel.IsFutureRiskLevel = true;
            }

            DateTime? riskLevelDate = EstimateCountyRiskLevelDate(countyRiskLevel, selectedCounty.CriticalDaysMovingCasesPerOneHundredThousandAverage, selectedCounty.CriticalDaysMovingCasesRateChange);
            DateTime? zeroAvailableICUBedsDate = EstimateZeroAvailableICUBedsDate(selectedCounty.CriticalDaysMovingAvailableICUBedsAverage, selectedCounty.CriticalDaysMovingAvailableICUBedsRateChange);
            countyRiskLevel.EstimateRiskLevelDate = riskLevelDate;
            string riskLevelDateDisplay = riskLevelDate != null ? $"{riskLevelDate:MMMM d, yyyy}" : selectedCounty.CriticalDaysMovingCasesRateChange >= 0 ? "-" : "N/A";
            countyRiskLevel.EstimateRiskLevelDateDisplay = riskLevelDateDisplay;
            countyRiskLevel.EstimateRiskLevelDateQualification = riskLevelDate;
            countyRiskLevel.EstimateRiskLevelDateQualificationDisplay = riskLevelDateDisplay;
            countyRiskLevel.EstimateZeroAvailableICUBedsDate = zeroAvailableICUBedsDate;
            string zeroAvailableICUBedsDateDisplay = zeroAvailableICUBedsDate != null ? $"{zeroAvailableICUBedsDate:MMMM d, yyyy}" : "N/A";
            countyRiskLevel.EstimateZeroAvailableICUBedsDateDisplay = zeroAvailableICUBedsDateDisplay;
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

        private DateTime? EstimateZeroAvailableICUBedsDate(decimal? criticalDaysMovingAvailableICUBedsAverage, decimal? criticalDaysMovingAvailableICUBedsRateChange)
        {
            if (criticalDaysMovingAvailableICUBedsRateChange > 0) { return null;} //Available ICU Beds are going up
            DateTime zeroAvaiableICUBedsDate = DateTime.Today;
            decimal? availableICUBedsAverage = criticalDaysMovingAvailableICUBedsAverage;
            int daysToAdd = 0;

            while (availableICUBedsAverage > 1)
            {
                var availableICUBedsChange = (decimal)(availableICUBedsAverage * (decimal)criticalDaysMovingAvailableICUBedsRateChange);
                availableICUBedsAverage += availableICUBedsChange;
                daysToAdd++;
            }

            return zeroAvaiableICUBedsDate.AddDays(daysToAdd);
        }
    }
}
