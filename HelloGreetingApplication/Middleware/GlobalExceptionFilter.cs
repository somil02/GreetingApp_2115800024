using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Middleware.ExceptionHandler;
using NLog;

namespace Middleware.GlobalExceptionFilter
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public GlobalExceptionFilter() { }

        public override void OnException(ExceptionContext context)
        {
            var errorResponse = ExceptionHandler.ExceptionHandler.CreateErrorResponse(context.Exception);

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = 500
            };

            context.ExceptionHandled = true;
        }
    }
}