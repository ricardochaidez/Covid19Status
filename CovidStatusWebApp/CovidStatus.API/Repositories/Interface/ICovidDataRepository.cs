﻿using System.Collections.Generic;
using CovidStatus.Shared.Entities;

namespace CovidStatus.API.Repositories.Interface
{
    public interface ICovidDataRepository
    {
        List<CovidData> GetCovidDataByCounty(string countyName);
    }
}