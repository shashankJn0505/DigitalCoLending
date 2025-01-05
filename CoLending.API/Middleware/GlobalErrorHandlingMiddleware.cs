using CoLending.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace CoLending.API.Middleware
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            //catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            //{
            //    await HandleExceptionAsync(context, ex);
            //    _loggerClient.LogException(ex);
            //}
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            var stackTrace = string.Empty;
            ErrorResponse errorMessage = new ErrorResponse();

            var exceptionType = exception.GetType();
            if (exceptionType == typeof(HttpRequestException))
            {
                HttpRequestException httpRequestException = (HttpRequestException)exception;
                status = (HttpStatusCode)httpRequestException.StatusCode!;
                if (exception != null)
                {
                    try
                    {

                        errorMessage = JsonSerializer.Deserialize<ErrorResponse>(exception.Message) ?? throw new ArgumentNullException();

                    }
                    catch
                    {
                        errorMessage.Message = exception.Message;
                    }
                    stackTrace = exception.StackTrace;
                }
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                status = HttpStatusCode.NotImplemented;
                errorMessage.Message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(UnauthorizedAccessException))
            {
                status = HttpStatusCode.Unauthorized;
                errorMessage.Message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else if (exceptionType == typeof(KeyNotFoundException))
            {
                status = HttpStatusCode.Unauthorized;
                errorMessage.Message = exception.Message;
                stackTrace = exception.StackTrace;
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
                errorMessage.Message = exception.Message;
                stackTrace = exception.StackTrace;
            }


            var exceptionResult = JsonSerializer.Serialize(errorMessage);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            return context.Response.WriteAsync(exceptionResult);
        }
    }

}
