What it does SkinnyControllersCommon/SkinnyControllersGenerator

Read this better at https://github.com/ignatandrei/AOP_With_Roslyn/tree/master/SkinnyControllers

Video at https://youtu.be/eNvt4Vq2PuY

SkinnyControllers generates controller action for each field of your controller 

How to install SkinnyControllers  in a .NET Core 5 WebAPI / MVC application
Step 1:

Install https://www.nuget.org/packages/SkinnyControllersGenerator/ 


Step 2:

Install https://www.nuget.org/packages/SkinnyControllersCommon/


Step 3:

Add a field to your action either via DI, either directly

    [ApiController]
    [Route("[controller]/[action]")]
    public partial class WeatherForecastController : ControllerBase
    {

        private readonly RepositoryWF repository;
        
        public WeatherForecastController(RepositoryWF repository)
        {
            this.repository = repository;            
            //or make
			//this.repository=new RepositoryWF();
        }

		

Step 4:

	Add partial to your controller and decorate your controller with 

	[AutoActions(template = TemplateIndicator.AllPost,FieldsName =new[] { "repository" })]
    [ApiController]
    [Route("[controller]/[action]")]
    public partial class WeatherForecastController : ControllerBase

You can choose your template from 
1. All Post
2. Get - if not arguments, POST else
3. Rest action

You can add your template by making a PR to https://github.com/ignatandrei/AOP_With_Roslyn/tree/master/SkinnyControllers

That's all!

Step 5: Advanced - BYT - bring your template

Create  a Template like <a href='TemplateIdentity.txt'>Template Identity</a>

Add to your controleer
[AutoActions(template = TemplateIndicator.CustomTemplateFile, CustomTemplateFileName = "TemplateIdentity.txt", ExcludeFields = new[] { "_logger" }, FieldsName = new[] { "*" })]
    
And this will be compiled

Problems:

1. When compile , it gives

error CS0260: Missing partial modifier on declaration of type '....'; another partial declaration of this type exists
 
 
Solution
Add partial to your controller declaration
public partial class 

2. Swagger refuses to load json

Failed to load API definition.
Fetch errorundefined /swagger/v1/swagger.json
 
 Solution:
 Run the project as console and look to the error in the console
 Usually is a " Swashbuckle.AspNetCore.SwaggerGen.SwaggerGeneratorException: Conflicting method/path combination"
 Try to 
 1. give different names to your methods 
 2. use TemplateIndicator.AllPost
3. Make the route with [action] [Route("api/[controller]/[action]")]  
