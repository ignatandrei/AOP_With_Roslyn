using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkinnyControllersCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkinnyControllerTest.Controllers
{
    [AutoActions(template = TemplateIndicator.AllPostWithRecord, FieldsName = new[] { "*" }, ExcludeFields = new[] { "_logger" })]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public partial class WithRecordsController : ControllerBase
    {
        private readonly RepositoryWithMoreArgs repositoryWithMoreArgs;

        public WithRecordsController(RepositoryWithMoreArgs repositoryWithMoreArgs)
        {
            this.repositoryWithMoreArgs = repositoryWithMoreArgs;
        }
    }
}
