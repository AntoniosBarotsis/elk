using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace elk
{
    public class MyMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public MyMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Something went wrong, {message}", exception.Data["someVeryImportantAttribute"]);
                
                await HandleExceptionAsync(context, exception);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500;
            return context.Response.WriteAsync("An exception occured");
        }
    }
}