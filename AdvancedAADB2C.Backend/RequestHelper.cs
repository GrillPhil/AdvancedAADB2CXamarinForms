using Microsoft.AspNetCore.Http;

namespace AdvancedAADB2C.Backend
{
    static class RequestHelper
    {
        internal static string GetBase(HttpRequest request)
        {
#if DEBUG
            // Workaround to test locally as the Android emulator has a different address
            return $"http://localhost:7071/api/";
#else
            return $"{request.Scheme}://{request.Host}{request.PathBase.Value}/api/";
#endif

        }
    }
}
