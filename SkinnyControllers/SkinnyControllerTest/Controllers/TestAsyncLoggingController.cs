using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkinnyControllersCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkinnyControllerTest.Controllers
{
    [AutoActions(template = TemplateIndicator.TryCatchLogging, FieldsName = new[] { "*" }, ExcludeFields = new[] { "_logger" })]

    [Route("api/[controller]/[action]")]
    [ApiController]
    public partial class TestAsyncLoggingController : ControllerBase
    {
        private readonly ITestAsyncLogging data;

        public TestAsyncLoggingController(ITestAsyncLogging data)
        {
            this.data = data;
            this.OKData();
        }
    }
}
