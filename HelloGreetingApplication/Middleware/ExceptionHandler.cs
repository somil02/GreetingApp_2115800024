using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NLog;

namespace Middleware.ExceptionHandler
{
    public class ExceptionHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly RequestDelegate _next;

        public ExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); 
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred in the application");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var errorResponse = new
                {
                    Success = false,
                    Message = "An internal server error occurred.",
                    Error = ex.Message
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            }
        }

        public static string HandleException(Exception ex, out object errorResponse)
        {
            logger.Error(ex, "An error occurred in the application");

            errorResponse = new
            {
                Success = false,
                Message = "An error occurred",
                ErrorEventArgs = ex.Message
            };

            return JsonConvert.SerializeObject(errorResponse);
        }

        public static object CreateErrorResponse(Exception ex)
        {
            logger.Error(ex, "An error occurred in the application");
            return new
            {
                Success = false,
                Message = "An error occurred",
                Error = ex.Message
            };
        }
    }
}
