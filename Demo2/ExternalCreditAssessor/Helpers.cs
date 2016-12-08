using System.Net;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace ExternalCreditAssessor
{
    public static class Helpers
    {
        public static async Task WriteStatus(this IOwinContext context, HttpStatusCode status, string reason)
        {
            var intStatus = (int) status;
            var response = context.Response;

            response.ReasonPhrase = reason;
            response.StatusCode = intStatus;

            await response.WriteAsync($"HTTP {intStatus} ({status}): {reason}");
        }
    }
}