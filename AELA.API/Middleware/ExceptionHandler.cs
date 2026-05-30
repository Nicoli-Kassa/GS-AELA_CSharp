using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace AELA.API.Middleware
{
    // Tratamento GLOBAL de exceções
    // Captura erros em qualquer endpoint, loga e devolve resposta padronizada
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext context,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // Escolhe status e mensagem conforme o TIPO de erro.
            var (status, mensagem) = exception switch
            {
                DbUpdateException => (503, "Erro ao acessar o banco de dados. Tente novamente."),
                FormatException => (400, "Formato de dado inválido em um dos campos enviados."),
                KeyNotFoundException => (404, "Recurso não encontrado."),
                _ => (500, "Erro interno inesperado.")
            };

            // Loga o erro completo no servidor (mas NÃO envia ao cliente).
            _logger.LogError(exception, "Erro tratado: {Mensagem}", mensagem);

            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(new
            {
                status,
                erro = mensagem, 
                horario = DateTime.UtcNow
            }, cancellationToken);

            // Marca o erro como tratado
            return true; 
        }
    }
}
