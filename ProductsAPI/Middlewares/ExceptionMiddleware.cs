using System.Net;
using System.Security;
using System.Text.Json;
using ProductsModels.Exceptions;

namespace ProductsAPI.Middlewares
{
    public class ExceptionMiddleware
    {

        private readonly RequestDelegate _next;

        public ExceptionMiddleware(
            RequestDelegate next
        )
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            int statusCode = exception switch
            {
                UnauthorizedAccessException _ => (int)HttpStatusCode.Unauthorized,
                SecurityException _ => (int)HttpStatusCode.Forbidden,
                AlreadyExistsException _ => (int)HttpStatusCode.Conflict,
                ConfigurationException _ => (int)HttpStatusCode.InternalServerError,
                NotAllowedException _ => (int)HttpStatusCode.Forbidden,
                NotFoundException _ => (int)HttpStatusCode.NotFound,
                ValidationException _ => (int)HttpStatusCode.BadRequest,
        
                _=> (int)HttpStatusCode.InternalServerError
            };
            
            context.Response.StatusCode = statusCode;

            var result = JsonSerializer.Serialize(new { isSuccess = false , error = exception.Message , status = statusCode });

            return context.Response.WriteAsync(result);
        }
        
    }
};

