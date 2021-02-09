using System.Threading.Tasks;

namespace SkinnyControllerTest
{
    public interface ITestAsyncLogging
    {
        Task<string> Blog();
        Task<int> Data();
        void DoWork();
        bool OKData();
    }
}