using Microsoft.AspNetCore.Mvc;
using SkinnyControllersCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SkinnyControllerTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AutoActions(template = TemplateIndicator.Rest,FieldsName = new[] { "personRepository"})]
    public partial class PersonController : ControllerBase
    {
        private PersonRepository personRepository;
        public PersonController()
        {
            //make via DI
            personRepository = new PersonRepository();
        }
    }
}
