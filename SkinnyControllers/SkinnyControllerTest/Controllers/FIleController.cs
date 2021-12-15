using Microsoft.AspNetCore.Mvc;
using SkinnyControllersCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace SkinnyControllerTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[AutoActions(template = TemplateIndicator., FieldsName = new[] { "personRepository" })]
    public class FIleController
    {
        private readonly IFile file;

        public FIleController(IFile file)
        {
            this.file = file;
        }
    }
}
