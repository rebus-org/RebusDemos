using System.Net;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace ExternalCreditAssessor
{
    public static class Helpers
    {
        public static async Task WriteStatus(this IOwinContext context, HttpStatusCode status, object body = null, string reason = null)
        {
            var intStatus = (int)status;
            var response = context.Response;

            response.StatusCode = intStatus;

            if (intStatus >= 200 && intStatus < 300)
            {
                await response.WriteAsync(body?.ToString() ?? "<empty response>");
                return;
            }

            response.ReasonPhrase = reason;

            await response.WriteAsync($"HTTP {intStatus} ({status}): {reason}");
        }
    }
}