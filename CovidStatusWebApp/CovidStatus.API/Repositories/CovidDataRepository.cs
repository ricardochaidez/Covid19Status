using System;
using System.Collections.Generic;
using System.Net.Http;
using CovidStatus.API.ConfigurationSettings;
using CovidStatus.API.Repositories.Interface;
using CovidStatus.Shared.Entities;
using Newtonsoft.Json;

namespace CovidStatus.API.Repositories
{
    public class CovidDataRepository : ICovidDataRepository
    {
        private static readonly HttpClient client = new HttpClient();

        public List<CovidData> GetCovidDataByCounty(string countyName)
        {
            string californiaOpenDataCovidRequest =
                $"{AppConfigurationSettings.CaliforniaCovidOpenDataAddress}&limit=100&q={countyName}&sort=date%20desc";
            
            string covidDataJson = client.GetStringAsync(californiaOpenDataCovidRequest).Result;

            Root covidDataDeserialized = JsonConvert.DeserializeObject<Root>(covidDataJson);

            var covidDataList = new List<CovidData>();
            foreach (var covidRecord in covidDataDeserialized.result.records)
            {
                var covidData = new CovidData();
                covidData.ID = covidRecord._id;
                covidData.TotalCountConfirmed = covidRecord.totalcountconfirmed;
                covidData.NewCountDeaths = covidRecord.newcountdeaths;
                covidData.TotalCountDeaths = covidRecord.totalcountdeaths;
                covidData.Rank = covidRecord.rank;
                covidData.County = covidRecord.county;
                covidData.NewCountConfirmed = covidRecord.newcountconfirmed;
                covidData.Date = covidRecord.date;
                covidDataList.Add(covidData);    
            }

            return covidDataList;
        }


        public class Info
        {
            public string notes { get; set; }
            public string type_override { get; set; }
            public string label { get; set; }
        }

        public class Field
        {
            public string type { get; set; }
            public string id { get; set; }
            public Info info { get; set; }
        }

        public class Record
        {
            public double totalcountconfirmed { get; set; }
            public int newcountdeaths { get; set; }
            public double totalcountdeaths { get; set; }
            public double rank { get; set; }
            public string county { get; set; }
            public int newcountconfirmed { get; set; }
            public DateTime date { get; set; }
            public int _id { get; set; }
        }

        public class Links
        {
            public string start { get; set; }
            public string next { get; set; }
        }

        public class Result
        {
            public string sort { get; set; }
            public bool include_total { get; set; }
            public string resource_id { get; set; }
            public List<Field> fields { get; set; }
            public string records_format { get; set; }
            public string q { get; set; }
            public List<Record> records { get; set; }
            public int limit { get; set; }
            public Links _links { get; set; }
            public int total { get; set; }
        }

        public class Root
        {
            public string help { get; set; }
            public bool success { get; set; }
            public Result result { get; set; }
        }
    }
}
