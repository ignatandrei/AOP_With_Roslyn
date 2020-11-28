What it does SkinnyControllersCommon/SkinnyControllersGenerator

Read this better at https://github.com/ignatandrei/AOP_With_Roslyn/tree/master/SkinnyControllers


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

	Decorate your controller with 

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