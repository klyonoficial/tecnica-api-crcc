using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tecnica.Domain.Daos;
using Tecnica.Domain.Dtos;
using Tecnica.Domain.Entities;

namespace Tecnica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioDao _usuarioDao;
        private readonly ApplicationDbContext _context;

        public UsuariosController(UsuarioDao usuarioDao, ApplicationDbContext context)
        {
            _usuarioDao = usuarioDao;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _usuarioDao.ListarTodosAsync();
            return Ok(usuarios.Select(ToDto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await _usuarioDao.ObterPorIdAsync(id);
            if (usuario == null)
                return NotFound();
            return Ok(ToDto(usuario));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsuario([FromBody] UsuarioDto usuarioDto)
        {
            var usuario = ToEntity(usuarioDto);
            await _usuarioDao.AdicionarAsync(usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id });
        }

        private UsuarioDto ToDto(Usuario usuario)
        {
            return new UsuarioDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Ativo = usuario.Ativo,
                Perfil = usuario.Perfil
            };
        }

        private Usuario ToEntity(UsuarioDto usuarioDto)
        {
            return new Usuario(usuarioDto.Nome, usuarioDto.Email, "senhaFixa123", usuarioDto.Perfil);
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateUsuario([FromBody] UsuarioDto usuarioDto)
        {
            var usuario = await _usuarioDao.ObterPorIdAsync(usuarioDto.Id);
            if (usuario == null)
                return NotFound();

            usuario.Atualizar(usuarioDto.Nome, usuarioDto.Email, usuarioDto.Ativo, usuarioDto.Perfil);

            await _context.SaveChangesAsync();


            return Ok();
        }
    }
}