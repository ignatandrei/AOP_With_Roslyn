using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SkinnyControllersCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkinnyControllerTest.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public partial class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;

        [AutoActions]
        private readonly RepositoryWF repository;
        
        public WeatherForecastController(ILogger<WeatherForecastController> logger, RepositoryWF repository)
        {
            _logger = logger;
            this.repository = repository;
                    
        }



    }
}