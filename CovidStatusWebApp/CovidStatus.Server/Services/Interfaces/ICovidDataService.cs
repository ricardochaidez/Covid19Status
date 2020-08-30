using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidStatus.Shared.Entities;

namespace CovidStatus.Server.Services.Interfaces
{
    public interface ICovidDataService
    {
        Task<List<County>> GetCountyList();
        Task<List<CovidData>> GetCovidDataByCounty(string countyName);
    }
}
