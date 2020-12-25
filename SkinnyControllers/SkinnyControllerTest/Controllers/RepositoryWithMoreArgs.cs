using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkinnyControllerTest.Controllers
{
    public class RepositoryWithMoreArgs
    {
        public bool NoArguments()
        {
            return false;
        }
        //public bool EqualityBetween(int xi)
        //{
        //    return xi==0;
        //}
        public bool EqualityBetween(string x, int i)
        {
            return x == i.ToString();
        }
        public bool IsEqualTemperatureC(int i, WeatherForecast f)
        {
            return f.TemperatureC == i;
        }
        public bool IsEqualTemperatureWith2Objects(WeatherForecast a, WeatherForecast f)
        {
            return f?.TemperatureC == a?.TemperatureC;
        }
    }
}
