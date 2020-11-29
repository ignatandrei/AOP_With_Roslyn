using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkinnyControllersCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkinnyControllerTest.Controllers
{
    [AutoActions(template = TemplateIndicator.AllPost, FieldsName = new[] { "*" })]

    [Route("api/[controller]")]
    [ApiController]
    public partial class AllController : ControllerBase
    {
        private PersonRepository personRepository;
        private readonly RepositoryWF repository;

        public AllController()
        {
            //do via DI
            personRepository = new PersonRepository();
            repository = new RepositoryWF();
        }
    }
}
