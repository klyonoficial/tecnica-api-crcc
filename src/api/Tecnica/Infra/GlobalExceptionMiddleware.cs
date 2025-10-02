using System.Net;
using System.Text.Json;
using Serilog;

namespace Tecnica.Infra
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
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
                Log.Error(ex, "Ocorreu uma exceção não tratada: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var response = new ErrorResponse(GetErrorMessage(exception));
            
            context.Response.StatusCode = GetStatusCode(exception);
            
            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        private static string GetErrorMessage(Exception exception)
        {
            return exception switch
            {
                DomainException domainEx => GetDomainExceptionMessage(domainEx),
                ArgumentNullException => "Parâmetro obrigatório não fornecido.",
                ArgumentException => "Parâmetro inválido fornecido.",
                InvalidOperationException => "Operação inválida.",
                UnauthorizedAccessException => "Acesso não autorizado.",
                _ => "Ocorreu um erro interno no servidor."
            };
        }

        private static string GetDomainExceptionMessage(DomainException domainEx)
        {
            if (domainEx.Messages?.Any() == true)
            {
                var firstMessage = domainEx.Messages.First();
                return firstMessage.UserMessage ?? firstMessage.ProgrammerMessage ?? domainEx.Message;
            }
            return domainEx.Message;
        }

        private static int GetStatusCode(Exception exception)
        {
            return exception switch
            {
                DomainException => (int)HttpStatusCode.BadRequest,
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                InvalidOperationException => (int)HttpStatusCode.BadRequest,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };
        }
    }
}
