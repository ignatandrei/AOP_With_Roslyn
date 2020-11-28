using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SkinnyControllersCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkinnyControllerTest.Controllers
{
    [AutoActions(template = TemplateIndicator.NoArgs_Is_Get_Else_Post,FieldsName =new[] { "repository" })]
    [ApiController]
    [Route("[controller]/[action]")]
    public partial class WeatherForecastControllerArgs0GetElsePost : ControllerBase
    {

        private readonly ILogger<WeatherForecastControllerArgs0GetElsePost > _logger;

       
        private readonly RepositoryWF repository;
        
        public WeatherForecastControllerArgs0GetElsePost(ILogger<WeatherForecastControllerArgs0GetElsePost> logger, RepositoryWF repository)
        {
            _logger = logger;
            this.repository = repository;            
            //var x=this.id ();   
            
        }

        [HttpGet]
        public string MyBlog()
        {
            return "http://msprogrammer.serviciipeweb.ro/";
        }


    }
}