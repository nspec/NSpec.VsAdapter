using System.Threading.Tasks;

namespace SampleAsyncSystem
{
    public class AsyncSystemUnderTest
    {
        public async Task<bool> IsAlwaysTrueAsync()
        {
            return await Task.Run(() => true);
        }
    }
}
