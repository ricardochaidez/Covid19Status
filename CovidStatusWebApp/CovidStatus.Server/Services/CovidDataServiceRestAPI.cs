using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CovidStatus.Server.Services.Interfaces;
using CovidStatus.Shared.Entities;

namespace CovidStatus.Server.Services
{
    public class CovidDataServiceRestAPI : ICovidDataService
    {
        private readonly HttpClient _httpClient;

        public CovidDataServiceRestAPI(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<County>> GetCountyList()
        {
            var countyList = new List<County>();
            countyList.Add(new County { CountyID = 1, CountyName = "Alameda", Population = 1000 });
            countyList.Add(new County { CountyID = 2, CountyName = "Alpine", Population = 1000 });
            countyList.Add(new County { CountyID = 3, CountyName = "Amador", Population = 1000 });
            countyList.Add(new County { CountyID = 4, CountyName = "Butte", Population = 1000 });
            countyList.Add(new County { CountyID = 5, CountyName = "Calaveras", Population = 1000 });
            countyList.Add(new County { CountyID = 6, CountyName = "Colusa", Population = 1000 });
            countyList.Add(new County { CountyID = 7, CountyName = "Contra Costa", Population = 1000 });
            countyList.Add(new County { CountyID = 8, CountyName = "Del Norte", Population = 1000 });
            countyList.Add(new County { CountyID = 9, CountyName = "El Dorado", Population = 1000 });
            countyList.Add(new County { CountyID = 10, CountyName = "Fresno", Population = 1000 });
            countyList.Add(new County { CountyID = 11, CountyName = "Glenn", Population = 1000 });
            countyList.Add(new County { CountyID = 12, CountyName = "Humboldt", Population = 1000 });
            countyList.Add(new County { CountyID = 13, CountyName = "Imperial", Population = 1000 });
            countyList.Add(new County { CountyID = 14, CountyName = "Inyo", Population = 1000 });
            countyList.Add(new County { CountyID = 15, CountyName = "Kern", Population = 1000 });
            countyList.Add(new County { CountyID = 16, CountyName = "Kings", Population = 1000 });
            countyList.Add(new County { CountyID = 17, CountyName = "Lake", Population = 1000 });
            countyList.Add(new County { CountyID = 18, CountyName = "Lassen", Population = 1000 });
            countyList.Add(new County { CountyID = 19, CountyName = "Los Angeles", Population = 1000 });
            countyList.Add(new County { CountyID = 20, CountyName = "Madera", Population = 1000 });
            countyList.Add(new County { CountyID = 21, CountyName = "Marin", Population = 1000 });
            countyList.Add(new County { CountyID = 22, CountyName = "Mariposa", Population = 1000 });
            countyList.Add(new County { CountyID = 23, CountyName = "Mendocino", Population = 1000 });
            countyList.Add(new County { CountyID = 24, CountyName = "Merced", Population = 1000 });
            countyList.Add(new County { CountyID = 25, CountyName = "Modoc", Population = 1000 });
            countyList.Add(new County { CountyID = 26, CountyName = "Mono", Population = 1000 });
            countyList.Add(new County { CountyID = 27, CountyName = "Monterey", Population = 1000 });
            countyList.Add(new County { CountyID = 28, CountyName = "Napa", Population = 1000 });
            countyList.Add(new County { CountyID = 29, CountyName = "Nevada", Population = 1000 });
            countyList.Add(new County { CountyID = 30, CountyName = "Orange", Population = 1000 });
            countyList.Add(new County { CountyID = 31, CountyName = "Placer", Population = 1000 });
            countyList.Add(new County { CountyID = 32, CountyName = "Plumas", Population = 1000 });
            countyList.Add(new County { CountyID = 33, CountyName = "Riverside", Population = 1000 });
            countyList.Add(new County { CountyID = 34, CountyName = "Sacramento", Population = 1000 });
            countyList.Add(new County { CountyID = 35, CountyName = "San Benito", Population = 1000 });
            countyList.Add(new County { CountyID = 36, CountyName = "San Bernardino", Population = 2180085 });
            countyList.Add(new County { CountyID = 37, CountyName = "San Diego", Population = 1000 });
            countyList.Add(new County { CountyID = 38, CountyName = "San Francisco", Population = 1000 });
            countyList.Add(new County { CountyID = 39, CountyName = "San Joaquin", Population = 1000 });
            countyList.Add(new County { CountyID = 40, CountyName = "San Luis Obispo", Population = 1000 });
            countyList.Add(new County { CountyID = 41, CountyName = "San Mateo", Population = 1000 });
            countyList.Add(new County { CountyID = 42, CountyName = "Santa Barbara", Population = 1000 });
            countyList.Add(new County { CountyID = 43, CountyName = "Santa Clara", Population = 1000 });
            countyList.Add(new County { CountyID = 44, CountyName = "Santa Cruz", Population = 1000 });
            countyList.Add(new County { CountyID = 45, CountyName = "Shasta", Population = 1000 });
            countyList.Add(new County { CountyID = 46, CountyName = "Sierra", Population = 1000 });
            countyList.Add(new County { CountyID = 47, CountyName = "Siskiyou", Population = 1000 });
            countyList.Add(new County { CountyID = 48, CountyName = "Solano", Population = 1000 });
            countyList.Add(new County { CountyID = 49, CountyName = "Sonoma", Population = 1000 });
            countyList.Add(new County { CountyID = 50, CountyName = "Stanislaus", Population = 1000 });
            countyList.Add(new County { CountyID = 51, CountyName = "Sutter", Population = 1000 });
            countyList.Add(new County { CountyID = 52, CountyName = "Tehama", Population = 1000 });
            countyList.Add(new County { CountyID = 53, CountyName = "Trinity", Population = 1000 });
            countyList.Add(new County { CountyID = 54, CountyName = "Tulare", Population = 1000 });
            countyList.Add(new County { CountyID = 55, CountyName = "Tuolumne", Population = 1000 });
            countyList.Add(new County { CountyID = 56, CountyName = "Ventura", Population = 1000 });
            countyList.Add(new County { CountyID = 57, CountyName = "Yolo", Population = 1000 });
            countyList.Add(new County { CountyID = 58, CountyName = "Yuba", Population = 1000 });



            return countyList;
        }
        public async Task<List<CovidData>> GetCovidDataByCounty(string countyName)
        {
            var covidDataList = await JsonSerializer.DeserializeAsync<List<CovidData>>(
                await _httpClient.GetStreamAsync($"api/coviddata/{countyName}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return covidDataList;
        }
    }
}
