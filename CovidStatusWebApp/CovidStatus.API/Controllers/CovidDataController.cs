using System.Threading.Tasks;
using CovidStatus.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CovidStatus.API.Controllers
{
    [Route("api/[controller]")]
    public class CovidDataController : ControllerBase
    {
        private readonly ICovidDataRepository _covidDataRepository;

        public CovidDataController(ICovidDataRepository covidDataRepository)
        {
            _covidDataRepository = covidDataRepository;
        }

        [HttpGet("{countyName}")]
        public IActionResult GetCovidDataByCounty(string countyName)
        {
            if (string.IsNullOrEmpty(countyName))
            {
                ModelState.AddModelError("CountyName", "County name is required");
            }
            return Ok(_covidDataRepository.GetCovidDataByCounty(countyName));
        }

        [HttpGet("countylist")]
        public IActionResult GetCountyList()
        {
            return Ok(_covidDataRepository.GetCountyList());
        }
    }
}
