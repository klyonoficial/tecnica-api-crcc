namespace Tecnica.Infra
{
    public class ErrorResponse
    {
        public string Mensagem { get; set; } = string.Empty;

        public ErrorResponse(string mensagem)
        {
            Mensagem = mensagem;
        }
    }
}
