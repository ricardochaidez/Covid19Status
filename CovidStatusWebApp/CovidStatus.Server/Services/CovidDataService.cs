using System.Collections.Generic;
using System.Threading.Tasks;
using CovidStatus.API.Repositories.Interface;
using CovidStatus.Server.Services.Interfaces;
using CovidStatus.Shared.Entities;

namespace CovidStatus.Server.Services
{
    public class CovidDataService : ICovidDataService
    {
        private readonly ICovidDataRepository _covidDataRepository;
        public CovidDataService(ICovidDataRepository covidDataRepository)
        {
            _covidDataRepository = covidDataRepository;
        }
        public async Task<List<County>> GetCountyList()
        {
            var countyList = _covidDataRepository.GetCountyList();
            return countyList;
        }

        public async Task<List<CovidData>> GetCovidDataByCounty(string countyName)
        {
            var covidData = _covidDataRepository.GetCovidDataByCounty(countyName);
            return covidData;
        }

        public async Task<string> GetCARawCovidJsonDataByCounty(string countyName)
        {
            return _covidDataRepository.GetCARawCovidJsonDataByCounty(countyName);
        }

        public async Task<string> GetCARawCovidHospitalJsonDataByCounty(string countyName)
        {
            return _covidDataRepository.GetCARawCovidHospitalJsonDataByCounty(countyName);
        }
    }
}
