using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Movie.Demo.API.Helpers;
using Movie.Demo.Utility.Enumeration;

namespace Movie.Demo.API.Loggers
{
    public static class ExceptionMiddleware
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                try
                {
                    logger.Information("******* In Exception Handler Code ***********");

                    appError.Run(ExceptionHandler(logger));
                }
                catch (Exception exp)
                {
                    logger.Error("Error in executing the Exception Handler {0}", exp.GetBaseException());
                }
            });
        }

        private static RequestDelegate ExceptionHandler(ILogger logger)
        {
            return async context =>
            {
                string message = "Internal server error. Please try again.";
                int statusCode = (int)HttpStatusCode.InternalServerError;
                try
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                }
                catch (Exception ex)
                {
                    logger.Error(Helper.LogContextErrorInfo(Layer.ExceptionMiddleware.ToString(), Method.ExceptionHandler.ToString(), 
                        "Null Input Received", ex.Message, ex.InnerException, ex.StackTrace));
                    await ErrorResponse(context, message, statusCode).ConfigureAwait(false);
                }
            };
        }

        private static async Task ErrorResponse(HttpContext context, string message, int statusCode)
        {
            await context.Response.WriteAsync(new Error()
            {
                StatusCode = statusCode,
                ErrorResponse = new Dictionary<string, object>
                                               {
                                                { "message", message },
                                                { "originalMessage", message }
                                               },
            }.ToString()).ConfigureAwait(false);
        }
    }
}