using System.Collections.Generic;
using CovidStatus.Shared.Entities;

namespace CovidStatus.API.Repositories.Interface
{
    public interface ICovidDataRepository
    {
        List<County> GetCountyList();
        List<CovidData> GetCovidDataByCounty(string countyName);
        string GetCARawCovidJsonDataByCounty(string countyName);
        string GetCARawCovidHospitalJsonDataByCounty(string countyName);
    }
}
