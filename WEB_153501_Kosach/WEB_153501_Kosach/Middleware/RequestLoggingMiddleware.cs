namespace WEB_153501_Kosach.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, 
            ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode < 200 || context.Response.StatusCode >= 300)
            {
                var logInformation = new
                {
                    RequestPath = context.Request.Path,
                    ResponseStatusCode = context.Response.StatusCode
                };
                string date = DateTime.UtcNow.Date.ToString();

                //_logger.Warning("Request-Response Information: {@Information}", logInformation);
                _logger.LogInformation($"----------> request {logInformation.RequestPath} returns {logInformation.ResponseStatusCode}");
            }
        }
    }
}
