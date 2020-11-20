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
            var countyList = await JsonSerializer.DeserializeAsync<List<County>>(
                await _httpClient.GetStreamAsync($"api/coviddata/countylist"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return countyList;
        }
        public async Task<List<CovidData>> GetCovidDataByCounty(string countyName)
        {
            var covidDataList = await JsonSerializer.DeserializeAsync<List<CovidData>>(
                await _httpClient.GetStreamAsync($"api/coviddata/{countyName}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return covidDataList;
        }

        public async Task<string> GetCARawCovidJsonDataByCounty(string countyName)
        {
            var covidDataList = await JsonSerializer.DeserializeAsync<string>(
                await _httpClient.GetStreamAsync($"api/coviddata/californiarawdata/{countyName}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return covidDataList;
        }

        public async Task<string> GetCARawCovidHospitalJsonDataByCounty(string countyName)
        {
            var covidDataList = await JsonSerializer.DeserializeAsync<string>(
                await _httpClient.GetStreamAsync($"api/coviddata/californiarawhospitaldata/{countyName}"),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return covidDataList;
        }
    }
}
