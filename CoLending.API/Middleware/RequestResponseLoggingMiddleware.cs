using CoLending.Core.Options;
using CoLending.Logs;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace CoLending.API.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILog _log;
        private readonly ConfigurationOptions _fleetConfigurationOptions;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILog log, IOptions<ConfigurationOptions> fleetConfigurationOptions)
        {
            _next = next;
            _log = log;
            _fleetConfigurationOptions = fleetConfigurationOptions.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            //context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
            //String refererHeader = context.Request.Headers.Referer.ToString();//context.Response.Headers.Referer.ToString();

            //String refererOrigin = context.Request.Headers.Origin.ToString();

            //if (refererHeader != string.Empty && refererHeader != refererOrigin)
            //{
            //    HttpStatusCode statusCode = HttpStatusCode.BadRequest;
            //    await context.Response.WriteAsync(statusCode.ToString());
            //}
            //else
            //{
            context.Request.Headers.TryGetValue("Authorization", out var Token);
            if (!string.IsNullOrEmpty(Token))
            {
                var token = new JwtSecurityTokenHandler().ReadJwtToken(Token.ToString().Split(' ')[1]);
                context.Items["BPno"] = token.Claims.First(c => c.Type == "BPNumber").Value;
                context.Items["userType"] = token.Claims.First(c => c.Type == "Role").Value;
            }

            LogModel requestModel = await AddLog(context);
            var requestId = await _log.AddLogs(requestModel);

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            var response = await FormatResponse(context.Response);

            LogModel updateLogModel = await UpdateLog(requestId, context);
            await _log.UpdateLogs(updateLogModel);

            await responseBody.CopyToAsync(originalBodyStream);
            //}
        }

        private static async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length)).ConfigureAwait(false);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);
            return bodyAsText;
        }

        private static async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }

        private async Task<LogModel> AddLog(HttpContext context)
        {
            // context.Request.Headers.TryGetValue("BpNo", out var bpNo);
            //context.Request.Headers.TryGetValue("UserType", out var userType);
            LogModel logModel = new LogModel();
            logModel.Url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
            logModel.RequestHttpVerb = context.Request.Method;
            logModel.RequestBody = await FormatRequest(context.Request);
            logModel.CreatedBy = "Deepak"; //context.Items["BPno"]as string
            logModel.BpNo = context.Items["BPno"] as string ?? "41";
            logModel.UserType = context.Items["userType"] as string ?? "User";
            logModel.CreatedDate = DateTime.Now;
            return logModel;
        }

        private bool IsSuccess(HttpContext context)
        {
            if (context.Response.StatusCode == (int)HttpStatusCode.Created || context.Response.StatusCode == (int)HttpStatusCode.OK || context.Response.StatusCode == (int)HttpStatusCode.NoContent)
            {
                return true;
            }
            return false;
        }

        private async Task<LogModel> UpdateLog(Guid id, HttpContext context)
        {
            LogModel logModel = new LogModel();
            logModel.ResponseBody = await FormatResponse(context.Response);
            logModel.IsSuccess = IsSuccess(context);
            logModel.ResponseStatusCode = context.Response.StatusCode;
            logModel.UpdatedDate = DateTime.Now;
            logModel.Id = id;

            return logModel;
        }
    }
}
