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
        public void PopulateAggregatesCovidData(List<CovidData> covidRecords, County selectedCounty)
        {
            //Get seven day moving
            foreach (var covidRecord in covidRecords)
            {
                var lastSevenDayCovidData = covidRecords.Where(x => x.Date > covidRecord.Date.AddDays(-7) && x.Date <= covidRecord.Date).ToList();

                decimal? sevenDayMovingSum = 0;
                int count = 0;
                foreach (var record in lastSevenDayCovidData)
                {
                    sevenDayMovingSum = sevenDayMovingSum + record.NewCountConfirmed;
                    count++;
                }

                decimal? sevenDayMovingAverage = sevenDayMovingSum / (count == 0 ? 1 : count);
                decimal? covidCasesPerOneHundredThousand = (decimal)(sevenDayMovingAverage / ((decimal)selectedCounty.Population / (decimal)100000));

                covidRecord.SevenDayMovingAverage = sevenDayMovingAverage;
                covidRecord.SevenDayMovingCasesPerOneHundredThousand = covidCasesPerOneHundredThousand;
            }

            //Get rate change based on seven/fourteen day moving
            foreach (var covidRecord in covidRecords)
            {
                decimal? previousDateSevenDayMovingAverage = covidRecords.FirstOrDefault(x => x.Date == covidRecord.Date.AddDays(-1))?.SevenDayMovingAverage;

                decimal? sevenRateChange = 0;
                if (covidRecord.SevenDayMovingAverage != null && previousDateSevenDayMovingAverage != null && previousDateSevenDayMovingAverage != 0)
                {
                    sevenRateChange = (decimal)covidRecord.SevenDayMovingAverage / (decimal)previousDateSevenDayMovingAverage;
                    sevenRateChange = sevenRateChange - 1;
                }
                covidRecord.SevenDayMovingRateChange = sevenRateChange;
            }
        }

        public void PopulateCountyAggregates(List<CovidData> covidRecords, County selectedCounty, DateTime lastUpdateDate)
        {
            //Get seven day moving averages
            var sevenDayCovidData = covidRecords.Where(x => x.Date > lastUpdateDate.AddDays(-7) && x.Date <= lastUpdateDate).ToList();

            decimal? sevenDayMovingCasesPerOneHundredThousandSum = 0;
            decimal? sevenDayMovingRateChangeSum = 0;
            decimal? sevenDayMovingCasesSum = 0;
            int sevenDaycount = 0;
            foreach (var record in sevenDayCovidData)
            {
                sevenDayMovingCasesPerOneHundredThousandSum = sevenDayMovingCasesPerOneHundredThousandSum + record.SevenDayMovingCasesPerOneHundredThousand;
                sevenDayMovingRateChangeSum = sevenDayMovingRateChangeSum + record.SevenDayMovingRateChange;
                sevenDayMovingCasesSum = sevenDayMovingCasesSum + record.SevenDayMovingAverage;
                sevenDaycount++;
            }

            decimal? sevenDayMovingCasesPerOneHundredThousandAverage = sevenDayMovingCasesPerOneHundredThousandSum / (sevenDaycount == 0 ? 1 : sevenDaycount);
            decimal? sevenDayMovingRateChangeAverage = sevenDayMovingRateChangeSum / (sevenDaycount == 0 ? 1 : sevenDaycount);
            decimal? sevenDayMovingCasesAverage = sevenDayMovingCasesSum / (sevenDaycount == 0 ? 1 : sevenDaycount);

            selectedCounty.SevenDayMovingCasesPerOneHundredThousandAverage = sevenDayMovingCasesPerOneHundredThousandAverage;
            selectedCounty.SevenDayMovingRateChange = sevenDayMovingRateChangeAverage;
            selectedCounty.SevenDayMovingCasesAverage = sevenDayMovingCasesAverage;
            selectedCounty.RiskLevels = GetCountyRiskLevels(selectedCounty, lastUpdateDate);
        }

        private List<CountyRiskLevel> GetCountyRiskLevels(County selectedCounty, DateTime lastUpdateDate)
        {
            var defaultDateDisplay = selectedCounty.SevenDayMovingRateChange >= 0 ? "N/A Cases are not declining" :"N/A";
            var riskLevelList = new List<CountyRiskLevel>();
            riskLevelList.Add(new CountyRiskLevel { RiskLevelOrder = 1, RiskLevel = RiskLevel.Minimal, RiskLelvelCasesMin = AppConfigurationSettings.MinimalMin, RiskLelvelCasesMax = AppConfigurationSettings.MinimalMax, EstimateRiskLevelDateDisplay = defaultDateDisplay, EstimateRiskLevelDateQualificationDisplay = defaultDateDisplay });
            riskLevelList.Add(new CountyRiskLevel { RiskLevelOrder = 2, RiskLevel = RiskLevel.Moderate, RiskLelvelCasesMin = AppConfigurationSettings.ModerateMin, RiskLelvelCasesMax = AppConfigurationSettings.ModerateMax, EstimateRiskLevelDateDisplay = defaultDateDisplay, EstimateRiskLevelDateQualificationDisplay = defaultDateDisplay });
            riskLevelList.Add(new CountyRiskLevel { RiskLevelOrder = 3, RiskLevel = RiskLevel.Substantial, RiskLelvelCasesMin = AppConfigurationSettings.SubstantialMin, RiskLelvelCasesMax = AppConfigurationSettings.SubstantialMax, EstimateRiskLevelDateDisplay = defaultDateDisplay, EstimateRiskLevelDateQualificationDisplay = defaultDateDisplay });
            riskLevelList.Add(new CountyRiskLevel { RiskLevelOrder = 4, RiskLevel = RiskLevel.Widespread, RiskLelvelCasesMin = AppConfigurationSettings.WidespreadMin, RiskLelvelCasesMax = AppConfigurationSettings.WidespreadMax, EstimateRiskLevelDateDisplay = defaultDateDisplay, EstimateRiskLevelDateQualificationDisplay = defaultDateDisplay });

            foreach (var countyRiskLevel in riskLevelList)
            {
                PopulateRiskLevel(countyRiskLevel, selectedCounty.SevenDayMovingCasesPerOneHundredThousandAverage, selectedCounty.SevenDayMovingRateChange, lastUpdateDate);
            }

            selectedCounty.CurrentRiskLevel = riskLevelList.FirstOrDefault(x => x.IsCurrentRiskLevel)?.RiskLevel ?? RiskLevel.Widespread;

            //California guideline date requirement
            foreach (var countyRiskLevel in riskLevelList.OrderByDescending(x => x.RiskLevelOrder))
            {
                var previousRiskLevel = riskLevelList.FirstOrDefault(x => x.RiskLevelOrder == (countyRiskLevel.RiskLevelOrder + 1));
                if (previousRiskLevel == null) continue;

                DateTime? previousRiskLevelDate = countyRiskLevel.EstimateRiskLevelDate > previousRiskLevel.EstimateRiskLevelDateQualification ? countyRiskLevel.EstimateRiskLevelDate : previousRiskLevel.EstimateRiskLevelDateQualification;

                if (previousRiskLevelDate == null) continue;

                var estimatedRiskLevelDateQualification = previousRiskLevelDate.Value.AddDays(AppConfigurationSettings.CaliforniaWaitTimeRequirement); //California requires to meet risk level criteria for 2 weeks, county must stay in that level for at least 3 weeks before moving to new level
                countyRiskLevel.EstimateRiskLevelDateQualification = estimatedRiskLevelDateQualification;
                countyRiskLevel.EstimateRiskLevelDateQualificationDisplay = $"{estimatedRiskLevelDateQualification:MMMM d, yyyy}";
            }

            return riskLevelList;
        }

        private void PopulateRiskLevel(CountyRiskLevel countyRiskLevel, decimal? sevenDayMovingCasesPerOneHundredThousandAverage, decimal? sevenDayMovingRateChange, DateTime latestUpdateDate)
        {
            if (sevenDayMovingCasesPerOneHundredThousandAverage >= countyRiskLevel.RiskLelvelCasesMin &&
                sevenDayMovingCasesPerOneHundredThousandAverage <= countyRiskLevel.RiskLelvelCasesMax)
            {
                countyRiskLevel.IsCurrentRiskLevel = true;
            }
            DateTime? riskLevelDate = EstimateCountyRiskLevelDate(sevenDayMovingCasesPerOneHundredThousandAverage, sevenDayMovingRateChange, countyRiskLevel.RiskLelvelCasesMax);
            countyRiskLevel.EstimateRiskLevelDate = riskLevelDate;
            string riskLevelDateDisplay = riskLevelDate != null ? $"{riskLevelDate:MMMM d, yyyy}" : sevenDayMovingRateChange >= 0 ? "N/A Cases are not declining" : "N/A";
            countyRiskLevel.EstimateRiskLevelDateDisplay = riskLevelDateDisplay;
            countyRiskLevel.EstimateRiskLevelDateQualification = riskLevelDate;
            countyRiskLevel.EstimateRiskLevelDateQualificationDisplay = riskLevelDateDisplay;
        }

        private DateTime? EstimateCountyRiskLevelDate(decimal? sevenDayMovingCasesPerOneHundredThousandAverage, decimal? sevenDayMovingRateChange, decimal riskLelvelCasesMax)
        {
            //Cases are increasing, return
            if (sevenDayMovingRateChange >= 0)
            {
                return null;
            }

            DateTime countyRiskLevelDate = new DateTime(2020, 08, 31); //This is the day California officially started with this risk level
            decimal? casesPerOneHundredThousandAverage = sevenDayMovingCasesPerOneHundredThousandAverage;
            int daysToAdd = 0;

            while (casesPerOneHundredThousandAverage > riskLelvelCasesMax)
            {
                var casesChange = (decimal)(casesPerOneHundredThousandAverage * (decimal)sevenDayMovingRateChange);
                casesPerOneHundredThousandAverage += casesChange;
                daysToAdd++;
            }

            return countyRiskLevelDate.AddDays(daysToAdd);
        }
    }
}
