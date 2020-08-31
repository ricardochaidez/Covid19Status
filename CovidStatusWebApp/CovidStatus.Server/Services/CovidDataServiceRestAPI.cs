﻿using System.Collections.Generic;
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
            countyList.Add(new County { CountyID = 1, CountyName = "Alameda", Population = 1671329 });
            countyList.Add(new County { CountyID = 2, CountyName = "Alpine", Population = 1129 });
            countyList.Add(new County { CountyID = 3, CountyName = "Amador", Population = 39752 });
            countyList.Add(new County { CountyID = 4, CountyName = "Butte", Population = 219186 });
            countyList.Add(new County { CountyID = 5, CountyName = "Calaveras", Population = 45905 });
            countyList.Add(new County { CountyID = 6, CountyName = "Colusa", Population = 21547 });
            countyList.Add(new County { CountyID = 7, CountyName = "Contra Costa", Population = 1153526 });
            countyList.Add(new County { CountyID = 8, CountyName = "Del Norte", Population = 27812 });
            countyList.Add(new County { CountyID = 9, CountyName = "El Dorado", Population = 192843 });
            countyList.Add(new County { CountyID = 10, CountyName = "Fresno", Population = 999101 });
            countyList.Add(new County { CountyID = 11, CountyName = "Glenn", Population = 28393 });
            countyList.Add(new County { CountyID = 12, CountyName = "Humboldt", Population = 135558 });
            countyList.Add(new County { CountyID = 13, CountyName = "Imperial", Population = 181215 });
            countyList.Add(new County { CountyID = 14, CountyName = "Inyo", Population = 18039 });
            countyList.Add(new County { CountyID = 15, CountyName = "Kern", Population = 900202 });
            countyList.Add(new County { CountyID = 16, CountyName = "Kings", Population = 152940 });
            countyList.Add(new County { CountyID = 17, CountyName = "Lake", Population = 64386 });
            countyList.Add(new County { CountyID = 18, CountyName = "Lassen", Population = 30573 });
            countyList.Add(new County { CountyID = 19, CountyName = "Los Angeles", Population = 10039107 });
            countyList.Add(new County { CountyID = 20, CountyName = "Madera", Population = 157327 });
            countyList.Add(new County { CountyID = 21, CountyName = "Marin", Population = 258826 });
            countyList.Add(new County { CountyID = 22, CountyName = "Mariposa", Population = 17203 });
            countyList.Add(new County { CountyID = 23, CountyName = "Mendocino", Population = 86749 });
            countyList.Add(new County { CountyID = 24, CountyName = "Merced", Population = 277680 });
            countyList.Add(new County { CountyID = 25, CountyName = "Modoc", Population = 8841 });
            countyList.Add(new County { CountyID = 26, CountyName = "Mono", Population = 14444 });
            countyList.Add(new County { CountyID = 27, CountyName = "Monterey", Population = 434061 });
            countyList.Add(new County { CountyID = 28, CountyName = "Napa", Population = 137744 });
            countyList.Add(new County { CountyID = 29, CountyName = "Nevada", Population = 99755 });
            countyList.Add(new County { CountyID = 30, CountyName = "Orange", Population = 3175692 });
            countyList.Add(new County { CountyID = 31, CountyName = "Placer", Population = 398329 });
            countyList.Add(new County { CountyID = 32, CountyName = "Plumas", Population = 18807 });
            countyList.Add(new County { CountyID = 33, CountyName = "Riverside", Population = 2470546 });
            countyList.Add(new County { CountyID = 34, CountyName = "Sacramento", Population = 1552058 });
            countyList.Add(new County { CountyID = 35, CountyName = "San Benito", Population = 62808 });
            countyList.Add(new County { CountyID = 36, CountyName = "San Bernardino", Population = 2180085 });
            countyList.Add(new County { CountyID = 37, CountyName = "San Diego", Population = 3338330 });
            countyList.Add(new County { CountyID = 38, CountyName = "San Francisco", Population = 881549 });
            countyList.Add(new County { CountyID = 39, CountyName = "San Joaquin", Population = 762148 });
            countyList.Add(new County { CountyID = 40, CountyName = "San Luis Obispo", Population = 283111 });
            countyList.Add(new County { CountyID = 41, CountyName = "San Mateo", Population = 766573 });
            countyList.Add(new County { CountyID = 42, CountyName = "Santa Barbara", Population = 446499 });
            countyList.Add(new County { CountyID = 43, CountyName = "Santa Clara", Population = 1927852 });
            countyList.Add(new County { CountyID = 44, CountyName = "Santa Cruz", Population = 273213 });
            countyList.Add(new County { CountyID = 45, CountyName = "Shasta", Population = 180080 });
            countyList.Add(new County { CountyID = 46, CountyName = "Sierra", Population = 3005 });
            countyList.Add(new County { CountyID = 47, CountyName = "Siskiyou", Population = 43539 });
            countyList.Add(new County { CountyID = 48, CountyName = "Solano", Population = 447643 });
            countyList.Add(new County { CountyID = 49, CountyName = "Sonoma", Population = 494336 });
            countyList.Add(new County { CountyID = 50, CountyName = "Stanislaus", Population = 550660 });
            countyList.Add(new County { CountyID = 51, CountyName = "Sutter", Population = 96971 });
            countyList.Add(new County { CountyID = 52, CountyName = "Tehama", Population = 65084 });
            countyList.Add(new County { CountyID = 53, CountyName = "Trinity", Population = 12285 });
            countyList.Add(new County { CountyID = 54, CountyName = "Tulare", Population = 466195 });
            countyList.Add(new County { CountyID = 55, CountyName = "Tuolumne", Population = 54478 });
            countyList.Add(new County { CountyID = 56, CountyName = "Ventura", Population = 846006 });
            countyList.Add(new County { CountyID = 57, CountyName = "Yolo", Population = 220500 });
            countyList.Add(new County { CountyID = 58, CountyName = "Yuba", Population = 78668 });



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
