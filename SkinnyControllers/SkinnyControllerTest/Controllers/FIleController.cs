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
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AutoActions(template = TemplateIndicator.AllPostWithRecord, FieldsName = new[] { "*" })]
    public partial class FIleController
    { 
        private readonly IFile file;

        public FIleController(IFile file)
        {
            this.file = file;
        }
    }
}
