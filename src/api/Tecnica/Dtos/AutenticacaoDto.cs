using System.ComponentModel.DataAnnotations;

namespace Tecnica.Dtos
{
    public class AutenticacaoDto
    {
        public string Login { get; set; } = string.Empty;

        public string Senha { get; set; } = string.Empty;
    }

    public class AutenticacaoResultDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool DeveAlterarSenha { get; set; }
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
    }
}