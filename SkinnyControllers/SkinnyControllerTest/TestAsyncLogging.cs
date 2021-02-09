using System.Threading.Tasks;

namespace SkinnyControllerTest
{
    public class TestAsyncLogging : ITestAsyncLogging
    {
        public Task<int> Data()
        {
            return Task.FromResult(10);
        }

        public async Task<string> Blog()
        {
            System.Console.WriteLine("working");
            await Task.Delay(5000);
            System.Console.WriteLine("done");
            return "http://msprogrammer.serviciipeweb.ro/";
        }
        public bool OKData()
        {
            return true;
        }
        public void DoWork()
        {
            //nothing
        }
    }

}
