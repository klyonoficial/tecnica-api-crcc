
namespace Tecnica.Domain.Dtos
{
    public class UsuarioDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public bool Ativo { get; set; }
        public bool DeveAlterarSenha { get; set; }
    }
}
