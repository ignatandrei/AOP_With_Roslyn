using System;
using System.Linq;
using System.Threading.Tasks;

namespace SkinnyControllerTest
{
    public class RepositoryWF
    {
        private static readonly string[] Summaries = new[]
{
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecast[] GetData()
        {
            return DataToDo(5);
        }
        public void DoStuff()
        {
            Console.WriteLine("do stuff");
        }
        public WeatherForecast[] DataToDo(int i)
        {
            var rng = new Random();
            return Enumerable.Range(1, i).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

        }

        public long Add(int i, int j)
        {
            return i + j;

        }
        public async Task<WeatherForecast> GetMyDataInAsyncManner()
        {
            await Task.Delay(5000);
            return DataToDo(1)[0];
        }
    }
}