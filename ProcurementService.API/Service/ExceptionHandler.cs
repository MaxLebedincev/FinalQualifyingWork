using Microsoft.AspNetCore.Diagnostics;

namespace ProcurementService.API.Service
{
    public class ExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is NotImplementedException error) 
            {
                httpContext.Response.StatusCode = 500;
                 
                return true;
            }
            else
            {
                return true;
            }
        }
    }
}
