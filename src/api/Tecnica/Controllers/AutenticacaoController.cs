using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tecnica.Domain.Daos;
using Tecnica.Dtos;

namespace Tecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly UsuarioDao _usuarioDao;
        private readonly ApplicationDbContext _context;

        public AutenticacaoController(UsuarioDao usuarioDao, ApplicationDbContext context)
        {
            _usuarioDao = usuarioDao;
            _context = context;
        }

        [HttpPost("autenticar")]
        public async Task<IActionResult> Autenticar([FromBody] AutenticacaoDto autenticacaoDto)
        {
            var usuario = await _usuarioDao.AutenticarAsync(autenticacaoDto.Login, autenticacaoDto.Senha);
            
            var resultado = new AutenticacaoResultDto();
            
            if (usuario == null)
            {
                resultado.Sucesso = false;
                resultado.Mensagem = "Usuário ou senha inválidos.";
                return Unauthorized(resultado);
            }

            resultado.Id = usuario.Id;
            resultado.Nome = usuario.Nome;
            resultado.Email = usuario.Email;
            resultado.DeveAlterarSenha = usuario.DeveAlterarSenha;
            resultado.Sucesso = true;
            resultado.Mensagem = "Autenticação realizada com sucesso.";

            return Ok(resultado);
        }
    }
}