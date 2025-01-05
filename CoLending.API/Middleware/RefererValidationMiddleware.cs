namespace CoLending.API.Middleware
{
    public class RefererValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RefererValidationMiddleware> _logger;
        private readonly List<string> _allowedReferrers;

        public RefererValidationMiddleware(RequestDelegate next, ILogger<RefererValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _allowedReferrers = new List<string>{
            "http://localhost:4200",
                        "http://172.26.101.56/",
                        "http://172.26.101.56:8080/",
                        "https://test.saarthi.tmf.co.in/",
                        "https://saarthi.tmf.co.in/",
                        "https://localhost:7215/"
            // Add more allowed domains as needed
            };
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("Referer", out var referrer) || string.IsNullOrEmpty(referrer))
            {
                _logger.LogWarning("Invalid referrer header.");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid referrer header.");
                return;
            }

            // Add additional validation for referrer value if necessary
            if (!IsValidReferrer(referrer))
            {
                _logger.LogWarning("Referrer header not valid.");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Referrer header not valid.");
                return;
            }

            await _next(context);
        }

        private bool IsValidReferrer(string referrer)
        {
            return _allowedReferrers.Any(allowedReferrer => referrer.StartsWith(allowedReferrer, StringComparison.OrdinalIgnoreCase));
        }
    }
}
