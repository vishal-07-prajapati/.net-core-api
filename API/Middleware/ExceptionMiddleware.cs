using System.Net;
using System.Text;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;

            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log Error

                _logger.LogError(ex, ex.Message);

                // Response

                context.Response.ContentType =
                    "application/json";

                context.Response.StatusCode =
                    (int)HttpStatusCode.InternalServerError;


                // you can add file creation or external saving like elmah to properly manage errors
                #region .txt file for errors
                var errorId = Guid.NewGuid().ToString();

                var logFolder = Path.Combine(_env.ContentRootPath, "ErrorLogs");

                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }

                var filePath = Path.Combine(logFolder, $"{errorId}.txt");

                var errorMessage = $@"
Date: {DateTime.Now}
Error Id: {errorId}

Message:
{ex.Message}

StackTrace:
{ex.StackTrace}

Inner Exception:
{ex.InnerException}
";

                await File.WriteAllTextAsync(filePath, errorMessage);

                #endregion


                await context.Response.WriteAsJsonAsync(
                    new
                    {
                        Success = false,
                        Message = "Something went wrong.",
                        ErrorId = errorId
                    });
            }
        }
    }
}
