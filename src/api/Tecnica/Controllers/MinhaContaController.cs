using Microsoft.AspNetCore.Mvc;
using Tecnica.Domain.Daos;
using Tecnica.Dtos;

namespace Tecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MinhaContaController : ControllerBase
    {
        private readonly UsuarioDao _usuarioDao;
        private readonly ApplicationDbContext _context;

        public MinhaContaController(UsuarioDao usuarioDao, ApplicationDbContext context)
        {
            _usuarioDao = usuarioDao;
            _context = context;
        }

        [HttpPost("alterarSenha")]
        public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaDto alterarSenhaDto)
        {
            var usuario = await _usuarioDao.ObterPorIdAsync(0);
            if (usuario == null)
                return NotFound();

            usuario.AlterarSenha(alterarSenhaDto.NovaSenha);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}