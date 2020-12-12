using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkinnyControllersCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkinnyControllerTest.Controllers
{

    /// <summary>
    /// Do not forget to put this in the csproj
    /// <ItemGroup>
    ///<AdditionalFiles Include = "Controllers\\CustomTemplate1.txt" />
    ///</ItemGroup >
    /// </summary>
    [AutoActions(template = TemplateIndicator.CustomTemplateFile, FieldsName = new[] { "*" } ,CustomTemplateFileName = "Controllers\\CustomTemplate1.txt")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public partial class CustomTemplateController : ControllerBase
    {
        private readonly RepositoryWF repository;

        public CustomTemplateController ()
        {
            //do via DI
            repository = new RepositoryWF();
        }

    }
}
